using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth = 100f;
    [SerializeField] private bool destroyOnDeath;
    [SerializeField] private float destroyDelay = 2f;
    
    [Header("Events")]
    public UnityEvent<float, float> onHealthChanged = new UnityEvent<float, float>();
    public UnityEvent<float> onDamaged = new UnityEvent<float>();
    public UnityEvent<float> onHealed = new UnityEvent<float>();
    public UnityEvent onDeath = new UnityEvent();
    public UnityEvent onRevived = new UnityEvent();

    public bool IsDead { get; private set; }
    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public float HealthPercentage => maxHealth > 0 ? currentHealth / maxHealth : 0f;

    private void Awake()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        IsDead = currentHealth <= 0f;
        
        if (IsDead)
        {
            Debug.LogWarning($"Health on {name}: Started dead (health <= 0)", this);
        }
    }

    public void TakeDamage(float damage)
    {
        if (damage <= 0f)
        {
            Debug.LogWarning($"Health on {name}: TakeDamage called with invalid amount {damage}", this);
            return;
        }
        if (IsDead) return;

        currentHealth = Mathf.Max(0f, currentHealth - damage);
        onDamaged.Invoke(damage);
        onHealthChanged.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0f && !IsDead)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (amount <= 0f)
        {
            Debug.LogWarning($"Health on {name}: Heal called with invalid amount {amount}", this);
            return;
        }

        if (IsDead) return;

        var previousHealth = currentHealth;
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        
        if (previousHealth >= currentHealth) return;
        
        onHealed.Invoke(amount);
        onHealthChanged.Invoke(currentHealth, maxHealth);
    }

    private void SetHealth(float newHealth)
    {
        currentHealth = Mathf.Clamp(newHealth, 0f, maxHealth);
        onHealthChanged.Invoke(currentHealth, maxHealth);

        switch (currentHealth)
        {
            case <= 0f when !IsDead:
                Die();
                break;
            case > 0f when IsDead:
                Revive();
                break;
        }
    }

    public void Revive(float? healthAmount = null)
    {
        if (!IsDead)
        {
            Debug.LogWarning($"Health on {name}: Revive called but not dead", this);
            return;
        }

        IsDead = false;
        currentHealth = healthAmount.HasValue ? Mathf.Clamp(healthAmount.Value, 1f, maxHealth) : maxHealth;
        
        onRevived.Invoke();
        onHealthChanged.Invoke(currentHealth, maxHealth);
    }

    private void Die()
    {
        IsDead = true;
        onDeath.Invoke();

        if (destroyOnDeath)
        {
            Destroy(gameObject, destroyDelay);
        }
    }

    public void SetMaxHealth(float newMaxHealth)
    {
        if (newMaxHealth <= 0f)
        {
            Debug.LogWarning($"Health on {name}: SetMaxHealth called with invalid value {newMaxHealth}", this);
            return;
        }

        maxHealth = newMaxHealth;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        
        onHealthChanged.Invoke(currentHealth, maxHealth);
    }

    public void RestoreToFull()
    {
        SetHealth(maxHealth);
    }
}
