using System;
using UnityEngine;
using UnityEngine.Events;

public interface IItem
{
    string Name { get; }
    Sprite Icon { get; }
    Sprite Sprite { get; }
    bool CanUse { get; }
    void Use(GameObject user);
    bool IsConsumable { get; }
    
    Action OnUse { get; }
}