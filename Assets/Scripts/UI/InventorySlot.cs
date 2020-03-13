using System;
using System.Text;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    // Inspector
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Image _hilight;
    
    // Public fields
    [HideInInspector] public int slotIndex = -1;
    
    // Privates
    private bool _selected;

    // Properties
    public Item Item => GameState.Instance.Inventory[slotIndex].Item;
    public int Count => GameState.Instance.Inventory[slotIndex].Count;

    public bool Selected
    {
        get => _selected;
        set
        {
            _selected = value;
            _hilight.gameObject.SetActive(value);
        }
    }

    private void OnEnable()
    {
        GameState.Instance.Inventory.OnChange += UpdateSlot;
    }
    
    private void OnDisable()
    {
        GameState.Instance.Inventory.OnChange -= UpdateSlot;
    }

    public void UpdateSlot()
    {
        if (ReferenceEquals(Item, null))
        {
            _icon.gameObject.SetActive(false);
            _text.gameObject.SetActive(false);
        }
        else
        {
            _icon.sprite = Item.Icon;
            _icon.gameObject.SetActive(true);
            _text.text = Count.ToString();
            _text.gameObject.SetActive(true);
        }
    }
}