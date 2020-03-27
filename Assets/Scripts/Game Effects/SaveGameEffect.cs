using System;
using System.Threading;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Effects/Save Game")]
public class SaveGameEffect : GameEffect
{
    public override void Execute(GameObject source, Action callback = null, CancellationTokenSource tokenSource = null)
    {
        GameState.Instance.SaveGame();
        callback?.Invoke();
    }
}