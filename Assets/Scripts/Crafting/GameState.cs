using UnityEngine;

public class GameState: MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;

    public AudioSource AudioSource
    {
        get => _audioSource;
        set => _audioSource = value;
    }
}