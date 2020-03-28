using UnityEngine;

public class DamageHandler : MonoBehaviour, IDamageable
{
    public IntEvent _onDamaged;
    public IntEvent OnDamaged => _onDamaged;
    public void TakeDamage(int value)
    {
        OnDamaged.Invoke(value);
    }
}
