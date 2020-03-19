using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _initialVelocity;

    private void Start()
    {
        GetComponent<Rigidbody2D>().velocity = _initialVelocity * transform.right;
    }
}
