using UnityEngine;

public abstract class AiManager : MonoBehaviour
{
    private StateMachine _stateMachine;
    private bool _detected;
    private GameObject _target;
    public bool Detected => _detected;
    public GameObject Target => _target;

    private void OnTriggerEnter2D(Collider2D other)
    {
        _detected = true;
        _target = other.attachedRigidbody.gameObject;
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        _detected = false;
        _target = null;
    }
    
    abstract protected void Tick();
    abstract protected void Initialize();
    abstract protected void InitializeStateMachine(out StateMachine newStateMachine);

    private void Awake()
    {
        Initialize();
        InitializeStateMachine(out _stateMachine);
    }
    private void Update()
    {
        Tick();
        _stateMachine.Tick();
    }
}