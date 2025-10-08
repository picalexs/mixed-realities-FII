using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private bool rotateOnlyAroundY = true;
    
    public void SetTarget(Transform setTarget)
    {
        target = setTarget;
    }

    private void Update()
    {
        RotateTowardsTarget();
    }

    private void RotateTowardsTarget()
    {
        if (!target) return;

        Vector3 directionToTarget = target.position - transform.position;

        if (rotateOnlyAroundY)
        {
            directionToTarget.y = 0;
        }

        if (directionToTarget.sqrMagnitude < 0.01f) return;

        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}

