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
        _movementController.Horizontal = NeoInput.GetAxis(NeoInput.AxisCode.Horizontal);
        _movementController.Vertical = NeoInput.GetAxis(NeoInput.AxisCode.Vertical);
    }
}