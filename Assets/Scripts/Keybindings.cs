using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Keybindings : MonoBehaviour
{
    [SerializeField] private GameObject bindingPrefab;
    [SerializeField] private GameObject bindingPanel;
    
    private InputManager _inputManager;
    private string _keyToRebind = null;
    
    void Start()
    {
        _inputManager = GameObject.FindObjectOfType<InputManager>();
        string[] buttonNames = _inputManager.GetButtonNames();

        foreach (string buttonName in buttonNames)
        {
            string bn = buttonName;
            GameObject button = Instantiate(bindingPrefab, bindingPanel.transform);

            TMP_Text actionNameText = button.transform.Find("Action Text (TMP)").GetComponent<TMP_Text>();
            actionNameText.text = bn;

            TMP_Text keyboardButtonText = button.transform.Find("Keyboard Button").GetComponentInChildren<TMP_Text>();
            keyboardButtonText.text = _inputManager.GetKeyNameForButton(bn);

            Button keybindButton = button.transform.Find("Keyboard Button").GetComponent<Button>();
            keybindButton.onClick.AddListener(() => { StartRebindFor(bn); } );
        }
    }

    private void Update()
    {
        if (_keyToRebind != null)
        {
            if (Input.anyKeyDown) 
            // This is all necessary because Unity has no simple way to know *which* key was pressed
            {
                KeyCode[] keys = (KeyCode[]) Enum.GetValues(typeof(KeyCode));
                _keyToRebind = null;
            }
        }
    }

    private void StartRebindFor(string buttonName)
    {
        _keyToRebind = buttonName;
    }
}
