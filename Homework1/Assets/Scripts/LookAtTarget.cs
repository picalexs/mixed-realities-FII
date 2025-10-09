using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private bool rotateOnlyAroundY = true;
    
    private CombatController _combatController;

    private void Awake()
    {
        _combatController = GetComponent<CombatController>();
        if (!_combatController)
        {
            Debug.LogError($"LookAtTarget on {name}: CombatController not found on this GameObject!", this);
        }
    }

    private void OnEnable()
    {
        if (!_combatController) return;
        _combatController.onTargetAcquired.AddListener(SetTarget);
        _combatController.onTargetLost.AddListener(ClearTarget);
    }
    
    private void OnDisable()
    {
        if (!_combatController) return;
        _combatController.onTargetAcquired.RemoveListener(SetTarget);
        _combatController.onTargetLost.RemoveListener(ClearTarget);
    }

    private void SetTarget(Transform setTarget)
    {
        target = setTarget;
    }

    private void ClearTarget()
    {
        target = null;
    }

    private void Update()
    {
        RotateTowardsTarget();
    }

    private void RotateTowardsTarget()
    {
        if (!target) return;

        var directionToTarget = target.position - transform.position;

        if (rotateOnlyAroundY)
        {
            directionToTarget.y = 0;
        }

        if (directionToTarget.sqrMagnitude < 0.01f) return;

        var targetRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
