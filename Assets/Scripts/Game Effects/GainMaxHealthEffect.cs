using System;
using System.Threading;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Effects/Gain Max Health")]
public class GainMaxHealthEffect : GameEffect
{
    [SerializeField] private int _amount;

    public override void Execute(GameObject source, Action callback = null, CancellationTokenSource tokenSource = null)
    {
        source.GetComponent<Health>().IncreaseMaxHealth(_amount);
        callback?.Invoke();
    }
}
