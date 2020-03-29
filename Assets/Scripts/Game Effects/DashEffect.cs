using System;
using System.Threading;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Effects/Dash")]
public class DashEffect : GameEffect
{
    [SerializeField] private float _dashDistance;
    [SerializeField] private LayerMask _obstacleLayers;
    [SerializeField] private float _collisionRadius;
    [SerializeField] private bool _clipNarrowObstacles;
    [SerializeField] private bool _debug;

    public override void Execute(GameObject source, Action callback = null, CancellationTokenSource tokenSource = null)
    {
        //if the source is moving, dash in its current movement direction
        var rb = source.GetComponentInChildren<Rigidbody2D>();
        var direction = rb.velocity;
        var sourceTransform = source.transform;
        if (direction.magnitude < 1)
        {
            //getting the spawn point will give us the direction to dash to
            var spawnPoints = source.GetComponentsInChildren<SpawnPoint>(false);
            direction = spawnPoints.Length > 0 ? spawnPoints[0].transform.right : sourceTransform.right;
        }

        var targetPoint = sourceTransform.position + (Vector3) direction.normalized * _dashDistance;
        if (_debug)
        {
            Debug.DrawLine(sourceTransform.position, targetPoint, Color.red, 1);
            Debug.DrawLine(targetPoint, targetPoint+Vector3.right * _collisionRadius, Color.green, 1);
            Debug.DrawLine(targetPoint, targetPoint+Vector3.up * _collisionRadius, Color.green, 1);
            Debug.DrawLine(targetPoint, targetPoint+Vector3.down * _collisionRadius, Color.green, 1);
            Debug.DrawLine(targetPoint, targetPoint+Vector3.left * _collisionRadius, Color.green, 1);
        }
        //check if we can clip through an obstacle
        Collider2D[] obstacles = new Collider2D[20];
        int nbObstacles = 0;
        if (_clipNarrowObstacles)
        {
            nbObstacles = Physics2D.OverlapCircleNonAlloc(targetPoint, _collisionRadius, obstacles, _obstacleLayers);
            for (int i = nbObstacles -1; i >= 0; i--)
            {
                if (obstacles[i].isTrigger)
                {
                    nbObstacles--;
                }
            }
        }
        if (_clipNarrowObstacles == false || nbObstacles > 0 )
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