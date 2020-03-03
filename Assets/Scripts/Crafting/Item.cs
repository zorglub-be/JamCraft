using System;
using UnityEngine;
using Object = UnityEngine.Object;

[CreateAssetMenu(menuName = "Item")]
public class Item : ScriptableObject, IItem
{
    [SerializeField] private string _name = default;
    [SerializeField] private Sprite _icon = default;
    [SerializeField] private Sprite _sprite = default;
    [SerializeField] private int _maxStack = default;

    public string Name => _name;
    public Sprite Icon => _icon;
    public Sprite Sprite => _sprite;
    public int MaxStack => _maxStack;

    private void OnValidate()
    {
        if (_name.Length == 0)
            _name = base.name; //set the name by default to be the name of the scriptable object instance
    }
}