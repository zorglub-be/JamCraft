using System;
using UnityEngine;

public class MoveState : IState
{
    private Transform _agentTransform;
    private MovementController _agentController;
    private float _errorMargin = 0.2f;
    private Vector3 _destination;
    public bool DestinationReached { get; private set; }

    public MoveState(Vector3 destination, MovementController agentController)
    {
        _destination = destination;
        _agentController = agentController;
        _agentTransform = agentController.transform;
    }
    
    public void Tick()
    {
        var targetDirection = _destination - _agentTransform.position;
        if (targetDirection.magnitude < _errorMargin)
        {
            DestinationReached = true;
            return;
        }
        _agentController.Horizontal =
            (Mathf.Abs(targetDirection.x) < _errorMargin) ? 0f : Mathf.Sign(targetDirection.x);
        _agentController.Vertical =
            (Mathf.Abs(targetDirection.y) < _errorMargin) ? 0f : Mathf.Sign(targetDirection.y);
    }


    public void OnEnter()
    {
        //do nothing
    }

    public void OnExit()
    {
        //do nothing
    }
}