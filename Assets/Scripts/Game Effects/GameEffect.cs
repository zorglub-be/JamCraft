using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public abstract class GameEffect : ScriptableObject, IGameEffect
{
    public abstract void Execute(GameObject source, Action callback = null, CancellationTokenSource tokenSource = null);
    protected async Task WaitForSeconds(float duration, CancellationTokenSource tokenSource = null)
    {

        var token = tokenSource?.Token ?? GameState.Instance.CancellationToken;
        var startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            await Task.Yield();
            if (token.IsCancellationRequested)
                return;
        }
    }
}