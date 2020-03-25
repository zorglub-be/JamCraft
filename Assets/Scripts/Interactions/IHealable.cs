public interface IHealable
{
    IntEvent OnHealed {get;}
    void Heal(int value);}