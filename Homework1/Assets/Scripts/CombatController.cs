using UnityEngine;
using System.Collections.Generic;

public class CombatController : MonoBehaviour
{
    [Header("Detection Setup")]
    [SerializeField] private ProximityDetector detectionRange;
    [SerializeField] private ProximityDetector attackRange;
    
    private LookAtTarget _lookAtTarget;
    private CombatAnimationController _animationController;
    private RandomLookAround _randomLookAround;

    private readonly HashSet<GameObject> _detectedObjects = new HashSet<GameObject>();
    private readonly HashSet<GameObject> _attackableObjects = new HashSet<GameObject>();

    private void Awake()
    {
        if (detectionRange == null)
        {
            Debug.LogError($"CombatController on {name}: Detection Range ProximityDetector not assigned!", this);
        }
        if (attackRange == null)
        {
            Debug.LogWarning($"CombatController on {name}: Attack Range ProximityDetector not assigned. Will use detection range for both.", this);
        }

        _lookAtTarget = GetComponent<LookAtTarget>();
        _animationController = GetComponent<CombatAnimationController>();
        _randomLookAround = GetComponent<RandomLookAround>();
    }

    private void OnEnable()
    {
        if (detectionRange)
        {
            detectionRange.onEnter.AddListener(OnTargetDetected);
            detectionRange.onExit.AddListener(OnTargetLost);
        }

        if (attackRange)
        {
            attackRange.onEnter.AddListener(OnTargetInAttackRange);
            attackRange.onExit.AddListener(OnTargetLeftAttackRange);
        }
    }

    private void OnDisable()
    {
        if (detectionRange)
        {
            detectionRange.onEnter.RemoveListener(OnTargetDetected);
            detectionRange.onExit.RemoveListener(OnTargetLost);
        }

        if (attackRange)
        {
            attackRange.onEnter.RemoveListener(OnTargetInAttackRange);
            attackRange.onExit.RemoveListener(OnTargetLeftAttackRange);
        }
    }

    private void OnTargetDetected(GameObject target)
    {
        Debug.Log($"{name}: Target detected - {target.transform.parent?.name ?? target.name}");
        _detectedObjects.Add(target);
        
        // Disable random looking when we have a target
        if (_detectedObjects.Count == 1 && _randomLookAround != null)
        {
            _randomLookAround.DisableRandomLooking();
        }
    }
    
    private void OnTargetLost(GameObject target)
    {
        Debug.Log($"{name}: Target lost - {target.transform.parent?.name ?? target.name}");
        _detectedObjects.Remove(target);
        
        // Enable random looking when we have no more targets
        if (_detectedObjects.Count == 0 && _randomLookAround != null)
        {
            _randomLookAround.EnableRandomLooking();
        }
    }

    private void OnTargetInAttackRange(GameObject target)
    {
        Debug.Log($"{name}: Target in attack range - {target.transform.parent?.name ?? target.name}");
        _attackableObjects.Add(target);
        
        if (_attackableObjects.Count == 1 && _animationController != null)
        {
            _animationController.EnterCombat();
        }
    }

    private void OnTargetLeftAttackRange(GameObject target)
    {
        Debug.Log($"{name}: Target left attack range - {target.transform.parent?.name ?? target.name}");
        _attackableObjects.Remove(target);
        
        if (_attackableObjects.Count == 0 && _animationController != null)
        {
            _animationController.ExitCombat();
        }
    }

    private GameObject GetClosestTarget()
    {
        GameObject closest = null;
        float closestDistance = float.MaxValue;
        foreach (var obj in _detectedObjects)
        {
            if (!obj) continue;
            
            float distance = Vector3.Distance(transform.position, obj.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = obj;
            }
        }

        return closest;
    }

    private float _targetTimer = 0f;
    [SerializeField] private float updateTargetInterval = 0.5f;

    private void Update()
    {
        _targetTimer += Time.deltaTime;
        if (_targetTimer < updateTargetInterval) return;
        _targetTimer = 0f;

        GameObject closestTarget = GetClosestTarget();

        if (!_lookAtTarget) return;
        if (!closestTarget)
        {
            _lookAtTarget.SetTarget(null);
            return;
        }

        Transform targetTransform = closestTarget.transform.parent
            ? closestTarget.transform.parent
            : closestTarget.transform;

        _lookAtTarget.SetTarget(targetTransform);
        Debug.Log($"[{gameObject.name}] Current Target: {targetTransform.name}");
    }
}