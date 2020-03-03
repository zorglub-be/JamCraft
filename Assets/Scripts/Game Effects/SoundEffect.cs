using UnityEngine;

[CreateAssetMenu(menuName = "Game Effects/Sound Effect")]
public class SoundEffect : GameEffect
{
    [SerializeField] private AudioClip _audioClip;

    public override void Execute()
    {
        GameState.Instance.AudioSource.PlayOneShot(_audioClip);
    }
}