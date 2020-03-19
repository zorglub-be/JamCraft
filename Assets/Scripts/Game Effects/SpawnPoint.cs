using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public GameObject Spawn(GameObject prefab)
    {
        return Instantiate(prefab, transform.position, transform.rotation);
    }
}