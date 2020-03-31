using System;
using UnityEngine;
using UnityEngine.Events;

public class ItemPresenceEvent : MonoBehaviour
{
    public enum ItemLocation{Inventory, Abilities,}

    [SerializeField] private ItemTracking[] _itemsToTrack;
    
    
    public UnityEvent OnAllItemsPresent;
    public UnityEvent OnAnyItemLost;
    
    private AbilitiesManager _abilitiesManager;

    private void Start()
    {
        _abilitiesManager = GameState.Instance.Player.GetComponent<AbilitiesManager>();
        CheckItemTrackings();
        _abilitiesManager.OnChange.AddListener(CheckItemTrackings);
        GameState.Instance.Inventory.OnChange += CheckItemTrackings;

    }

    private void CheckItemTrackings()
    {
        var allPresent = true;
        var hasChanged = false;
        for (int i = 0; i < _itemsToTrack.Length; i++)
        {
            hasChanged = CheckItemPresenceChanged(_itemsToTrack[i]) || hasChanged;
            allPresent = allPresent && _itemsToTrack[i].isPresent;
        }
        if (hasChanged && allPresent)
            OnAllItemsPresent?.Invoke();
        if (hasChanged && !allPresent)
            OnAnyItemLost?.Invoke();
    }

    private bool CheckItemPresenceChanged(ItemTracking tracking)
    {
        var found = false;
        switch (tracking.itemLocation)
        {
            case ItemLocation.Abilities:
                found = _abilitiesManager.IsSelected(tracking.item);
                break;
            case ItemLocation.Inventory:
                found = GameState.Instance.Inventory.FirstIndexOf(tracking.item) >= 0;
                break;
        }

        if (found && !tracking.isPresent)
        {
            tracking.isPresent = true;
            return true;
        }
        if (!found && tracking.isPresent)
        {
            tracking.isPresent = false;
            return true;
        }
        return false;
    }
}

[Serializable]
internal class ItemTracking
{
    public ItemPresenceEvent.ItemLocation itemLocation;
    public Item item;
    [HideInInspector]public bool isPresent;
}
