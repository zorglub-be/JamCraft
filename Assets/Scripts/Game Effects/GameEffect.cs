using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public abstract class GameEffect : ScriptableObject, IGameEffect
{
    public abstract void Execute(GameObject source, Action callback = null);
    protected async Task WaitForSeconds(float duration)
    {
        var token = GameState.Instance.CancellationToken;
        var startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            await Task.Yield();
            if (token.IsCancellationRequested)
                return;
        }
    }
}