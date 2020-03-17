using System;
using UnityEngine;

public abstract class AiManager : MonoBehaviour
{
    private StateMachine _stateMachine;
    private bool _detected;
    public bool Detected => _detected;

    private void OnTriggerEnter2D(Collider2D other)
    {
        _detected = true;
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        _detected = false;
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

public class TurretAi : AiManager
{
    private AttackState _attack;
    private IdleState _idle;
    
    public bool Detected { get; private set; }


    protected override void Tick()
    {
        //nothing to do
    }

    protected override void Initialize()
    {
        _idle = new IdleState();
        _attack = new AttackState();
    }

    protected override void InitializeStateMachine(out StateMachine newStateMachine)
    {
        newStateMachine = new StateMachine();
        newStateMachine.AddStateChange(_idle, _attack, () => Detected);
        newStateMachine.AddStateChange(_attack, _idle, () => !Detected);
    }
}

internal class IdleState : IState
{
    
    
    public void Tick()
    {
        //do nothing
    }

    public void OnEnter()
    {
        //set animation to idle
    }

    public void OnExit()
    {
        //do nothing
    }
}

public class ChaseState : IState
{
    public void Tick()
    {
    }

    public void OnEnter()
    {
        throw new NotImplementedException();
    }

    public void OnExit()
    {
        throw new NotImplementedException();
    }
}

public class PatrollAiBehaviour : IState
{
    
    public void Tick()
    {
        throw new NotImplementedException();
    }

    public void OnEnter()
    {
        throw new NotImplementedException();
    }

    public void OnExit()
    {
        throw new NotImplementedException();
    }
}

public class AttackState : IState
{
    private bool _finished;
    public bool Finished => _finished;

    public void Tick()
    {
        //we just wait for the attack to finish
    }

    public void OnEnter()
    {
        //perform attack
    }

    public void OnExit()
    {
        //nothing to do here
    }

}
