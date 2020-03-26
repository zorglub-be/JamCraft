using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Game Effects/Load Level")]
public class LoadLevelEffect : GameEffect
{
    [FormerlySerializedAs("_levelNames")] [SerializeField] private string[] _sceneNames;
    [SerializeField] private UnloadMode _unloadMode = UnloadMode.ActiveOnly;

    public enum UnloadMode
    {
        None,
        ActiveOnly,
        All,
    }

    public async override void Execute(GameObject source, Action callback = null)
    {
        Time.timeScale = 0;
        GameState.Instance.Loading = true;
        for (int i = 0; i < _sceneNames.Length; i++)
        {
            var mode = (i == 0 && _unloadMode == UnloadMode.All) ? LoadSceneMode.Single : LoadSceneMode.Additive;
            var asyncOp = SceneManager.LoadSceneAsync(_sceneNames[i], mode);
            var waitUntilLoaded = WaitUntilDone(asyncOp);
            await waitUntilLoaded;
        }
        if (_unloadMode == UnloadMode.ActiveOnly)
        {
            var currentLevel = SceneManager.GetActiveScene().name;
            var asyncOp = SceneManager.UnloadSceneAsync(currentLevel);
            var waitUntilUnloaded = WaitUntilDone(asyncOp);
            await waitUntilUnloaded;
        }

        if (_unloadMode != UnloadMode.None && _sceneNames.Length > 0)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(_sceneNames[0]));
        }
        GameState.Instance.CurrentLevelLoader = this;
        GameState.Instance.SaveGame();
        Time.timeScale = 1;
        GameState.Instance.Loading = false;
        GameState.Instance.Player.GetComponent<AbilitiesManager>().InitReferences();
        callback?.Invoke();
    }

    private async Task WaitUntilDone(AsyncOperation asyncOp)
    {
        while (asyncOp.isDone == false)
        {
            await Task.Yield();
        }
    }
}