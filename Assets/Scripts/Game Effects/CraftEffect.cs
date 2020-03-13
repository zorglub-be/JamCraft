using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Game Effects/Craft")]
public class CraftEffect : GameEffect
{
    [SerializeField] private Item _item;
    public Item Item => _item;

    public override void Execute(GameObject source)
    {
        GameState.Instance.Inventory.TryAdd(_item);
    }
}
