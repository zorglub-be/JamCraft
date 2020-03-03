using System.Runtime.InteropServices;
using UnityEngine;

public abstract class GameEffect : ScriptableObject, IGameEffect
{
    public abstract void Execute();
}