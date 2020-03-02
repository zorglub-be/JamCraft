using UnityEngine;

[CreateAssetMenu(menuName = "Item")]
public class Item : ScriptableObject, IItem
{
    [SerializeField] private string _name;
    [SerializeField] private Sprite _icon;
    [SerializeField] private Sprite _sprite;

    public string Name => _name;
    public Sprite Icon => _icon;
    public Sprite Sprite => _sprite;
}