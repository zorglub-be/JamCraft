using UnityEngine.SceneManagement;

public class Menu : IState
{
    public void Tick()
    {
         
    }
 
    public void OnEnter()
    {
        PlayLevel.LevelToLoad = null;
        SceneManager.LoadSceneAsync("MainMenu");
    }
 
    public void OnExit()
    {
         
    }
}