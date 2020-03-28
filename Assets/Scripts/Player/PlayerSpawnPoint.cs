using Cinemachine;
using UnityEngine;

public class PlayerSpawnPoint : MonoBehaviour
{
    private void Start()
    {
        var camera = GameState.Instance.Player.GetComponentInChildren<CinemachineVirtualCamera>();
        camera.enabled = false;
        GameState.Instance.Player.transform.position = transform.position;
        camera.enabled = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 2);
    }
}
