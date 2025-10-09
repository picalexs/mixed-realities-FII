using UnityEngine;

public class DeathAnimationController : MonoBehaviour
{
    private static readonly int IsDead = Animator.StringToHash("IsDead");
    private Animator _animator;
    private Health _health;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        if (!_animator)
        {
            Debug.LogWarning($"DeathAnimationController on {name}: No Animator component found!", this);
        }

        _health = GetComponent<Health>();
        if (!_health)
        {
            Debug.LogError($"DeathAnimationController on {name}: Health component not found!", this);
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
        if (!_animator) return;
        _animator.SetBool(IsDead, true);
    }

    private void OnRevived()
    {
        if (!_animator) return;
        _animator.SetBool(IsDead, false);
    }
}

