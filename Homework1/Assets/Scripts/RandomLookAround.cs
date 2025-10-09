using UnityEngine;

public class RandomLookAround : MonoBehaviour
{
    [Header("Random Look Settings")]
    [SerializeField] private float minLookTime = 2f;
    [SerializeField] private float maxLookTime = 5f;
    [SerializeField] private float rotationSpeed = 2f;
    [SerializeField] private bool rotateOnlyAroundY = true;
    [SerializeField] private float lookAngleRange = 180f;
    
    private CombatController _combatController;
    
    private float _lookTimer;
    private Quaternion _targetRotation;
    private bool _isEnabled = true;

    private void Awake()
    {
        _combatController = GetComponent<CombatController>();
        if (!_combatController)
        {
            Debug.LogError($"RandomLookAround on {name}: CombatController not found on this GameObject!", this);
        }

        PickNewRandomDirection();
    }

    private void OnEnable()
    {
        if (!_combatController) return;
        _combatController.onTargetAcquired.AddListener(DisableRandomLooking);
        _combatController.onTargetLost.AddListener(EnableRandomLooking);
    }
    
    private void OnDisable()
    {
        if (!_combatController) return;
        _combatController.onTargetAcquired.RemoveListener(DisableRandomLooking);
        _combatController.onTargetLost.RemoveListener(EnableRandomLooking);
    }
    
    private void EnableRandomLooking()
    {
        _isEnabled = true;
    }

    private void DisableRandomLooking(Transform target)
    {
        _isEnabled = false;
    }

    private void Update()
    {
        if (!_isEnabled) return;

        _lookTimer -= Time.deltaTime;
        if (_lookTimer <= 0f)
        {
            PickNewRandomDirection();
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void PickNewRandomDirection()
    {
        _lookTimer = Random.Range(minLookTime, maxLookTime);
        var randomAngle = Random.Range(-lookAngleRange / 2f, lookAngleRange / 2f);
        
        Vector3 randomDirection;
        var randomPitch = Random.Range(-lookAngleRange / 4f, lookAngleRange / 4f);
        if (rotateOnlyAroundY)
        {
            randomDirection = Quaternion.Euler(0f, randomAngle, 0f) * transform.forward;
        }
        else
        {
            randomDirection = Quaternion.Euler(randomPitch, randomAngle, 0f) * transform.forward;
        }

        _targetRotation = Quaternion.LookRotation(randomDirection);
    }
}
