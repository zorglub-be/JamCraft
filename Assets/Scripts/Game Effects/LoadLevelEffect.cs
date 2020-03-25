using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Game Effects/Load Level")]
public class LoadLevelEffect : GameEffect
{
    [SerializeField] private string[] _levelNames;
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
        for (int i = 0; i < _levelNames.Length; i++)
        {
            var mode = (i == 0 && _unloadMode == UnloadMode.All) ? LoadSceneMode.Single : LoadSceneMode.Additive;
            var asyncOp = SceneManager.LoadSceneAsync(_levelNames[i], mode);
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
        Time.timeScale = 1;
        GameState.Instance.Loading = false;
        callback?.Invoke();
    }

    private async Task WaitUntilDone(AsyncOperation asyncOp)
    {
        while (asyncOp.isDone == false)
        {
            await Task.Delay(1);
        }
    }
}