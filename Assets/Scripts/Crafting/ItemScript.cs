using UnityEngine;

public class ItemScript : MonoBehaviour
{
    public Item _item;
    public int amount;

    private void OnTriggerEnter(Collider other)
    {
        print("pickup");
        Pickup();
    }

    public void Pickup()
    {
        amount -= GameState.Instance.Inventory.TryAdd(_item, amount);
        if (amount == 0)
            Destroy(gameObject);
    }
}