using UnityEngine;

public class GameState: SingletonMB<GameState>
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private GameObject _player;

    public AudioSource AudioSource
    {
        get => _audioSource;
        set => _audioSource = value;
    }
    public GameObject Player => _player;
}