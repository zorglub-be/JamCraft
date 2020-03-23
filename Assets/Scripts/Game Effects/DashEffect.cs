using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Effects/Dash")]
public class DashEffect : GameEffect
{
    [SerializeField] private float _dashDistance;
    [SerializeField] private LayerMask _obstacleLayers;
    [SerializeField] private float _collisionRadius;
    [SerializeField] private bool _clipNarrowObstacles;

    public override void Execute(GameObject source, Action callback = null)
    {
        //if the source is moving, dash in its current movement direction
        var rb = source.GetComponentInChildren<Rigidbody2D>();
        var direction = rb.velocity;
        var sourceTransform = source.transform;
        if (direction == Vector2.zero)
        {
            //getting the spawn point will give us the direction to dash to
            var spawnPoints = source.GetComponentsInChildren<SpawnPoint>(false);
            direction = spawnPoints.Length > 0 ? spawnPoints[0].transform.right : sourceTransform.right;
        }

        var targetPoint = sourceTransform.position + (Vector3) direction.normalized * _dashDistance;
        //check if we can clip through an obstacle
        if (_clipNarrowObstacles == false || Physics2D.OverlapCircle(targetPoint, _collisionRadius, _obstacleLayers))
        {
            //check if there's something in the way
            var hit = Physics2D.CircleCast(source.transform.position, _collisionRadius, direction, _dashDistance,
                _obstacleLayers);
            if (hit)
            {
                targetPoint = sourceTransform.position + (Vector3) direction.normalized * (hit.distance - _collisionRadius);
            }
        }
        sourceTransform.position = targetPoint;
        callback?.Invoke();
    }
}