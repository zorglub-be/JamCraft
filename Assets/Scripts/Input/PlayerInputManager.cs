using UnityEngine;

[RequireComponent(typeof(MovementController))]
public class PlayerInputManager : MonoBehaviour
{
    private MovementController _movementController;
    private AbilitiesManager _abilitiesManager;

    private void Awake()
    {
        _movementController = GetComponent<MovementController>();
        _abilitiesManager = GetComponent<AbilitiesManager>();
    }

    void Update()
    {
        if (Time.timeScale > 0)
        {
            _movementController.Horizontal = NeoInput.GetAxis(NeoInput.AxisCode.Horizontal);
            _movementController.Vertical = NeoInput.GetAxis(NeoInput.AxisCode.Vertical);
            if (NeoInput.GetKey(NeoInput.NeoKeyCode.PrimaryAttack))
                _abilitiesManager.UsePrimary();
            if (NeoInput.GetKey(NeoInput.NeoKeyCode.SecondaryAttack))
                _abilitiesManager.UseSecondary();
            if (NeoInput.GetKey(NeoInput.NeoKeyCode.SpecialAttack))
                _abilitiesManager.UseSpecial();            
        }

    }
}