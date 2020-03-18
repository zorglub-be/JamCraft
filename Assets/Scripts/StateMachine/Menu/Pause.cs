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
        Time.timeScale = 0f;
        
    }

    public void OnExit()
    {
        SceneManager.UnloadSceneAsync("PauseMenu");
        Time.timeScale = 1f;
    }
}