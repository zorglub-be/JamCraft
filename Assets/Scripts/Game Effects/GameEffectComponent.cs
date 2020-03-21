using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEffectComponent : MonoBehaviour
{
    [SerializeField] private GameEffect _effect;
    [SerializeField] private GameObject _sourceObject;
    public UnityEvent OnEffect;

    public void Execute()
    {
        _effect.Execute(_sourceObject, () => OnEffect?.Invoke());
    }

    public void OnValidate()
    {
        if (ReferenceEquals(_sourceObject, null))
            _sourceObject = gameObject;
    }
}

