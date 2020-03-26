using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : IState
{
    public void Tick()
    {
    }

    public void OnEnter()
    {
        SceneManager.LoadScene("PauseMenu", LoadSceneMode.Additive);
        GameState.Instance.Paused = true;
        Time.timeScale = 0f;
        
    }

    public void OnExit()
    {
        SceneManager.UnloadSceneAsync("PauseMenu");
        GameState.Instance.Paused = false;
        Time.timeScale = 1f;
    }
}