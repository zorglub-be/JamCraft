using System;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Item")]
public class Item : ScriptableObject, IItem
{
    [SerializeField] private string _name = default;
    [SerializeField] private Sprite _icon = default;
    [SerializeField] private Sprite _sprite = default;
    [SerializeField] private int _maxStack = default;
    [SerializeField] private float _useDelay = default;
    [SerializeField] private bool _isConsumable = default;
    [SerializeField] private bool _isUsable = default;
    [SerializeField] private GameEffect _useEffect = default;
    [FormerlySerializedAs("_requireItem")] [SerializeField] private Item _requiredItem = null;
    [SerializeField] private AudioClip _sound;
    
    //Properties
    public string Name => _name;
    public Sprite Icon => _icon;
    public Sprite Sprite => _sprite;
    public AudioClip Sound => _sound;
    public bool IsReady => RemainingCooldown == 0f;
    public float RemainingCooldown
    {
        get
        {
            if (!_isListeningToRequiredItem)
                CheckRequiredItem();
            if (_hasRequiredItem == false)
                return 1;
            return Mathf.Max(0f, _useDelay - (Time.time - _lastUseTime));
        }
    }

    public float Cooldown => _useDelay;
    public int MaxStack => _maxStack;
    public bool IsConsumable => _isConsumable;
    public bool IsUsable => _isUsable;

    // Events
    public Action OnUse { get; }

    // Privates
    private float _lastUseTime;
    private bool _hasRequiredItem;
    private bool _isListeningToRequiredItem;

    public void OnEnable()
    {
        _lastUseTime = Time.time -_useDelay;
        _isListeningToRequiredItem = false;
    }

    public bool TryUse(GameObject user)
    {
        _hasRequiredItem = _requiredItem == null || GameState.Instance.Inventory.FirstIndexOf(_requiredItem) >= 0;
        if (IsUsable == false || IsReady == false)
            return false;
        if (_requiredItem && _requiredItem._isConsumable)
            GameState.Instance.Inventory.TryRemove(_requiredItem);
        _hasRequiredItem = _requiredItem == null || GameState.Instance.Inventory.FirstIndexOf(_requiredItem) >= 0;
        _useEffect.Execute(user);
        PlaySound();
        _lastUseTime = Time.time;
        OnUse?.Invoke();
        return true;
    }

    public void PlaySound()
    {
        if (_sound != null)
            GameState.Instance.AudioSource.PlayOneShot(_sound);
    }

    private void CheckRequiredItem()
    {
        if (!_isListeningToRequiredItem)
        {
            GameState.Instance.Inventory.OnChange += CheckRequiredItem;
            _isListeningToRequiredItem = true;
        }
        _hasRequiredItem = _requiredItem == null || GameState.Instance.Inventory.FirstIndexOf(_requiredItem) >= 0;
    }
    private void OnValidate()
    {
        if (_name.Length == 0)
            _name = base.name; //set the name by default to be the name of the scriptable object instance
    }
}