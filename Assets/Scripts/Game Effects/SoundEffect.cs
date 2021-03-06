using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Effects/Sound Effect")]
public class SoundEffect : GameEffect
{
    [SerializeField] private AudioClip _audioClip;

    public override void Execute(GameObject user, Action callback=null)
    {
        GameState.Instance.AudioSource.PlayOneShot(_audioClip);
        var task = WaitForSeconds(_audioClip.length, callback);
        task.Start();
    }
    private async Task WaitForSeconds(float duration, Action callback)
    {
        var token = GameState.Instance.CancellationToken;
        await Task.Delay((int)duration * 1000, token);
        if (token.IsCancellationRequested)
            return;
        callback?.Invoke();
    }
}