using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Efects/Delay Effect")]
public class DelayEffect : GameEffect
{
    public float delay;
    public async override void Execute(GameObject source, Action callback = null, CancellationTokenSource tokenSource = null)
    {
        var token = tokenSource?.Token ?? GameState.Instance.CancellationToken;
        var wait = WaitForSeconds(delay, tokenSource);
        await wait;
        if (token.IsCancellationRequested)
            return;
        callback?.Invoke();
    }
    
}