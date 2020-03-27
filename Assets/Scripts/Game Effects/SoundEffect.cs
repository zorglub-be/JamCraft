using System;
using System.Threading;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Effects/Sound Effect")]
public class SoundEffect : GameEffect
{
    [SerializeField] private AudioClip _audioClip;

    public override async void Execute(GameObject user, Action callback=null, CancellationTokenSource tokenSource = null)
    {
        GameState.Instance.AudioSource.PlayOneShot(_audioClip);
        var task = WaitForSeconds(_audioClip.length, tokenSource);
        await task;
        callback?.Invoke();
    }
}