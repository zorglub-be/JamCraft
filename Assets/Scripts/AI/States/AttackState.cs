using System;
using UnityEngine;

public class AttackState : IState
{
    private bool _finished;
    private GameEffect[] _effects;
    private GameObject _gameObject;
    public Action Finished;

    public AttackState(GameEffect[] effects, GameObject gameObject)
    {
        _effects = effects;
        _gameObject = gameObject;
    }

    public void Tick()
    {
    }

    public void OnEnter()
    {
        foreach (var effect in _effects)
        {
            effect.Execute(_gameObject);
        }
    }

    public void OnExit()
    {
        //nothing to do here
    }

}