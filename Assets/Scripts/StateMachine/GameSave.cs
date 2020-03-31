using UnityEngine;

public class GameSave
{
    public int CurrentHealth { get; }
    public int MaxHealth { get; }
    public Item[] Abilities { get; }
    public int SelectedPrimary { get; }
    public int SelectedSecondary { get; }
    public ItemStack[] Inventory { get;}
    public LoadLevelEffect LevelLoader { get;}

    public GameSave(GameObject player, Inventory inventory, LoadLevelEffect currentLevelLoader)
    {
        CurrentHealth = player.GetComponent<Health>().CurrentHealth;
        CurrentHealth = player.GetComponent<Health>().MaximumHealth;
        var abilitiesManager = player.GetComponent<AbilitiesManager>();
        Abilities = new Item[abilitiesManager.Abilities.Length];
        abilitiesManager.Abilities.CopyTo(Abilities, 0);
        SelectedPrimary = abilitiesManager.SelectedPrimaryIndex;
        SelectedSecondary = abilitiesManager.SelectedSecondaryIndex;
        Inventory = new ItemStack[inventory.Length];
        for (int i = 0; i < inventory.Length; i++)
        {
            var stack = inventory[i];
            Inventory[i] = stack == null ? null : new ItemStack(stack.Item, stack.Count);
        }
        LevelLoader = currentLevelLoader.Clone();
    }
}