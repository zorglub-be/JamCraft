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
        GameState.Instance.Paused = false;
        Time.timeScale = 1f;
        //we check if it is loaded before exiting in case exit is caused by pause being unloaded by another source
        if (SceneManager.GetSceneByName("PauseMenu").isLoaded)
            SceneManager.UnloadSceneAsync("PauseMenu");
    }
}