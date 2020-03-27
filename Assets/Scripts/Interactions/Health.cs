using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, IDamageable, IHealable, IKillable
{
    //Inspector
    [SerializeField] private int _maxHealth;
    //Privates
    [SerializeField] private int _currentHealth;
    //Properties
    public int CurrentHealth => _currentHealth;
    //Events
    [SerializeField] private UnityEvent _onChanged;
    [SerializeField] private IntEvent _onDamaged;
    [SerializeField] private IntEvent _onHealed;
    [SerializeField] private UnityEvent _onKilled;
    public UnityEvent OnChanged => _onChanged;
    public IntEvent OnDamaged => _onDamaged;
    public IntEvent OnHealed => _onHealed;
    public UnityEvent OnKilled => _onKilled;
    public int MaximumHealth => _maxHealth;

    void OnEnable()
    {
        _currentHealth = _maxHealth;
    }

    [ContextMenu("Test Damage")]
    private void TestDamage()
    {
        TakeDamage(1);
    }
    
    public void TakeDamage(int value)
    {
        if (_currentHealth <= 0 || value == 0)
            return;
        if (value < 0)
        {
            Heal(-value);
        }
        var actualDamage = Mathf.Min(_currentHealth, value);
        _currentHealth -= actualDamage;
        OnDamaged?.Invoke(actualDamage);
        OnChanged?.Invoke();
        if (_currentHealth <= 0)
            Kill();
    }

    public void Heal(int value)
    {
        if (_currentHealth >= _maxHealth || value == 0)
            return;
        if (value < 0)
        {
            TakeDamage(-value);
        }
        var actualHealing = Mathf.Min(_maxHealth - _currentHealth, value);
        _currentHealth += actualHealing;
        OnChanged?.Invoke();
        OnHealed?.Invoke(actualHealing);
    }

    public void SetHealth(int value)
    {
        if (value == _currentHealth)
            return;
        _currentHealth = Mathf.Clamp(value, 0, _maxHealth);
        OnChanged?.Invoke();
        if(_currentHealth == 0)
            Kill();
    }
    public void Kill()
    {
        OnKilled?.Invoke();
    }

    public void IncreaseMaxHealth(int amount)
    {
        _maxHealth += amount;
        OnChanged.Invoke();
    }
}