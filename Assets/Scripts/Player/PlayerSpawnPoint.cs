using UnityEngine;

public class PlayerSpawnPoint : MonoBehaviour
{
    private void Start()
    {
        GameState.Instance.Player.transform.position = transform.position;
        GameState.Instance.Player.GetComponent<AbilitiesManager>().InitReferences();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 2);
    }
}
