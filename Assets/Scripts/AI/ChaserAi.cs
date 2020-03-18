using UnityEngine;

public class ChaserAi : AiManager
{
    [SerializeField] private GameEffect[] _attackEffects;
    [SerializeField] private float _attackRange;
    private IState _idle;
    private IState _chase;
    private IState _attack;

    protected override void Tick()
    {
        //nothing to do
    }
    protected override void Initialize()
    {
        _idle = new IdleState();
        _chase = new ChaseState(()=>Target ,GetComponentInChildren<MovementController>());
        _attack = new AttackState(_attackEffects, gameObject);
    }

    protected override void InitializeStateMachine(out StateMachine newStateMachine)
    {
        newStateMachine = new StateMachine();
        newStateMachine.AddStateChange(_idle, _chase, () => Detected);
        newStateMachine.AddStateChange(_chase, _idle, () => !Detected);
        newStateMachine.AddStateChange(_chase, _attack, InAttackRange);
        newStateMachine.AddStateChange(_attack, _idle, () => true);
        newStateMachine.SetState(_idle);
    }

    private bool InAttackRange()
    {
        return Vector2.Distance(transform.position, Target.transform.position) < _attackRange;
    }
}
