using System.Linq;
using UnityEngine;

public class ChaserAi : AiManager
{
    [SerializeField] private LayerMask _detectedLayers;
    [SerializeField] private string[] _detectedTags;
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
    private Collider2D[] _hitResults = new Collider2D[30];

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

    private void OnDisable()
    {
        _stateMachine.SetState(_idle);
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
            GameObject newTarget = _target;
            for (int i = 0; i < _hitResults.Length; i++)
            {
                //we've looked at all hits in the array
                if (_hitResults[i] == null)
                    break;
                if (_hitResults[i].isTrigger)
                    continue;
                var obj = _hitResults[i]?.attachedRigidbody.gameObject;
                if (obj && _detectedTags.Contains(obj.tag))
                {
                    _detected = true;
                    newTarget = obj;
                    //If we didn't have a target yet, that one will do
                    if (_target == null)
                    {
                        _target = newTarget;
                        return;
                    }
                    //If it's already our target, no need to change targets
                    if (newTarget == _target)
                        return;
                    //This is a potential target, let's look if there's others
                }
                if (newTarget != null)
                {
                    _target = newTarget;
                    return;
                }
            }
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
