using System;
using System.ComponentModel;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Serialization;

public class GameState: SingletonMB<GameState>
{
    // Inspector
    [SerializeField] private Inventory _inventory;
    [SerializeField] private AudioSource _audioSource;

    // Private fields
    private GameObject _player;
    private GameObject _hudUI;
    private GameObject _inventoryUI;
    private StateMachine _stateMachine;

    // Events
    public static event Action<IState> OnGameStateChanged;
    
    // Properties
    public Inventory Inventory => FindInventory();
    public AudioSource AudioSource => FindAudioSource();
    public GameObject Player => FindPlayer();
    public GameObject InventoryUI => FindInventoryUI();
    public GameObject HudUI => FindHudUI();
    
    public Type CurrentStateType => _stateMachine.CurrentState.GetType();

    // Overrides
    protected override void Initialize()
    {
        _stateMachine = new StateMachine();
        _stateMachine.OnStateChanged += state => OnGameStateChanged?.Invoke(state);
        
        var menu = new Menu();
        var loading = new LoadLevel();
        var play = new Play();
        var pause = new Pause();
        
        _stateMachine.SetState(menu);
        
        _stateMachine.AddStateChange(menu, loading, () => PlayLevel.LevelToLoad != null);
        
        
        _stateMachine.AddStateChange(loading, play, loading.Finished);
        _stateMachine.AddStateChange(play, pause, ()=> PlayerInput.Instance.PausePressed);
        _stateMachine.AddStateChange(pause, play, ()=> PlayerInput.Instance.PausePressed);
        //_stateMachine.AddStateChange(pause, menu, ()=>RestartButton.Pressed);
    }
    
    public void Update()
    {
        _stateMachine.Tick();
    }
    
    private GameObject FindPlayer()
    {
        if(ReferenceEquals(_player, null))
            _player = GameObject.FindWithTag("Player");
        return _player;
    }
    private GameObject FindHudUI()
    {
        if(ReferenceEquals(_hudUI, null))
            _hudUI = GameObject.FindWithTag("Hud");
        return _hudUI;
    }
    private GameObject FindInventoryUI()
    {
        if(ReferenceEquals(_inventoryUI, null))
            _inventoryUI = GameObject.FindWithTag("Inventory");
        return _inventoryUI;
    }
    private AudioSource FindAudioSource()
    {
        if(ReferenceEquals(_audioSource, null))
            _audioSource = FindObjectOfType<AudioSource>();
        return _audioSource;
    }

    private Inventory FindInventory()
    {
        if (ReferenceEquals(_inventory, null))
            _inventory = ScriptableObject.CreateInstance<Inventory>();
        return _inventory;
    }

    

}