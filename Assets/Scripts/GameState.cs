using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using AsyncOperation = UnityEngine.AsyncOperation;

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

public interface IState
{
    void Tick();
    void OnEnter();
    void OnExit();
}

public class Menu : IState
{
    public void Tick()
    {
         
    }
 
    public void OnEnter()
    {
        PlayLevel.LevelToLoad = null;
        SceneManager.LoadSceneAsync("MainMenu");
    }
 
    public void OnExit()
    {
         
    }
}
 
public class Play : IState
{
    public void Tick()
    {
    }

    public void OnEnter()
    {
    }

    public void OnExit()
    {
    }
}

public class Pause : IState
{
    public void Tick()
    {
    }

    public void OnEnter()
    {
        SceneManager.LoadScene("PauseMenu", LoadSceneMode.Additive);
        Time.timeScale = 0f;
        
    }

    public void OnExit()
    {
        SceneManager.UnloadSceneAsync("PauseMenu");
        Time.timeScale = 1f;
    }
}

public class LoadLevel : IState
{
    public bool Finished() => _operations.TrueForAll(t =>t.isDone);
    private List<AsyncOperation> _operations = new List<AsyncOperation>();
    public void Tick()
    {
    }

    public void OnEnter()
    {
        Debug.Log(PlayLevel.LevelToLoad);
        _operations.Add(SceneManager.LoadSceneAsync(PlayLevel.LevelToLoad));
        _operations.Add(SceneManager.LoadSceneAsync("HUD", LoadSceneMode.Additive));
    }

    public void OnExit()
    {
        _operations.Clear();
    }
}

public class StateMachine
{
    private List<ChangeState> _stateChangeStates = new List<ChangeState>();
    private List<ChangeState> _anyStateChange = new List<ChangeState>();

    private List<IState> _states = new List<IState>();
    private IState _currentState;
    public IState CurrentState => _currentState;
    public event Action<IState> OnStateChanged;

    public void AddStateChange(IState from, IState to, Func<bool> condition)
    {
        var changeState = new ChangeState(from, to, condition);
        _stateChangeStates.Add(changeState);
    }

    public void AddAnyStateChange(IState to, Func<bool> condition)
    {
        var changeState = new ChangeState(null, to, condition);
        _anyStateChange.Add(changeState);
    }

    public void SetState(IState state)
    {
        if (_currentState == state)
            return;

        _currentState?.OnExit();
        _currentState = state;
        Debug.Log($"Change to state {state}");
        _currentState.OnEnter();
        OnStateChanged?.Invoke(_currentState);
    }

    public void Tick()
    {
        ChangeState changeState = CheckForStateChange();
        if (changeState != null)
        {
            SetState(changeState.To);
        }

        _currentState.Tick();
    }

    private ChangeState CheckForStateChange()
    {
        foreach (var changeState in _anyStateChange)
        {
            if (changeState.Condition())
                return changeState;
        }

        foreach (var changeState in _stateChangeStates)
        {
            if (changeState.From == _currentState && changeState.Condition())
            {
                return changeState;
            }
        }

        return null;
    }
}

public class ChangeState
 {
     public readonly IState From;
     public readonly IState To;
     public readonly Func<bool> Condition;
 
     public ChangeState(IState from, IState to,Func<bool> condition)
     {
         From = @from;
         To = to;
         Condition = condition;
     }
 }