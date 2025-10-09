using UnityEngine;

public class HealthVisualFeedback : MonoBehaviour
{
    [Header("Visual Settings")]
    [SerializeField] private Renderer targetRenderer;
    [SerializeField] private bool changeColorOnDamage = true;
    [SerializeField] private Color fullHealthColor = Color.green;
    [SerializeField] private Color lowHealthColor = Color.red;
    
    [Header("Flash Settings")]
    [SerializeField] private bool flashOnDamage = true;
    [SerializeField] private Color flashColor = Color.white;
    [SerializeField] private float flashDuration = 0.2f;
    [SerializeField, Range(1f, 20f)] private float flashBrightness = 5f;
    
    private Health _health;
    private Material _material;
    private float _flashTimer;
    private bool _isFlashing;
    private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
    private static readonly int MainColor = Shader.PropertyToID("_Color");
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

    private void Awake()
    {
        _health = GetComponent<Health>();
        if (!_health)
        {
            Debug.LogError($"HealthVisualFeedback on {name}: Health component not found!", this);
        }
        if (!targetRenderer)
        {
            targetRenderer = GetComponentInChildren<Renderer>();
        }

        if (!targetRenderer) return;
        
        _material = targetRenderer.material;
        if (_material.HasProperty(EmissionColor))
        {
            _material.EnableKeyword("_EMISSION");
        }
    }

    private void OnEnable()
    {
        if (!_health) return;
        
        _health.onHealthChanged.AddListener(OnHealthChanged);
        _health.onDamaged.AddListener(OnDamaged);
        _health.onHealed.AddListener(OnHealed);
    }

    private void OnDisable()
    {
        if (!_health) return;
        
        _health.onHealthChanged.RemoveListener(OnHealthChanged);
        _health.onDamaged.RemoveListener(OnDamaged);
        _health.onHealed.RemoveListener(OnHealed);
    }

    private void Update()
    {
        if (!_isFlashing) return;
        _flashTimer -= Time.deltaTime;
        if (!(_flashTimer <= 0f)) return;
        
        _isFlashing = false;
        UpdateHealthColor();
    }

    private void OnHealthChanged(float current, float max)
    {
        if (!changeColorOnDamage || !_material || _isFlashing) return;
        UpdateHealthColor();
    }

    private void OnDamaged(float amount)
    {
        if (!flashOnDamage || !_material || _health.IsDead) return;

        _isFlashing = true;
        _flashTimer = flashDuration;
        
        var brightFlash = flashColor * flashBrightness;
        
        if (_material.HasProperty(BaseColor))
        {
            _material.SetColor(BaseColor, brightFlash);
        }
        else if (_material.HasProperty(MainColor))
        {
            _material.SetColor(MainColor, brightFlash);
        }
        else
        {
            _material.color = brightFlash;
        }
        
        if (_material.HasProperty(EmissionColor))
        {
            _material.SetColor(EmissionColor, brightFlash);
        }
    }

    private void OnHealed(float amount)
    {
        //later
    }

    private void UpdateHealthColor()
    {
        if (!_material || !_health) return;
        
        var healthPercentage = _health.HealthPercentage;
        var targetColor = Color.Lerp(lowHealthColor, fullHealthColor, healthPercentage);
        
        if (_material.HasProperty(BaseColor))
        {
            _material.SetColor(BaseColor, targetColor);
        }
        else if (_material.HasProperty(MainColor))
        {
            _material.SetColor(MainColor, targetColor);
        }
        else
        {
            _material.color = targetColor;
        }
        
        if (_material.HasProperty(EmissionColor))
        {
            _material.SetColor(EmissionColor, Color.black);
        }
    }
}
