using System;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Efects/Delay Effect")]
public class DelayEffect : GameEffect
{
    public float delay;
    public async override void Execute(GameObject source, Action callback = null)
    {
        var token = GameState.Instance.CancellationToken;
        await Task.Delay((int)(delay * 1000), token);
        callback?.Invoke();
    }
}