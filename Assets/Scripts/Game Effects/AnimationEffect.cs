using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Effects/Play Animation")]
public class AnimationEffect : GameEffect
{
    [SerializeField] private string _animatorState;
    [SerializeField] private float _animationDuration;
    [SerializeField] private int _layerIndex;

    public override async void Execute(GameObject source, Action callback, CancellationTokenSource tokenSource = null)
    {
        var token = tokenSource?.Token ?? GameState.Instance.CancellationToken;
        var animator = source.GetComponentsInChildren<Animator>(false)[0];
        if (ReferenceEquals(animator, null))
            return;
        
        animator.Play(_animatorState, _layerIndex);
        await Task.Yield();
        var wait = WaitForSeconds(_animationDuration, tokenSource);
        await wait;
        if (token.IsCancellationRequested)
            return;
        callback.Invoke();
    }
}