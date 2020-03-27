using System;
using System.Threading;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Effects/Load Game")]
public class LoadGameEffect : GameEffect
{
    public override void Execute(GameObject source, Action callback = null, CancellationTokenSource tokenSource = null)
    {
        GameState.Instance.LoadGame();
        callback?.Invoke();
    }
}