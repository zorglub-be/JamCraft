using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Effects/Play Animation")]
public class AnimationEffect : GameEffect
{
    [SerializeField] private string _animatorState;
    [SerializeField] private int _layerIndex;

    public override async void Execute(GameObject source, Action callback)
    {
        var token = GameState.Instance.CancellationToken;
        var animator = source.GetComponentsInChildren<Animator>(false)[0];
        if (ReferenceEquals(animator, null))
            return;
        
        animator.Play(_animatorState, _layerIndex);
        await Task.Yield();
        var stateInfo = animator.GetCurrentAnimatorStateInfo(_layerIndex);
        var duration = stateInfo.length;
        var wait = WaitForSeconds(duration);
        await wait;
        if (token.IsCancellationRequested)
            return;
        callback.Invoke();
    }
}