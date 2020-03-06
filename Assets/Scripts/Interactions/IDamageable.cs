public interface IDamageable
{
    IntEvent OnDamaged {get;}
    void TakeDamage(int value);
}