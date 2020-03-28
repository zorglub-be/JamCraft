using System;
using System.Threading;
using UnityEngine;


[CreateAssetMenu(menuName = "Game Effects/Inventory")]
public class InventoryEffect : GameEffect
{
    [SerializeField] private Item _item;
    [SerializeField] private InventoryEffectType _effectType = InventoryEffectType.Gain;
    [SerializeField] private int _amount = 1;
    public Item Item => _item;

    public override void Execute(GameObject source, Action callback=null, CancellationTokenSource tokenSource = null)
    {
        if (_effectType == InventoryEffectType.Consume)
        {
            GameState.Instance.Inventory.TryRemove(_item, _amount);
        }
        else
        {
            if (_item.Sound != null)
                GameState.Instance.AudioSource.PlayOneShot(_item.Sound);
            GameState.Instance.Inventory.TryAdd(_item);
        }
        callback?.Invoke();
    }
    public enum InventoryEffectType {Gain, Consume,}
}
