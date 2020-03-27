using UnityEngine;

public class Mover : IMover
{
    private readonly MovementController _movementController;
    private readonly Rigidbody2D _rigidbody2D;
    
    private Vector3 currentVelocity = Vector3.zero;
    
    public Mover(MovementController movementController)
    {
        _movementController = movementController;
        _rigidbody2D = _movementController.GetComponent<Rigidbody2D>();
    }

    public void Tick()
    {
        // changed by Zorglub: we take the values directly from the Player script
//        var movementDirection = new Vector2(PlayerInput.Instance.Horizontal, PlayerInput.Instance.Vertical);
        var movementDirection = new Vector2(_movementController.Horizontal, _movementController.Vertical);
        // Set target velocity to smooth towards
        Vector2 targetVelocity = new Vector2(movementDirection.x * _movementController.moveSpeed * 10f, movementDirection.y * _movementController.moveSpeed * 10) * Time.fixedDeltaTime;

        // Smoothing out the movement
        _rigidbody2D.velocity = Vector3.SmoothDamp(_rigidbody2D.velocity, targetVelocity, ref currentVelocity, _movementController.m_MovementSmoothing);
    }
}
