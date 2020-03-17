using System;
using UnityEngine;

[RequireComponent(typeof(MovementController))]
public class PlayerInputManager : MonoBehaviour
{
    private MovementController _movementController;

    private void Awake()
    {
        _movementController = GetComponent<MovementController>();
    }

    void Update()
    {
        _movementController.Horizontal = NeoInput.HorizontalAxis();
        _movementController.Vertical = NeoInput.VerticalAxis();
    }
}