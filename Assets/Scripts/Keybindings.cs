using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Keybindings : MonoBehaviour
{
    [SerializeField] private GameObject bindingPrefab;
    [SerializeField] private GameObject bindingPanel;
    
    private string _buttonToRebind = null;
    private int _indexToRebind;
    private Dictionary <string, TMP_Text> _buttonToLabel;
    
    void Start()
    {
        string[] buttonNames = NeoInput.StringKeyCodes.Keys.ToArray();
        _buttonToLabel = new Dictionary<string, TMP_Text>();

        foreach (string buttonName in buttonNames)
        {
            string bn = buttonName;
            GameObject button = Instantiate(bindingPrefab, bindingPanel.transform);
            TMP_Text actionNameText = button.transform.Find("Action Text (TMP)").GetComponent<TMP_Text>();
            actionNameText.text = bn;

            TMP_Text keyboardButtonText = button.transform.Find("Keyboard Button").GetComponentInChildren<TMP_Text>();
            keyboardButtonText.text = NeoInput.keyCodesMap[NeoInput.StringKeyCodes[bn]][0].ToString();
            _buttonToLabel[bn] = keyboardButtonText;

            Button keybindButton = button.transform.Find("Keyboard Button").GetComponent<Button>();
            keybindButton.onClick.AddListener(() => { StartRebindFor(bn, 0); } );

            TMP_Text gamepadButtonText = button.transform.Find("Gamepad Button").GetComponentInChildren<TMP_Text>();
            gamepadButtonText.text = NeoInput.keyCodesMap[NeoInput.StringKeyCodes[bn]][1].ToString();
            _buttonToLabel[bn] = gamepadButtonText;

            Button gamepadButton = button.transform.Find("Gamepad Button").GetComponent<Button>();
            gamepadButton.onClick.AddListener(() => { StartRebindFor(bn, 1); } );
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
                        NeoInput.keyCodesMap[NeoInput.StringKeyCodes[_buttonToRebind]][_indexToRebind] = key;
                        _buttonToLabel[_buttonToRebind].text = key.ToString();
                        _buttonToRebind = null;
                        break;
                    }
                }
            }
        }
    }

    private void StartRebindFor(string buttonName, int index)
    {
        _buttonToRebind = buttonName;
        _indexToRebind = index;
    }
}
