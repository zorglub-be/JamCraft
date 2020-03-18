using UnityEngine;

public class InventoryInputManager : MonoBehaviour
{
    [SerializeField] private InventoryUI _inventoryUi;
    [SerializeField] private Animator _inventoryAnimator;

    private bool _isOpen;
    private float _horizontalDelay = 0.2f;
    private float _verticalDelay = 0.2f;
    private float _horizontalAxis;
    private float _verticalAxis;
    private float _lastHorizontalUse;
    private float _lastVerticalUse;

    private void Update()
    {
        
        if (_isOpen)
        {
            if(NeoInput.UpdateTimedAxis(NeoInput.AxisCode.Horizontal, ref _horizontalAxis,ref _lastHorizontalUse, _horizontalDelay, true))
            {
                if (_horizontalAxis > 0)
                    _inventoryUi.MoveRight();
                if (_horizontalAxis < 0)
                    _inventoryUi.MoveLeft();
            }

            if (NeoInput.UpdateTimedAxis(NeoInput.AxisCode.Vertical, ref _verticalAxis, ref _lastVerticalUse,
                _verticalDelay, true))
            {
                if (_verticalAxis > 0)
                    _inventoryUi.MoveUp();
                if (_verticalAxis < 0)
                    _inventoryUi.MoveDown();
            }

            if (NeoInput.GetKeyDown(NeoInput.NeoKeyCode.Select))
            {
                _inventoryUi.SendSelectedItemToCraft();
            }
            if (NeoInput.GetKeyDown(NeoInput.NeoKeyCode.Use))
            {
                _inventoryUi.UseSelectedItem();
            }
            if (NeoInput.GetKeyDown(NeoInput.NeoKeyCode.Craft))
            {
                _inventoryUi.Craft();
            }
            if (NeoInput.GetKeyDown(NeoInput.NeoKeyCode.Drop))
            {
                _inventoryUi.Drop();
            }

        }
        if (NeoInput.GetKeyDown(NeoInput.NeoKeyCode.ToggleInventory))
        {
            _isOpen = !_isOpen;
            _inventoryAnimator.SetBool("IsOpen", _isOpen);
        }
    }
}