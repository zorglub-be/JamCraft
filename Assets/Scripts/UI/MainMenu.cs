using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void StartNewGame()
    {
        Debug.Log("Started New Game");
    }

    public void ContinueFromLastSave()
    {
        Debug.Log("Starting Game From Most Recent Save");
    }

    public void OpenSettingsMenu()
    {
        Debug.Log("Opening Settings Menu");
        SceneManager.LoadScene("SettingsMenu", LoadSceneMode.Additive);
    }

    public void DisplayCredits()
    {
        Debug.Log("Showing Credits");
    }
    
}
