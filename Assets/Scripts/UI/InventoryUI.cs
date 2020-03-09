using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Transform _inventoryPanel;
    private Inventory _inventory;
    private InventorySlot[] _slots;
      
    private void Awake()
    {
        _inventory = GameState.Instance.Inventory;
        _inventory.OnChange += UpdateUI;

        _slots = _inventoryPanel.GetComponentsInChildren<InventorySlot>();
    }

    private void UpdateUI()
    {
        List<ItemStack> items = new List<ItemStack>();
        var enumerator = _inventory.GetEnumerator();
        while (enumerator.MoveNext())
        {
            //Debug.Log(enumerator.Current);
            items.Add((ItemStack) enumerator.Current);
        }
        //Item item;
        //IEnumerator _items = _inventory.GetEnumerator();
        
        for (int i = 0; i < _slots.Length; i++)
        {
            if (i < _inventory.Length && items[i]?.Item)
            {
                _slots[i].AddItem(items[i].Item, items[i].Count); 
            }
            else
            {
                _slots[i].ClearSlot();
            }
            
        }
        Debug.Log("Update UI");
    }
}
