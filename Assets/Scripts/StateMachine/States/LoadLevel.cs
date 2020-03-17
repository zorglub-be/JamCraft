using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : IState
{
    public bool Finished() => _operations.TrueForAll(t =>t.isDone);
    private List<AsyncOperation> _operations = new List<AsyncOperation>();
    public void Tick()
    {
    }

    public void OnEnter()
    {
        Debug.Log(PlayLevel.LevelToLoad);
        _operations.Add(SceneManager.LoadSceneAsync(PlayLevel.LevelToLoad));
        _operations.Add(SceneManager.LoadSceneAsync("HUD", LoadSceneMode.Additive));
    }

    public void OnExit()
    {
        _operations.Clear();
    }
}