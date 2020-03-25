using UnityEngine.Events;

public interface IKillable
{
    UnityEvent OnKilled { get; }
    void Kill();
}