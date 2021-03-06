﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject creditsPanel;

/*    public void StartNewGame()
    {
        //commented by zorglub: use Load Level Game Effect instead
        Debug.Log("Started New Game");
        SceneManager.LoadScene("Level1");
        SceneManager.LoadScene("HUD", LoadSceneMode.Additive);
        SceneManager.LoadScene("Inventory", LoadSceneMode.Additive);
        
    }
        */

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
        creditsPanel.SetActive(true);
    }

    public void CloseCredits()
    {
        Debug.Log("Closing Credits");
        creditsPanel.SetActive(false);
    }
    
}
