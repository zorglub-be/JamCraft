using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
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
}
