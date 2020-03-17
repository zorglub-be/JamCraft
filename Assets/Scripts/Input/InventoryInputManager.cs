using UnityEngine;
using UnityEngine.Serialization;

public class InventoryInputManager : MonoBehaviour
{
    [SerializeField] private InventoryUI _inventoryUi;
    [SerializeField] private Animator _inventoryAnimator;

    private bool _isOpen;

    private void Update()
    {
        if (_isOpen)
        {
            if (NeoInput.GetKeyDown(NeoInput.NeoKeyCode.Left))
            {
                _inventoryUi.MoveLeft();
            }
            if (NeoInput.GetKeyDown(NeoInput.NeoKeyCode.Right))
            {
                _inventoryUi.MoveRight();
            }
            if (NeoInput.GetKeyDown(NeoInput.NeoKeyCode.Up))
            {
                _inventoryUi.MoveUp();
            }
            if (NeoInput.GetKeyDown(NeoInput.NeoKeyCode.Down))
            {
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