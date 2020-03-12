using System;
using UnityEngine;
using UnityEngine.Events;

public interface IItem
{
    string Name { get; }
    Sprite Icon { get; }
    Sprite Sprite { get; }
    bool IsReady { get; }
    bool TryUse(GameObject user);
    bool IsConsumable { get; }
    bool IsUsable { get; }
    
    Action OnUse { get; }
}