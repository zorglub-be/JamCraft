using System;
using UnityEngine;
using Object = UnityEngine.Object;

[CreateAssetMenu(menuName = "Item")]
public abstract class Item : ScriptableObject, IItem
{
    [SerializeField] private string _name = default;
    [SerializeField] private Sprite _icon = default;
    [SerializeField] private Sprite _sprite = default;
    [SerializeField] private int _maxStack = default;
    [SerializeField] private int _useDelay = default;
    [SerializeField] private bool _isConsumable = default;

    //Properties
    public string Name => _name;
    public Sprite Icon => _icon;
    public Sprite Sprite => _sprite;
    public bool CanUse => RemainingCooldown == 0f;
    public float RemainingCooldown => Mathf.Max(0f, (Time.time - _lastUseTime) - _useDelay);
    public float Cooldown => _useDelay;
    public int MaxStack => _maxStack;
    public bool IsConsumable => _isConsumable;

    // Events
    public Action OnUse { get; }

    // Privates
    private float _lastUseTime;
    
    public void Use(GameObject user)
    {
        if (CanUse == false)
            return;
        UseEffect(user);
        _lastUseTime = Time.time;
        OnUse?.Invoke();
    }



    protected abstract void UseEffect(GameObject user);


    private void OnValidate()
    {
        if (_name.Length == 0)
            _name = base.name; //set the name by default to be the name of the scriptable object instance
    }
}