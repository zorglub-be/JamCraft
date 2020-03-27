using System;
using UnityEngine;

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
    [SerializeField] private AudioClip _sound;

    
    //Properties
    public string Name => _name;
    public Sprite Icon => _icon;
    public Sprite Sprite => _sprite;
    public AudioClip Sound => _sound;
    public bool IsReady => RemainingCooldown == 0f;
    public float RemainingCooldown => Mathf.Max(0f,  _useDelay - (Time.time - _lastUseTime));
    public float Cooldown => _useDelay;
    public int MaxStack => _maxStack;
    public bool IsConsumable => _isConsumable;
    public bool IsUsable => _isUsable;

    // Events
    public Action OnUse { get; }

    // Privates
    private float _lastUseTime;

    public void OnEnable()
    {
        _lastUseTime = Time.time -_useDelay;
    }

    public bool TryUse(GameObject user)
    {
        if (IsUsable == false || IsReady == false)
            return false;
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

    private void OnValidate()
    {
        if (_name.Length == 0)
            _name = base.name; //set the name by default to be the name of the scriptable object instance
    }
}