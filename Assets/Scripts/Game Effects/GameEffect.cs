using System;
using System.Threading;
using UnityEngine;

public abstract class GameEffect : ScriptableObject, IGameEffect
{
    public abstract void Execute(GameObject source, Action callback = null);
}