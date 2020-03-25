using System;
using UnityEngine;

[RequireComponent(typeof(MovementController), typeof(AbilitiesManager))]
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
            else if (NeoInput.GetKey(NeoInput.NeoKeyCode.SecondaryAttack))
                _abilitiesManager.UseSecondary();
            else if (NeoInput.GetKey(NeoInput.NeoKeyCode.SpecialAttack))
                _abilitiesManager.UseSpecial();            
        }
        if (NeoInput.GetKeyDown(NeoInput.NeoKeyCode.NextPrimary))
            _abilitiesManager.NextPrimary();
        if (NeoInput.GetKeyDown(NeoInput.NeoKeyCode.NextSecondary))
            _abilitiesManager.NextSecondary();
        if (NeoInput.GetKeyDown(NeoInput.NeoKeyCode.PreviousPrimary))
            _abilitiesManager.PreviousPrimary();
        if (NeoInput.GetKeyDown(NeoInput.NeoKeyCode.PreviousSecondary))
            _abilitiesManager.PreviousSecondary();
    }

    private void OnDisable()
    {
        _movementController.Horizontal = 0;
        _movementController.Vertical = 0;
    }
}