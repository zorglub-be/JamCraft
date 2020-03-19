using System;
using UnityEngine;

public class AttackState : IState
{
    private bool _finished;
    private GameEffect[] _effects;
    private GameObject _gameObject;
    public bool Finished => _finished;

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
        _finished = false;
        foreach (var effect in _effects)
        {
            effect.Execute(_gameObject, () => _finished = true);
        }
    }

    public void OnExit()
    {
        //nothing to do here
    }

}