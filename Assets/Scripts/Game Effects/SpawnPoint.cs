using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public GameObject Spawn(GameObject prefab)
    {
        var spawn = Instantiate(prefab, transform.position, transform.rotation);
        return spawn;
    }
}