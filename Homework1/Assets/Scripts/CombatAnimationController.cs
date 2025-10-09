using UnityEngine;

public class CombatAnimationController : MonoBehaviour
{
    private static readonly int InCombat = Animator.StringToHash("IsInCombat");
    private Animator _animator;
    
    private CombatController _combatController;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        if (!_animator)
        {
            Debug.LogWarning($"CombatAnimationController on {name}: No Animator component found!", this);
        }

        _combatController = GetComponent<CombatController>();
        if (!_combatController)
        {
            Debug.LogError($"CombatAnimationController on {name}: CombatController not found on this GameObject!",this);
        }
    }

    private void OnEnable()
    {
        if (!_combatController) return;
        _combatController.onCombatStarted.AddListener(EnterCombat);
        _combatController.onCombatEnded.AddListener(ExitCombat);
    }
    
    private void OnDisable()
    {
        if (!_combatController) return;
        _combatController.onCombatStarted.RemoveListener(EnterCombat);
        _combatController.onCombatEnded.RemoveListener(ExitCombat);
    }

    private void EnterCombat()
    {
        if (!_animator) return;
        _animator.SetBool(InCombat, true);
    }

    private void ExitCombat()
    {
        if (!_animator) return;
        _animator.SetBool(InCombat, false);
    }
}
