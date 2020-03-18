using System;
using UnityEngine;

public class ChaseState : IState
{
    private Transform _targetTransform;
    private Transform _agentTransform;
    private MovementController _agentController;
    private float _errorMargin = 0.2f;
    private Func<GameObject> Target;

    public ChaseState(Func<GameObject>  target, MovementController agentController)
    {
        Target = target;
        _agentController = agentController;
        _agentTransform = agentController.transform;
    }
    
    public void Tick()
    {
        var targetDirection = _targetTransform.position - _agentTransform.position;
        _agentController.Horizontal =
            (Mathf.Abs(targetDirection.x) < _errorMargin) ? 0f : Mathf.Sign(targetDirection.x);
        _agentController.Vertical =
            (Mathf.Abs(targetDirection.y) < _errorMargin) ? 0f : Mathf.Sign(targetDirection.y);
    }


    public void OnEnter()
    {
        _targetTransform = Target().transform;
    }

    public void OnExit()
    {
        _agentController.Horizontal = 0;
        _agentController.Vertical = 0;
    }
}