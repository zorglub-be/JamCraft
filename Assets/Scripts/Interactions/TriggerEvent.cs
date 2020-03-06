using UnityEngine;

public class TriggerEvent : MonoBehaviour
{
    public GameObjectEvent OnTriggerEntered;
    public GameObjectEvent OnTriggerStay;
    public GameObjectEvent OnTriggerExit;

    private void OnTriggerEnter2D(Collider2D other)
    {
        OnTriggerEntered?.Invoke(other.attachedRigidbody.gameObject);
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        OnTriggerStay?.Invoke(other.attachedRigidbody.gameObject);
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        OnTriggerExit?.Invoke(other.attachedRigidbody.gameObject);
    }
}