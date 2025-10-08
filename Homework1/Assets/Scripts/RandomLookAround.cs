using UnityEngine;

public class RandomLookAround : MonoBehaviour
{
    [Header("Random Look Settings")]
    [SerializeField] private float minLookTime = 2f;
    [SerializeField] private float maxLookTime = 5f;
    [SerializeField] private float rotationSpeed = 2f;
    [SerializeField] private bool rotateOnlyAroundY = true;
    [SerializeField] private float lookAngleRange = 180f;
    
    private float _lookTimer = 0f;
    private Quaternion _targetRotation;
    private bool _isEnabled = true;
    private LookAtTarget _lookAtTarget;

    private void Awake()
    {
        _lookAtTarget = GetComponent<LookAtTarget>();
        PickNewRandomDirection();
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
        float randomAngle = Random.Range(-lookAngleRange / 2f, lookAngleRange / 2f);
        
        Vector3 randomDirection;
        if (rotateOnlyAroundY)
        {
            randomDirection = Quaternion.Euler(0f, randomAngle, 0f) * transform.forward;
        }
        else
        {
            float randomPitch = Random.Range(-lookAngleRange / 4f, lookAngleRange / 4f);
            randomDirection = Quaternion.Euler(randomPitch, randomAngle, 0f) * transform.forward;
        }

        _targetRotation = Quaternion.LookRotation(randomDirection);
        
        Debug.Log($"{name}: Looking in new random direction for {_lookTimer:F1} seconds");
    }
    
    public void EnableRandomLooking()
    {
        if (_isEnabled) return;
        
        _isEnabled = true;
        
        if (_lookAtTarget != null)
        {
            _lookAtTarget.SetTarget(null);
        }
        
        PickNewRandomDirection();
        Debug.Log($"{name}: Random looking enabled");
    }
    
    public void DisableRandomLooking()
    {
        if (!_isEnabled) return;
        
        _isEnabled = false;
        Debug.Log($"{name}: Random looking disabled");
    }

    public bool IsEnabled()
    {
        return _isEnabled;
    }
}

