using UnityEngine;

public interface IItem
{
    string Name { get; }
    Sprite Icon { get; }
    Sprite Sprite { get; }
}