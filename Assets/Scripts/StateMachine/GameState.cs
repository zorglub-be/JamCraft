    using System;
    using System.Collections.Generic;
    using System.Threading;
    using UnityEngine;

    public class GameState: SingletonMB<GameState>
    {
        // Inspector
        [SerializeField] private Player _playerPrefab;
        [SerializeField] private Inventory _inventory;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private LayerFriends[] _layerRelationships;

        // Private fields
        private GameObject _player;
        private GameObject _hudUI;
        private GameObject _inventoryUI;
        private StateMachine _stateMachine;
        private CancellationTokenSource _cancellationTokenSource;
        private Dictionary<int, int> _friendlyLayersIndex;
        private GameSave _gameSave;
        private Inventory savedInventory;


        // Events
        public static event Action<IState> OnGameStateChanged;
        
        // Properties
        public Inventory Inventory => FindInventory();
        public AudioSource AudioSource => FindAudioSource();
        public GameObject Player => FindPlayer();
        public GameObject InventoryUI => FindInventoryUI();
        public GameObject HudUI => FindHudUI();
        public LoadLevelEffect CurrentLevelLoader { get; set; }
        public CancellationToken CancellationToken
        {
            get
            {
                if (_cancellationTokenSource == null)
                    _cancellationTokenSource = new CancellationTokenSource();
                return _cancellationTokenSource.Token;
            }
        }

        public Type CurrentStateType => _stateMachine.CurrentState.GetType();
        public bool Loading { get; set;}
        public bool Paused { get; set; }

        // Overrides
        protected override void Initialize()
        {
            _friendlyLayersIndex = new Dictionary<int, int>(_layerRelationships.Length);
            _cancellationTokenSource = new CancellationTokenSource();
            foreach (var item in _layerRelationships)
            {
                _friendlyLayersIndex.Add(LayerMask.NameToLayer(item.layerName), item.friendlyLayers.value);
            }
            _stateMachine = new StateMachine();
            _stateMachine.OnStateChanged += CheckStateAndCancelAsyncIfNecessary;
            _stateMachine.OnStateChanged += state => OnGameStateChanged?.Invoke(state);
            
            var loading = new LoadLevel();
            var play = new Play();
            var pause = new Pause();
            var menu = new Menu(_musicSource);
            
            
            _stateMachine.SetState(menu);
            
            _stateMachine.AddAnyStateChange(loading, () => Loading == true);
            _stateMachine.AddStateChange(loading, play, () => Loading == false && _player != null);
            _stateMachine.AddStateChange(loading, menu, () => Loading == false && _player == null);
            _stateMachine.AddStateChange(play, pause, ()=> Time.timeScale > 0 &&  NeoInput.GetKeyDown(NeoInput.NeoKeyCode.Pause));
            _stateMachine.AddStateChange(pause, play, ()=> NeoInput.GetKeyDown(NeoInput.NeoKeyCode.Pause));
            _stateMachine.AddStateChange(play, pause, ()=> Paused);
            _stateMachine.AddStateChange(pause, play, ()=> Paused == false);
        }

        public void SaveGame()
        {
            _gameSave = new GameSave(Player, Inventory, CurrentLevelLoader);
        }

        public void LoadGame()
        {
            if (_gameSave == null)
                return;
            if (Player)
            {
                Destroy(Player);
            }
            Inventory.FillFrom(_gameSave.Inventory);
            _player = Instantiate(_playerPrefab).gameObject;
            _player.GetComponent<Health>().SetHealth(_gameSave.CurrentHealth);
            _player.GetComponent<Health>().SetMaxHealth(_gameSave.CurrentHealth);
            var abilitiesManager = _player.GetComponent<AbilitiesManager>();
            abilitiesManager.Abilities = _gameSave.Abilities;
            abilitiesManager.SelectedPrimaryIndex = _gameSave.SelectedPrimary;
            abilitiesManager.SelectedSecondaryIndex = _gameSave.SelectedSecondary;
            _gameSave.LevelLoader.Execute(null);
        }
        

        public void NewGame(LoadLevelEffect firstLevelLoader)    
        {
            if (Player)
            {
                Destroy(Player);
            }
            Inventory.Clear();
            _player = Instantiate(_playerPrefab).gameObject;
            firstLevelLoader.Execute(null);
        }
        
        
        protected override void Cleanup()
        {
            if (Player)
            {
                Destroy(Player);
            }
            Inventory.Clear();
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource = null;
        }

        private void CheckStateAndCancelAsyncIfNecessary(IState state)
        {
            if (state.GetType() != typeof(Pause))
                _cancellationTokenSource.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void Update()
        {
            //this is a bit ugly but I just want to gain time
            
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

        public bool AreFriendly(GameObject object1, GameObject object2)
        {
            var layer1 = object1.layer;
            var layer2 = object2.layer;
            if (_friendlyLayersIndex.ContainsKey(layer1) && _friendlyLayersIndex[layer1] == (_friendlyLayersIndex[layer1] | 1 << layer2))
                return true;
            return (_friendlyLayersIndex.ContainsKey(layer2) && _friendlyLayersIndex[layer2] == (_friendlyLayersIndex[layer2] | 1 << layer1));
        }
    }

    [Serializable]
    public struct LayerFriends    
    {    
        public string layerName;
        public LayerMask friendlyLayers;
    }