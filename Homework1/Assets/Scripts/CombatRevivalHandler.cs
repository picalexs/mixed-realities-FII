using UnityEngine;

public class CombatRevivalHandler : MonoBehaviour
{
    private Health _health;
    private ProximityDetector[] _proximityDetectors;

    private void Awake()
    {
        _health = GetComponent<Health>();
        if (!_health)
        {
            Debug.LogError($"CombatRevivalHandler on {name}: Health component not found!", this);
        }
        _proximityDetectors = GetComponentsInChildren<ProximityDetector>();
    }

    private void OnEnable()
    {
        if (!_health) return;
        
        _health.onDeath.AddListener(OnDeath);
        _health.onRevived.AddListener(OnRevived);
    }

    private void OnDisable()
    {
        if (!_health) return;
        
        _health.onDeath.RemoveListener(OnDeath);
        _health.onRevived.RemoveListener(OnRevived);
    }

    private void OnDeath()
    {
        foreach (var detector in _proximityDetectors)
        {
            var detectorCollider = detector.GetComponent<Collider>();
            if (detectorCollider)
            {
                detectorCollider.enabled = false;
            }
        }
    }

    private void OnRevived()
    {
        foreach (var detector in _proximityDetectors)
        {
            var detectorCollider = detector.GetComponent<Collider>();
            if (!detectorCollider) continue;
            
            detectorCollider.enabled = false;
            detectorCollider.enabled = true;
        }
    }
}
