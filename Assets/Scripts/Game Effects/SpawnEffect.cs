using System;
using System.Threading;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Effects/Spawn Object")]
public class SpawnEffect : GameEffect
{
    [SerializeField] private GameObject _objectToSpawn;
    [Tooltip("Sets the spawned object's layer to the source layer")]
    [SerializeField] private bool _copyLayer = true;

    public override void Execute(GameObject source, Action callback=null)
    {
        var loot = source.GetComponent<LootComponent>();
        if (ReferenceEquals(loot, null) == false)
            _objectToSpawn = loot.lootItem;
        var spawnPoint = source.GetComponentsInChildren<SpawnPoint>(false)[0];
        GameObject spawn;
        if (ReferenceEquals(spawnPoint, null) == false)
            spawn = spawnPoint.Spawn(_objectToSpawn);
        else
            spawn = Instantiate(_objectToSpawn, source.transform.position, source.transform.rotation);
        if (_copyLayer)
        {
            spawn.layer = source.layer;
            foreach (Transform t in spawn.transform)
            {
                t.gameObject.layer = spawn.layer;
            }
        }
        callback?.Invoke();
    }
}