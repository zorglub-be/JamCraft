using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Crafting/Inventory")]
public class Inventory : ScriptableObject, IEnumerable<ItemStack>
{
    // Inspector
    [SerializeField] private bool _clearOnAwake = true;
    [SerializeField] private ItemStack[] _items;

    private static int _defaultSize = 12;
    
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
    
    // Indexer
    
    /// <summary>
    /// Indexer to access the Item at a specific position in the inventory. This is a read-only indexer.
    /// </summary>
    /// <param name="index"></param>
    /// 
    public ItemStack this[int index] => _items[index];
    private void Awake()
    {
        if (_items == null)
            _items = new ItemStack[_defaultSize];
        if(_clearOnAwake)
            Clear();
    }

    private void OnEnable()
    {
        for (int i = 0; i < _items.Length; i++)
        {
            if (_items[i]?.Item == null)
                continue;
            var stackIndex = i;
            _items[i].OnCountChange += () => { CheckEmptyStack(stackIndex); };
            _items[i].OnCountChange += () => OnChange?.Invoke();
        }
    }

    public Inventory Duplicate()
    {
        return Instantiate(this);
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
        foreach (var stack in _items)
        {
            if(stack != null)
                stack.Clear();
        }
        Array.Clear(_items, 0, _items.Length);
        _count = 0;
    }
    
    /// <summary>
    /// removes all remaining items at the given index
    /// </summary>
    public void ClearAt(int index)
    {
        _items[index].Clear();
        Array.Clear(_items, index, 1);
        _count--;
    }
    
    /// <summary>
    /// removes a number of items at index, returns the number of items actually removed.
    /// </summary>
    public int TryRemoveAt(int index, int nbToRemove)
    {
        return ModifyItemCountAt(index, -nbToRemove);
    }
    
    /// <summary>
    /// adds a number of items at index, returns the number of items actually added.
    /// </summary>
    public int TryAddAt(int index, int nbToAdd)
    {
        return ModifyItemCountAt(index, nbToAdd);
    }

    /// <summary>
    /// splits a stack in half (rounded) and return the rounded up stack.
    /// </summary>
    public ItemStack SplitAt(int index)
    {
        if (_items[index] == null)
            return null;
        var count = _items[index].Count;
        var item = _items[index].Item;
        if (count == 0 || item == null)
            return null;
        var nbToRemove = count - count / 2;
        TryRemoveAt(index, nbToRemove);
        var newStack =  new ItemStack(item, nbToRemove);
        return newStack;
    }

    private void CheckEmptyStack(int index)
    {
        if (_items[index].Count == 0)
            ClearAt(index);
    }


    public IEnumerator<ItemStack> GetEnumerator()
    {
        return (IEnumerator<ItemStack>)_items.GetEnumerator();
        
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
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
            if (_items[i] == null || _items[i].Count == 0 || _items[i].Item == null)
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
            if (_items[i] == null || _items[i].Item == null)
            {
                continue;
            }
            if (ReferenceEquals(_items[i].Item, item))
            {
                changedItems += ModifyItemCountAt(i, delta);
                if (changedItems == Mathf.Abs(delta))
                {
//                    OnChange?.Invoke();
                    return changedItems;
                }
            }
        }
        
        // if all items couldn't find a spot in an existing stacks we look for empty spaces
        if (delta > 0)
        {
            for (int i = 0; i < _items.Length; i++)
            {
                if (_items[i] == null || _items[i].Item == null || _items[i].Count == 0)
                {
                    var itemsToAdd = Mathf.Min(delta - changedItems, item.MaxStack);
                    _items[i] = new ItemStack(item, itemsToAdd);
                    var stackIndex = i; //this is necessary to make sure the CheckEmptyStack call uses the right index
                    _items[i].OnCountChange += () => { CheckEmptyStack(stackIndex); };
                    _items[i].OnCountChange += () => OnChange?.Invoke();
                    OnChange?.Invoke();
                    changedItems += itemsToAdd;
                    _count++;
                    if (changedItems == delta)
                    {
                        return changedItems;
                    }
                }
            }
        }
        return changedItems;              
    }

    /// <summary>
    /// modifies the number of a specific item in the inventory, returns the number of items impacted
    /// </summary>
    private int ModifyItemCountAt(int index, int delta)
    {
        if (delta == 0)
            return 0;
        var stack = _items[index];
        if (ReferenceEquals(stack?.Item, null))
        {
            return 0;
        }
        var initialCount = stack.Count;
        if (delta > 0)
            stack.Increment(delta);
        else
            stack.Decrement(Mathf.Abs(delta));
        var modifiedNb = Mathf.Abs(initialCount - stack.Count);
        if (modifiedNb > 0)
            OnChange?.Invoke();
        return modifiedNb;
    }

    private void OnValidate()
    {
        if (_items == null)
            _items = new ItemStack[_defaultSize];
    }

    public int FirstIndexOf(Item item)
    {
        for (int i = 0; i < _items.Length; i++)
        {
            if (_items[i].Item == item && _items[i].Count > 0)
                return i;
        }
        return -1;
    }

    public void FillFrom(IEnumerable<ItemStack> stacks)
    {
        OnChange = null;
        Clear();
        foreach (ItemStack stack in stacks)
        {
            TryAdd(stack);
        }
    }
}