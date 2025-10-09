using UnityEngine;

public class HealthComponentController : MonoBehaviour
{
    [SerializeField] private MonoBehaviour[] componentsToDisable;
    [SerializeField] private bool disableCollidersOnDeath = true;
    
    private Health _health;
    private Collider[] _colliders;

    private void Awake()
    {
        _health = GetComponent<Health>();
        if (!_health)
        {
            Debug.LogError($"HealthComponentController on {name}: Health component not found!", this);
        }
        
        if (disableCollidersOnDeath)
        {
            _colliders = GetComponentsInChildren<Collider>();
        }
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
        CharacterEvents.BroadcastDeath(gameObject);
        SetComponentsEnabled(false);
        SetCollidersEnabled(false);
    }

    private void OnRevived()
    {
        SetCollidersEnabled(true);
        SetComponentsEnabled(true);
        CharacterEvents.BroadcastRevival(gameObject);
    }

    private void SetComponentsEnabled(bool isEnabled)
    {
        foreach (var component in componentsToDisable)
        {
            if (component != null)
            {
                component.enabled = isEnabled;
            }
        }
    }
    
    private void SetCollidersEnabled(bool isEnabled)
    {
        if (!disableCollidersOnDeath || _colliders == null) return;
        
        foreach (var col in _colliders)
        {
            if (col != null)
            {
                col.enabled = isEnabled;
            }
        }
    }
}
