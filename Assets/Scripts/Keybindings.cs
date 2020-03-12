using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Keybindings : MonoBehaviour
{
    [SerializeField] private GameObject bindingPrefab;
    [SerializeField] private GameObject bindingPanel;
    
    private InputManager _inputManager;
    private string _buttonToRebind = null;
    private Dictionary <string, TMP_Text> _buttonToLabel;
    
    void Start()
    {
        _inputManager = GameObject.FindObjectOfType<InputManager>();
        string[] buttonNames = _inputManager.GetButtonNames();
        _buttonToLabel = new Dictionary<string, TMP_Text>();

        foreach (string buttonName in buttonNames)
        {
            string bn = buttonName;
            GameObject button = Instantiate(bindingPrefab, bindingPanel.transform);

            TMP_Text actionNameText = button.transform.Find("Action Text (TMP)").GetComponent<TMP_Text>();
            actionNameText.text = bn;

            TMP_Text keyboardButtonText = button.transform.Find("Keyboard Button").GetComponentInChildren<TMP_Text>();
            keyboardButtonText.text = _inputManager.GetKeyNameForButton(bn);
            _buttonToLabel[bn] = keyboardButtonText;

            Button keybindButton = button.transform.Find("Keyboard Button").GetComponent<Button>();
            keybindButton.onClick.AddListener(() => { StartRebindFor(bn); } );
        }
    }

    private void Update()
    {
        if (_buttonToRebind != null)
        {
            // This is all necessary because Unity has no simple way to know *which* key was pressed
            if (Input.anyKeyDown) 
            {
                // This is every possible key that could have been pressed
                foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(key))
                    {
                        _inputManager.SetButtonForAction(_buttonToRebind, key);
                        _buttonToLabel[_buttonToRebind].text = key.ToString();
                        _buttonToRebind = null;
                        break;
                    }
                }
            }
        }
    }

    private void StartRebindFor(string buttonName)
    {
        _buttonToRebind = buttonName;
    }
}
