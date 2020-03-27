using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExitTrigger : MonoBehaviour
{
    [Tooltip("Look at the build settings for build index.")]
    [SerializeField] private string nextLevelToLoad;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(nextLevelToLoad);
        }
    }
}
