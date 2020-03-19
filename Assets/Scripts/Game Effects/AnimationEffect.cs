using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Effects/Play Animation")]
public class AnimationEffect : GameEffect
{
    [SerializeField] private string _animatorState;
    [SerializeField] private int _layerIndex;

    public async override void Execute(GameObject source, Action callback)
    {
        var animator = source.GetComponentsInChildren<Animator>(false)[0];
        if (ReferenceEquals(animator, null))
            return;
        
        animator.Play(_animatorState, _layerIndex);
        await Task.Delay(1);
        var stateInfo = animator.GetCurrentAnimatorStateInfo(_layerIndex);
        var duration = stateInfo.length;
        WaitForSeconds(duration, callback);
    }
    
    private async Task WaitForSeconds(float duration, Action callback)
    {
        await Task.Delay((int)(duration * 1000));
        callback?.Invoke();
    }
}