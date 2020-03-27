using System;
using System.Threading;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Effects/New Game")]
public class NewGameEffect : GameEffect
{
    [SerializeField] private LoadLevelEffect _firstLevelLoader;

    public override void Execute(GameObject source, Action callback = null, CancellationTokenSource tokenSource = null)
    {
        GameState.Instance.NewGame(_firstLevelLoader);
        callback?.Invoke();
    }
}