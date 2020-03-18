using UnityEngine;

[CreateAssetMenu(menuName = "Game Effect/Animation")]
public class AnimationEffect : GameEffect
{
    [SerializeField] private int _animationLayer;
    [SerializeField] private string _animationName;

    public override void Execute(GameObject source)
    {
        var animator = source.GetComponentsInChildren<Animator>(false)[0];
        if (ReferenceEquals(animator, null))
            return;
        animator.Play(_animationName, _animationLayer);
    }
}