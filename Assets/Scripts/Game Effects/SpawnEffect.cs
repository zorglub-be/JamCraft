using System;
using System.Threading;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Effects/Spawn Object")]
public class SpawnEffect : GameEffect
{
    [SerializeField] private GameObject _objectToSpawn;

    public override void Execute(GameObject source, Action callback=null)
    {
        var spawnPoint = source.GetComponentsInChildren<SpawnPoint>(false)[0];
        if (ReferenceEquals(spawnPoint, null))
            return;
        spawnPoint.Spawn(_objectToSpawn);
        callback?.Invoke();
    }
}