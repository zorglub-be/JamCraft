using System;
using UnityEngine;

public class ChaserAi : AiManager
{
    [SerializeField] private GameEffect[] _attackEffects;
    [SerializeField] private float _attackRange;
    [SerializeField] private bool _betterAim;
    private IdleState _idle;
    private ChaseState _chase;
    private AttackState _attack;
    private SpawnPoint[] _projectileSpawners;

    protected override void Tick()
    {
        if (_projectileSpawners.Length == 0 || ReferenceEquals(Target, null))
            return;
        
        var targetPosition = Target.transform.position;
        foreach (var spawner in _projectileSpawners)
        {
            var angle = Vector3.SignedAngle(Vector3.right, targetPosition - spawner.transform.position, Vector3.forward);
            spawner.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
    protected override void Initialize()
    {
        _idle = new IdleState();
        _chase = new ChaseState(()=>Target ,GetComponentInChildren<MovementController>());
        _attack = new AttackState(_attackEffects, gameObject);
    }

    private void Start()
    {
        _projectileSpawners = GetComponentsInChildren<SpawnPoint>(true);
    }

    protected override void InitializeStateMachine(out StateMachine newStateMachine)
    {
        newStateMachine = new StateMachine();
        newStateMachine.AddStateChange(_idle, _chase, () => Detected);
        newStateMachine.AddStateChange(_chase, _idle, () => !Detected);
        newStateMachine.AddStateChange(_chase, _attack, InAttackRange);
        newStateMachine.AddStateChange(_attack, _idle, () => _attack.Finished);
        newStateMachine.SetState(_idle);
    }

    private bool InAttackRange()
    {
        return Vector2.Distance(transform.position, Target.transform.position) < _attackRange;
    }
}
