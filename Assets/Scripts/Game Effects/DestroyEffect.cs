using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Effects/Destroy Object")]
public class DestroyEffect : GameEffect
{
    public override void Execute(GameObject source, Action callback = null)
    {
        Destroy(source);
        callback?.Invoke();
    }
}
