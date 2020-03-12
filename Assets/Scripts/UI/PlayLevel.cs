using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class PlayLevel : MonoBehaviour
{
    public static string LevelToLoad;
    [FormerlySerializedAs("_levelName")] [SerializeField] private string levelName;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => LevelToLoad = levelName);
    }
}