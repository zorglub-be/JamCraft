using UnityEngine;

public class GameState: SingletonMB<GameState>
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private GameObject _player;
    [SerializeField] private Inventory _inventory;

    public AudioSource AudioSource
    {
        get => _audioSource;
        set => _audioSource = value;
    }
    public GameObject Player => _player;
    public Inventory Inventory => _inventory;
}