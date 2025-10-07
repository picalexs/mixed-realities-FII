using UnityEngine;
using System.Collections.Generic;

public class CombatController : MonoBehaviour
{
    private ProximityDetector _proximityDetector;
    private LookAtTarget _lookAtTarget;

    private HashSet<GameObject> _detectedObjects;

    private void Start()
    {
        _detectedObjects = new HashSet<GameObject>();
    }

    private void Awake()
    {
        _proximityDetector = GetComponentInChildren<ProximityDetector>();
        if (_proximityDetector == null)
        {
            Debug.LogError($"CombatController on {name}: ProximityDetector not found in children!", this);
        }

        if (GetComponentInParent<Rigidbody>() == null)
        {
            Debug.LogError($"CombatController on {name}: Rigidbody not found in parent!", this);
        }

        _lookAtTarget = GetComponent<LookAtTarget>();
    }

    private void OnEnable()
    {
        if (_proximityDetector == null) return;
        _proximityDetector.onEnter.AddListener(OnEnterCombat);
        _proximityDetector.onExit.AddListener(OnExitCombat);
    }

    private void OnDisable()
    {
        if (_proximityDetector == null) return;
        _proximityDetector.onEnter.RemoveListener(OnEnterCombat);
        _proximityDetector.onExit.RemoveListener(OnExitCombat);
    }

    private void OnEnterCombat(GameObject attacker)
    {
        Debug.Log($"Entering Combat with {attacker.transform.parent?.name ?? attacker.name}");
        _detectedObjects.Add(attacker);
    }

    private void OnExitCombat(GameObject attacker)
    {
        Debug.Log($"Exiting Combat with {attacker.transform.parent?.name ?? attacker.name}");
        _detectedObjects.Remove(attacker);
    }

    private GameObject GetClosestTarget()
    {
        GameObject closest = null;
        float closestDistance = float.MaxValue;
        foreach (var obj in _detectedObjects)
        {
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

        if (_lookAtTarget == null) return;
        if (closestTarget == null)
        {
            _lookAtTarget.SetTarget(null);
            return;
        }

        Transform targetTransform = closestTarget.transform.parent != null
            ? closestTarget.transform.parent
            : closestTarget.transform;

        _lookAtTarget.SetTarget(targetTransform);
        Debug.Log($"[{gameObject.name}] Current Target: {targetTransform.name}");
        
    }
}