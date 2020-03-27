using UnityEngine;

public class ItemScript : MonoBehaviour
{
    public Item _item;
    public int amount;
    private SpriteRenderer _spriteRenderer;

    private void OnTriggerEnter(Collider other)
    {
        print("pickup");
        Pickup();
    }

    public void Pickup()
    {
        var before = amount;
        amount -= GameState.Instance.Inventory.TryAdd(_item, amount);
        /*
        if (amount < before)
            _item.PlaySound();
            */
        if (amount == 0)
            Destroy(gameObject);
    }

    private void OnValidate()
    {
        if (_spriteRenderer == null)
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (_spriteRenderer != null)
            _spriteRenderer.sprite = _item.Sprite;
    }
}