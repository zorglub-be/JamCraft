using UnityEngine;

public abstract class AiManager : MonoBehaviour
{
    private StateMachine _stateMachine;
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

public static class GameUtils
{
    public static bool IsInMask(GameObject gameObject, LayerMask layerMask)
    {
        return (layerMask | 1 << gameObject.layer) == layerMask;
    }
}