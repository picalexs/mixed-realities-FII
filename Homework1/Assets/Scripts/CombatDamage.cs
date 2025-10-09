using UnityEngine;

public class CombatDamage : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] private float damageAmount = 10f;
    
    private CombatController _combatController;
    private Health _targetHealth;
    private bool _isInCombat;

    private void Awake()
    {
        _combatController = GetComponent<CombatController>();
        if (!_combatController)
        {
            Debug.LogError($"CombatDamage on {name}: CombatController not found!", this);
        }
    }

    private void OnEnable()
    {
        if (!_combatController) return;
        
        _combatController.onTargetAcquired.AddListener(OnTargetAcquired);
        _combatController.onCombatStarted.AddListener(OnCombatStarted);
        _combatController.onCombatEnded.AddListener(OnCombatEnded);
    }

    private void OnDisable()
    {
        if (!_combatController) return;
        
        _combatController.onTargetAcquired.RemoveListener(OnTargetAcquired);
        _combatController.onCombatStarted.RemoveListener(OnCombatStarted);
        _combatController.onCombatEnded.RemoveListener(OnCombatEnded);
    }

    private void OnTargetAcquired(Transform target)
    {
        _targetHealth = target.GetComponent<Health>();
        if (!_targetHealth)
        {
            Debug.LogWarning($"CombatDamage on {name}: Target {target.name} has no Health component!", this);
        }
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
