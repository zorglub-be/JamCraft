using System;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "Game Effects/Spawn Object")]
public class SpawnEffect : GameEffect
{
    [Tooltip("set to true if you want to use the source's LootComponent to determine the object to spawn")]
    [SerializeField]
    private bool _useLootComponent;
    [SerializeField]
    [Tooltip("set to true if you want to use the source's SpawnPoint to spawn the object")]
    private bool _useSpawnPoint;
    [SerializeField] private GameObject _objectToSpawn;
    [Tooltip("Sets the spawned object's layer to the source layer")]
    [SerializeField] private bool _copyLayer = true;

    public override void Execute(GameObject source, Action callback=null)
    {
        GameObject toSpawn;
        GameObject spawnedObject = null;
        if (_useLootComponent)
        {
            if (GetFromLootComponent(source, out toSpawn) == false)
            {
                callback?.Invoke();
                return;
            }
        }
        else
            toSpawn = _objectToSpawn;
        if (toSpawn == null)
        {
            callback?.Invoke();
            return;
        }
        if (_useSpawnPoint)
        {
            var spawnPoint = source.GetComponentsInChildren<SpawnPoint>(false)[0];
            if (ReferenceEquals(spawnPoint, null) == false)
                spawnedObject = spawnPoint.Spawn(toSpawn);
        }
        if (spawnedObject == null)
            spawnedObject = Instantiate(toSpawn, source.transform.position, source.transform.rotation);
        if (_copyLayer)
        {
            spawnedObject.layer = source.layer;
            foreach (Transform t in spawnedObject.transform)
            {
                t.gameObject.layer = spawnedObject.layer;
            }
        }
        callback?.Invoke();
    }

    bool GetFromLootComponent(GameObject source, out GameObject lootItem)
    {
        var loot = source.GetComponent<LootComponent>();
        if (loot == null)
        {
            lootItem = null;
            return false;
        }
        lootItem = loot.lootItem;
        return ReferenceEquals(lootItem, null) == false;
    }
}