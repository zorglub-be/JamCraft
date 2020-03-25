using System;
using UnityEngine;

public class ChaserAi : AiManager
{
    [SerializeField] private LayerMask _detectedLayers;
    [SerializeField] private GameEffect[] _attackEffects;
    [SerializeField] private float _detectionRange;
    [SerializeField] private float _attackRange;
    [Tooltip("If true, attacks will be precisely aimed at the target")]
    [SerializeField] private bool _aimedAttacks;
    [Tooltip("Additional delay before an attack")]
    [SerializeField] private float _attackDelay = 0.5f;
    private IdleState _idle;
    private ChaseState _chase;
    private AttackState _attack;
    private SpawnPoint[] _projectileSpawners;
    private Collider2D[] _hitResults = new Collider2D[1];

    private bool _detected;
    private GameObject _target;
    private Transform _transform;
    public bool Detected => _detected;
    public GameObject Target => _target;

    protected override void Tick()
    {
        Detect();
        if (_aimedAttacks)
            UpdateSpawnersRotation();
    }

    void UpdateSpawnersRotation()
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
    void Detect()
    {
        if (Physics2D.OverlapCircleNonAlloc(_transform.position, _detectionRange, _hitResults, _detectedLayers) > 0)
        {
            _detected = true;
            _target = _hitResults[0].attachedRigidbody.gameObject;
            return;
        }
        _detected = false;
        _target = null;
    }
    protected override void Initialize()
    {
        _transform = transform;
        _idle = new IdleState();
        _chase = new ChaseState(()=>Target ,GetComponentInChildren<MovementController>());
        _attack = new AttackState(_attackEffects, gameObject, _attackDelay);
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
        if (ReferenceEquals(_target, null))
            return false;
        return Vector2.Distance(transform.position, Target.transform.position) < _attackRange;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        var position = gameObject.transform.position;
        Gizmos.DrawWireSphere(position, _detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(position, _attackRange);
    }
}
