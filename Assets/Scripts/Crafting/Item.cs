using System;
using UnityEngine;
using Object = UnityEngine.Object;

[CreateAssetMenu(menuName = "Item")]
public class Item : ScriptableObject, IItem
{
    [SerializeField] private string _name;
    [SerializeField] private Sprite _icon;
    [SerializeField] private Sprite _sprite;

    public string Name => _name;
    public Sprite Icon => _icon;
    public Sprite Sprite => _sprite;

    private void OnValidate()
    {
        if (_name.Length == 0)
            _name = name; //set the name by default to be the name of the scriptable object instance
    }
}