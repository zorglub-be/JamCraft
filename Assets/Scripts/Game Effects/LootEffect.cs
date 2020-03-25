using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


[CreateAssetMenu(menuName = "Game Effects/Loot Item")]
public class LootEffect : GameEffect
{
    [SerializeField] private Item _item;
    public Item Item => _item;

    public override void Execute(GameObject source, Action callback=null)
    {
        if (_item.Sound != null)
            GameState.Instance.AudioSource.PlayOneShot(_item.Sound);
        GameState.Instance.Inventory.TryAdd(_item);
        callback?.Invoke();
    }
}
