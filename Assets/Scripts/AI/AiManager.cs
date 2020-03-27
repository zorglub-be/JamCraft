using UnityEngine;

public abstract class AiManager : MonoBehaviour
{
    protected StateMachine _stateMachine;
    protected abstract void Tick();
    protected abstract void Initialize();
    protected abstract void InitializeStateMachine(out StateMachine newStateMachine);

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