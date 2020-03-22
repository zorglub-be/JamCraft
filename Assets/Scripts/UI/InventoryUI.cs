using System.Data;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

///Note from Zorglub:
/// I changed this class heavily because I think it's better to move the responsibility of item and count tracking
/// in InventorySlot. This class will be used for managing interactions with the InventoryUI instead.

[RequireComponent(typeof(GridLayoutGroup))]
public class InventoryUI : MonoBehaviour
{
    //Inspector
    [SerializeField] private Transform _inventoryPanel;
    [SerializeField] private int _columns = 4;
    [SerializeField] private UnityEvent OnCursorMove;
    [SerializeField] private CraftingUI _craftingUI;
    [SerializeField] private AudioClip _cursorSound;
    
    //Privates
    private InventorySlot[] _slots;
    private int _cursorIndex;
    private GridLayoutGroup _gridLayoutGroup;

    //Properties
    private Inventory Inventory => GameState.Instance.Inventory;
    private AudioSource AudioSource => GameState.Instance.AudioSource;
    private GameObject Player => GameState.Instance.Player;
    
    private void Awake()
    {
        _gridLayoutGroup = GetComponent<GridLayoutGroup>();
        _slots = _inventoryPanel.GetComponentsInChildren<InventorySlot>();
        for (int i = 0; i < _slots.Length; i++)
        {
            _slots[i].slotIndex = i;
            _slots[i].UpdateSlot();
        }
        SetCursor(0);
    }

    private void OnEnable()
    {
        if (_slots != null)
        {
            SetCursor(0);
        }
    }

    public void MoveUp()
    {
        MoveCursor(Direction.Up);
    }
    public void MoveDown()
    {
        MoveCursor(Direction.Down);
    }
    public void MoveLeft()
    {
        MoveCursor(Direction.Left);
    }
    public void MoveRight()
    {
        MoveCursor(Direction.Right);
    }

    public void UseSelectedItem()
    {
        if (_cursorIndex >= 0)
        {
            if (Inventory[_cursorIndex].TryUse(Player))
            {
                
            }
        }
    }

    public void SendSelectedItemToCraft()
    {
        if (_cursorIndex >= 0)
        {
            _craftingUI.AddItem(_cursorIndex);
        }
    }
    
    public void MoveCursor(Direction direction)
    {
        var index = _cursorIndex;
        switch (direction)
        {
            case Direction.Right:
                index++;
                break;
            case Direction.Left:
                index--;
                break;
            case Direction.Down:
                index += _columns;
                break;
            case Direction.Up:
                index -= _columns;
                break;
        }
        AudioSource.PlayOneShot(_cursorSound);
        SetCursor((index + _slots.Length)% _slots.Length);
    }

    private void SetCursor(int newIndex)
    {
        _slots[_cursorIndex].Selected = false;
        _cursorIndex = newIndex;
        _slots[_cursorIndex].Selected = true;
        OnCursorMove?.Invoke();
    }

    
    // It is necessary to enforce constraints in the gridlayoutgroup so the cursor moves as expected
    private void OnValidate()
    {
        if (_gridLayoutGroup == null)
            _gridLayoutGroup = GetComponent<GridLayoutGroup>();
        if (_gridLayoutGroup.constraint != GridLayoutGroup.Constraint.FixedColumnCount 
            || _gridLayoutGroup.constraintCount != _columns)
        {
            _gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            _gridLayoutGroup.constraintCount = _columns;
            Debug.LogFormat("<b><color='red'>Warning!</color></b> Grid constraints not in line with expected configuration. \r\n " +
                            "Automatically adjusted to {0} columnts", _columns);
        }
    }


    public void Craft()
    {
        _craftingUI.Combine();
    }

    public void Drop()
    {
        //Todo: create a drop prefab to instantiate and contain the item stack
        throw new System.NotImplementedException();
    }
}