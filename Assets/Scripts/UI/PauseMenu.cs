using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public void Resume()
    {
        GameState.Instance.Paused = false;
    }

        public void OpenSettingsMenu()
        {
            SceneManager.LoadScene("SettingsMenu", LoadSceneMode.Additive);
        }
        public void OpenMainMenu()
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
            Destroy(GameState.Instance.Player);
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        private void OnDisable()
        {
            //Todo: this is dirty, need to improve
            if (SceneManager.GetSceneByName("KeybindingsMenu").isLoaded)
                SceneManager.UnloadSceneAsync("KeybindingsMenu");
            if (SceneManager.GetSceneByName("SettingsMenu").isLoaded)
                SceneManager.UnloadSceneAsync("SettingsMenu");
        }
}
