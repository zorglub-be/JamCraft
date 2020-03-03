using System;
using System.Collections;
using UnityEngine;

public class Inventory : ScriptableObject, IEnumerable
{
    // Inspector
    [SerializeField] private ItemStack[] _items = new ItemStack[20];

    // Privates
    private int _count;

    // Properties
    public int Capacity => _items.Length - Count;
    /// <summary>
    /// The total size of the inventory
    /// </summary>
    public int Length => _items.Length;
    /// <summary>
    /// The number of slots occupied in the inventory, they are not guaranteed to be contiguous.
    /// Do not use in a for-loop. use Length instead
    /// </summary>
    public int Count => _count;

    // Events
    
    /// <summary>
    /// This event is invoked whenever something changes in the inventory
    /// </summary>
    public Action OnChange;
    
    private void OnEnable()
    {
        _count = 0;
        for (int i = 0; i < _items.Length; i++)
        {
            if (_items[i] != null)
                _count++;
        }
    }

    /// <summary>
    ///tries to add an item to the inventory. Returns true if it was successfully added, else false
    /// </summary>
    public bool TryAdd(Item item)
    {
        return TryAdd(item, 1) > 0;
    }

    /// <summary>
    ///tries to add a stack of items to the inventory. Returns the number effectively added
    /// </summary>
    public int TryAdd(ItemStack stack)
    {
        if (stack == null || stack.Item == null || stack.Count == 0)
            return 0;
        return ModifyItemCount(stack.Item, stack.Count);

    }

    /// <summary>
    ///tries to add a number of items to the inventory. Returns the number effectively added
    /// </summary>
    public int TryAdd(Item item, int nbItems)
    {
        return ModifyItemCount(item, nbItems);
    }

    /// <summary>
    ///tries to remove a stack of items from the inventory. Returns true if successful, else false
    /// </summary>
    public bool TryRemove(Item item)
    {
        return ModifyItemCount(item, -1) > 0;
    }

    /// <summary>
    ///tries to remove a stack of items from the inventory. Returns the number effectively removed
    /// </summary>
    public int TryRemove(ItemStack stack)
    {
        if (stack == null || stack.Item == null || stack.Count == 0)
            return 0;
        return ModifyItemCount(stack.Item, -stack.Count);
    }

    /// <summary>
    ///tries to remove a number of items from the inventory. Returns the number effectively added
    /// </summary>
    public int TryRemove(Item item, int nbItems)
    {
        if (item == null || nbItems == 0)
            return 0;
        return ModifyItemCount(item, -nbItems);
    }


    /// <summary>
    /// Counts the total number of a given item present in the inventory
    /// </summary>
    public int CountItems(Item item)
    {
        var nbItems = 0;
        for (int i = 0; i < _items.Length; i++)
        {
            if (ReferenceEquals(item, _items[i]?.Item))
            {
                nbItems += _items[i].Count;
            }
        }
        return nbItems;
    }

    /// <summary>
    /// Clears the inventory
    /// </summary>
    public void Clear()
    {
        Array.Clear(_items, 0, _items.Length);
        _count = 0;
        OnChange?.Invoke();
    }
    
    /// <summary>
    /// removes all remaining items at the given index
    /// </summary>
    public void ClearAt(int index)
    {
        Array.Clear(_items, index, 1);
        _count--;
        OnChange?.Invoke();
    }

    public IEnumerator GetEnumerator()
    {
        return _items.GetEnumerator();
    }

    /// <summary>
    /// returns the first index in the inventory where there is no item
    /// </summary>
    public int FirstAvailableIndex()
    {
        if (Capacity == 0)
            return -1;
        for (int i = 0; i < Length; i++)
        {
            if (_items[i] == null)
            {
                return i;
            }
        }
        return -1;
    }

    /// <summary>
    /// modifies the number of a specific item in the inventory, returns the number of items impacted
    /// </summary>
    private int ModifyItemCount(Item item, int delta)
    {
        if (item == null || delta == 0)
            return 0;
        var changedItems = 0;
        // first we see if there are any already existing stacks with the same item
        for (int i = 0; i < _items.Length; i++)
        {
            var currentStack = _items[i];
            if (currentStack == null)
            {
                //we remember this for later   
                continue;
            }
            if (ReferenceEquals(currentStack.Item, item))
            {
                while (changedItems <= Mathf.Abs(delta) && currentStack.Count < item.MaxStack)
                {
                    if (delta > 0)
                        currentStack.Increment();
                    else
                        currentStack.Decrement();
                    
                    changedItems++;
                }
                if (changedItems == Mathf.Abs(delta))
                {
                    OnChange?.Invoke();
                    return changedItems;
                }
            }
        }

        // if we're adding stuff, we look for empty spaces
        if (delta > 0)
        {
            for (int i = 0; i < _items.Length; i++)
            {
                if (_items[i] == null)
                {
                    var itemsToAdd = Mathf.Min(delta - changedItems, item.MaxStack);
                    _items[i] = new ItemStack(item, itemsToAdd);
                    changedItems += itemsToAdd;
                    _count++;
                    if (changedItems == delta)
                    {
                        OnChange?.Invoke();
                        return changedItems;
                    }
                }
            }
        }
        if (changedItems > 0)
            OnChange?.Invoke();
        return changedItems;              
    }
}