using System;
using System.Threading;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Effects/Destroy Object")]
public class DestroyEffect : GameEffect
{
    public override void Execute(GameObject source, Action callback = null, CancellationTokenSource tokenSource = null)
    {
        Destroy(source.gameObject);
        callback?.Invoke();
    }
}
