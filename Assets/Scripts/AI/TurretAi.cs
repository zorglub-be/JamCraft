using UnityEngine;

public class TurretAi : AiManager
{
    private AttackState _attack;
    private IdleState _idle;
    [SerializeField] private GameEffect[] _attackEffects;

    protected override void Tick()
    {
        //nothing to do
    }

    protected override void Initialize()
    {
        _idle = new IdleState();
        _attack = new AttackState(_attackEffects, gameObject);
    }

    protected override void InitializeStateMachine(out StateMachine newStateMachine)
    {
        newStateMachine = new StateMachine();
        newStateMachine.AddStateChange(_idle, _attack, () => Detected);
        newStateMachine.AddStateChange(_attack, _idle, () => !Detected);
    }
}