using System;
using System.Collections.Generic;

public class StateMachine
{
    private List<ChangeState> _stateChangeStates = new List<ChangeState>();
    private List<ChangeState> _anyStateChange = new List<ChangeState>();

    private List<IState> _states = new List<IState>();
    private IState _currentState;
    public IState CurrentState => _currentState;
    public event Action<IState> OnStateChanged;

    public void AddStateChange(IState from, IState to, Func<bool> condition)
    {
        var changeState = new ChangeState(from, to, condition);
        _stateChangeStates.Add(changeState);
    }

    public void AddAnyStateChange(IState to, Func<bool> condition)
    {
        var changeState = new ChangeState(null, to, condition);
        _anyStateChange.Add(changeState);
    }

    public void SetState(IState state)
    {
        if (_currentState == state)
            return;

        _currentState?.OnExit();
        _currentState = state;
//        Debug.Log($"Change to state {state}"); 
        _currentState.OnEnter();
        OnStateChanged?.Invoke(_currentState);
    }

    public void Tick()
    {
        ChangeState changeState = CheckForStateChange();
        if (changeState != null)
        {
            SetState(changeState.To);
        }

        _currentState.Tick();
    }

    private ChangeState CheckForStateChange()
    {
        foreach (var changeState in _anyStateChange)
        {
            if (changeState.Condition())
                return changeState;
        }
        foreach (var changeState in _stateChangeStates)
        {
            if (changeState.From == _currentState && changeState.Condition())
            {
                return changeState;
            }
        }

        return null;
    }
}