using UnityEngine;

public class CombatAnimationController : MonoBehaviour
{
    private static readonly int InCombat = Animator.StringToHash("IsInCombat");
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        if (!_animator)
        {
            Debug.LogWarning($"CombatAnimationController on {name}: No Animator component found!", this);
        }
    }
    
    public void EnterCombat()
    {
        if (!_animator) return;
        
        _animator.SetBool(InCombat, true);
        Debug.Log($"{name}: Entering combat state");
    }

    public void ExitCombat()
    {
        if (!_animator) return;

        _animator.SetBool(InCombat, false);
        Debug.Log($"{name}: Exiting combat state");
    }
}
