using UnityEngine;

public class CollisionEvent : MonoBehaviour
{
    public GameObjectEvent OnCollisionEntered;
    public GameObjectEvent OnCollisionStay;
    public GameObjectEvent OnCollisionExit;

    private void OnCollisionEnter2D(Collision2D col)
    {
        OnCollisionEntered?.Invoke(col.gameObject);
    }
    private void OnCollisionStay2D(Collision2D col)
    {
        OnCollisionStay?.Invoke(col.gameObject);
    }
    private void OnCollisionExit2D(Collision2D col)
    {
        OnCollisionExit?.Invoke(col.gameObject);
    }
}