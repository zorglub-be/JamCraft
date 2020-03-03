using UnityEngine;

public class ItemScript : MonoBehaviour
{
    public Item _item;

    private void OnTriggerEnter(Collider other)
    {
        Pickup();
    }

    public void Pickup()
    {
        if (GameState.Instance.Inventory.TryAdd(_item))
            Destroy(gameObject);
    }
}