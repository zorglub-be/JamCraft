using UnityEngine;
using UnityEngine.Events;

public class Interaction : MonoBehaviour
{
    [SerializeField] private Item _requiredItem;
    [SerializeField] private bool _consumesItem;
    public UnityEvent OnInteract;

    private void Update()
    {
        if (Time.timeScale > 0f && NeoInput.GetKeyDown(NeoInput.NeoKeyCode.Interact))
        {
            if (_requiredItem)
            {
                var itemIndex = GameState.Instance.Inventory.FirstIndexOf(_requiredItem);
                if (itemIndex < 0)
                    return;
                if (_consumesItem && GameState.Instance.Inventory.TryRemoveAt(itemIndex, 1) > 0 == false)
                    return;
            }
            OnInteract?.Invoke();
        }
    }
}