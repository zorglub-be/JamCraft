﻿using System;
using System.Threading;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Effects/Heal")]
public class HealEffect : GameEffect
{
    [SerializeField] private int _healAmount;

    public override void Execute(GameObject source, Action callback = null, CancellationTokenSource tokenSource = null)
    {
        var health = source.GetComponent<IHealable>();
        if (health != null)
            health.Heal(_healAmount);
        callback?.Invoke();
    }
}