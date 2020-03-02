using UnityEngine;

[CreateAssetMenu(menuName = "Game Effects/Sound Effect")]
public class SoundEffect : GameEffect
{
    [SerializeField] private AudioClip _audioClip;

    public override void Execute()
    {
        LoadGameState();
        gameState.AudioSource.PlayOneShot(_audioClip);
    }
}