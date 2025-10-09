using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class CombatController : MonoBehaviour
{
    [SerializeField] private ProximityDetector detectionRange;
    [SerializeField] private ProximityDetector attackRange;
    
    public UnityEvent<Transform> onTargetAcquired = new UnityEvent<Transform>();
    public UnityEvent onTargetLost = new UnityEvent();
    public UnityEvent onCombatStarted = new UnityEvent();
    public UnityEvent onCombatEnded = new UnityEvent();

    private readonly HashSet<GameObject> _detectedObjects = new HashSet<GameObject>();
    private readonly HashSet<GameObject> _attackableObjects = new HashSet<GameObject>();
    private readonly Dictionary<GameObject, ProximityDetector[]> _characterDetectorCache = new Dictionary<GameObject, ProximityDetector[]>();

    private void Awake()
    {
        if (!detectionRange)
        {
            Debug.LogError($"CombatController on {name}: Detection Range ProximityDetector not assigned!", this);
        }
        if (!attackRange)
        {
            Debug.LogWarning($"CombatController on {name}: Attack Range ProximityDetector not assigned.", this);
        }
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
        
        CharacterEvents.OnCharacterDied += OnAnyCharacterDied;
        CharacterEvents.OnCharacterRevived += OnAnyCharacterRevived;
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
        
        CharacterEvents.OnCharacterDied -= OnAnyCharacterDied;
        CharacterEvents.OnCharacterRevived -= OnAnyCharacterRevived;
    }
    
    private void OnAnyCharacterDied(GameObject character)
    {
        if (character == gameObject) return;
        
        if (!_characterDetectorCache.TryGetValue(character, out var proximityDetectors))
        {
            proximityDetectors = character.GetComponentsInChildren<ProximityDetector>();
            _characterDetectorCache[character] = proximityDetectors;
        }
        
        foreach (var detector in proximityDetectors)
        {
            RemoveTarget(detector.gameObject);
        }
    }
    
    private void OnAnyCharacterRevived(GameObject character)
    {
        _characterDetectorCache.Remove(character);
        ValidateTargetDistances();
    }
    
    private void ValidateTargetDistances()
    {
        if (!detectionRange || !attackRange) return;
        
        var detectionCollider = detectionRange.GetComponent<Collider>();
        var attackCollider = attackRange.GetComponent<Collider>();
        if (!detectionCollider || !attackCollider) return;
        
        _detectedObjects.RemoveWhere(obj =>
        {
            if (!obj) return true;
            
            var objCollider = obj.GetComponent<Collider>();
            if (!objCollider) return true;
            return !detectionCollider.bounds.Intersects(objCollider.bounds);
        });
        
        var attackTargetsToRemove = new List<GameObject>();
        foreach (var obj in _attackableObjects)
        {
            if (!obj)
            {
                attackTargetsToRemove.Add(obj);
                continue;
            }
            
            var objCollider = obj.GetComponent<Collider>();
            if (!objCollider || !attackCollider.bounds.Intersects(objCollider.bounds))
            {
                attackTargetsToRemove.Add(obj);
            }
        }
        
        foreach (var obj in attackTargetsToRemove)
        {
            RemoveTarget(obj);
        }
        
        if (_detectedObjects.Count == 0)
        {
            onTargetLost.Invoke();
        }
    }

    private void OnTargetDetected(GameObject target)
    {
        _detectedObjects.Add(target);
    }
    
    private void OnTargetLost(GameObject target)
    {
        _detectedObjects.Remove(target);
        if (_detectedObjects.Count == 0)
        {
            onTargetLost.Invoke();
        }
    }

    private void OnTargetInAttackRange(GameObject target)
    {
        _attackableObjects.Add(target);
        if (_attackableObjects.Count == 1)
        {
            onCombatStarted.Invoke();
        }
    }

    private void OnTargetLeftAttackRange(GameObject target)
    {
        _attackableObjects.Remove(target);
        if (_attackableObjects.Count == 0)
        {
            onCombatEnded.Invoke();
        }
    }

    private void RemoveTarget(GameObject target)
    {
        _detectedObjects.Remove(target);
        var wasInAttack = _attackableObjects.Remove(target);
        
        if (_detectedObjects.Count == 0)
        {
            onTargetLost.Invoke();
        }
        
        if (_attackableObjects.Count == 0 && wasInAttack)
        {
            onCombatEnded.Invoke();
        }
    }

    private GameObject GetClosestTarget()
    {
        GameObject closest = null;
        var closestDistance = float.MaxValue;
        
        _detectedObjects.RemoveWhere(obj => !obj);
        
        foreach (var obj in _detectedObjects)
        {
            if (!obj) continue;
            var distance = Vector3.Distance(transform.position, obj.transform.position);
            if (!(distance < closestDistance)) continue;
            
            closestDistance = distance;
            closest = obj;
        }

        return closest;
    }

    private float _targetTimer;
    [SerializeField] private float updateTargetInterval = 0.5f;

    private void Update()
    {
        _targetTimer += Time.deltaTime;
        if (_targetTimer < updateTargetInterval) return;
        _targetTimer = 0f;

        var closestTarget = GetClosestTarget();
        
        if (!closestTarget) return;

        var targetTransform = closestTarget.transform.parent
            ? closestTarget.transform.parent
            : closestTarget.transform;

        onTargetAcquired.Invoke(targetTransform);
    }
}