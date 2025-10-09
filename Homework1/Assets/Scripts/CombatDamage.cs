using UnityEngine;

public class CombatDamage : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] private float damageAmount = 10f;
    
    private CombatController _combatController;
    private Health _health;
    private Health _targetHealth;
    private Transform _currentTarget;
    private bool _isInCombat;

    private void Awake()
    {
        _combatController = GetComponent<CombatController>();
        if (!_combatController)
        {
            Debug.LogError($"CombatDamage on {name}: CombatController not found!", this);
        }

        _health = GetComponent<Health>();
        if (!_health)
        {
            Debug.LogError($"CombatDamage on {name}: Health component not found!", this);
        }
    }

    private void OnEnable()
    {
        if (!_combatController) return;
        
        _combatController.onTargetAcquired.AddListener(OnTargetAcquired);
        _combatController.onCombatStarted.AddListener(OnCombatStarted);
        _combatController.onCombatEnded.AddListener(OnCombatEnded);
        
        if (_health)
        {
            _health.onDeath.AddListener(OnSelfDeath);
        }
    }

    private void OnDisable()
    {
        if (!_combatController) return;
        
        _combatController.onTargetAcquired.RemoveListener(OnTargetAcquired);
        _combatController.onCombatStarted.RemoveListener(OnCombatStarted);
        _combatController.onCombatEnded.RemoveListener(OnCombatEnded);
        
        if (_health)
        {
            _health.onDeath.RemoveListener(OnSelfDeath);
        }
        
        UnsubscribeFromTargetDeath();
    }

    private void OnTargetAcquired(Transform target)
    {
        UnsubscribeFromTargetDeath();
        
        _currentTarget = target;
        _targetHealth = target.GetComponent<Health>();
        if (!_targetHealth)
        {
            Debug.LogWarning($"CombatDamage on {name}: Target {target.name} has no Health component!", this);
            return;
        }
        
        _targetHealth.onDeath.AddListener(OnTargetDeath);
    }

    private void UnsubscribeFromTargetDeath()
    {
        if (!_targetHealth) return;
        _targetHealth.onDeath.RemoveListener(OnTargetDeath);
        _targetHealth = null;
        _currentTarget = null;
    }

    private void OnTargetDeath()
    {
        if (_currentTarget && _combatController)
        {
            var proximityDetectors = _currentTarget.GetComponentsInChildren<ProximityDetector>();
            foreach (var detector in proximityDetectors)
            {
                _combatController.RemoveTarget(detector.gameObject);
            }
        }
        
        UnsubscribeFromTargetDeath();
    }

    private void OnSelfDeath()
    {
        _isInCombat = false;
        enabled = false;
    }

    private void OnCombatStarted()
    {
        _isInCombat = true;
    }

    private void OnCombatEnded()
    {
        _isInCombat = false;
    }

    public void OnAttackHit()
    {
        if (!_isInCombat || !_targetHealth || _targetHealth.IsDead) return;
        
        _targetHealth.TakeDamage(damageAmount);
    }
}
