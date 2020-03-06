using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : IMover
{
    private readonly Player _player;
    private readonly Rigidbody2D _rigidbody2D;
    
    private Vector3 currentVelocity = Vector3.zero;
    
    public Mover(Player player)
    {
        _player = player;
        _rigidbody2D = _player.GetComponent<Rigidbody2D>();
    }

    public void Tick()
    {
        var movementDirection = new Vector2(_player.PlayerInput.Horizontal, _player.PlayerInput.Vertical);
        // Set target velocity to smooth towards
        Vector2 targetVelocity = new Vector2(movementDirection.x * _player.moveSpeed * 10f, movementDirection.y * _player.moveSpeed * 10) * Time.fixedDeltaTime;

        // Smoothing out the movement
        _rigidbody2D.velocity = Vector3.SmoothDamp(_rigidbody2D.velocity, targetVelocity, ref currentVelocity, _player.m_MovementSmoothing);
    }
}
