using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private AudioClip _unlockSound;
    [SerializeField] private Item _key;

    public void TryUnlock(GameObject character)
    {
        var charInventory = character.GetComponentInChildren<CharacterInventory>();
        if (charInventory)
        {
            if (charInventory.Inventory.TryRemove(_key))
            {
                Unlock();
            }
        }
            
    }

    public void Unlock()
    {
        GameState.Instance.AudioSource.PlayOneShot(_unlockSound);
        gameObject.SetActive(false);
    }
}
