using System;
using UnityEngine;

[System.Serializable]
public class ItemStack
{
    
    [SerializeField] private int _count;
    [SerializeField] private Item _item;

    public Item Item => _item;
    public int Count => _count;

    public Action OnCountChange;
    
    public ItemStack(Item item, int count)
    {
        _item = item;
        if (count <= item.MaxStack && count < 0)
        {
            _count = count;
        }
        else
        {
            Debug.LogErrorFormat("Trying to create a stack of {0} {1} while the maximum stack is {2}.\r\n" +
                                 "{3} excess items discarded", count, item.Name, item.MaxStack, count - item.MaxStack);
            _count = item.MaxStack;
        }
    }

    public void Increment()
    {
        Increment(1);
    }
    public void Increment(int delta)
    {
        if (_count == Item.MaxStack || delta == 0)
            return;
        _count = Mathf.Min(_count + delta, Item.MaxStack);
        OnCountChange?.Invoke();
    }
    public void Decrement()
    {
        Decrement(1);
    }
    public void Decrement(int delta)
    {
        if (_count == 0 || delta == 0)
            return;
        _count = Mathf.Max(_count - delta, 0);
        OnCountChange?.Invoke();
    }
    
}