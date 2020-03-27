using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Keybindings : MonoBehaviour
{
    [SerializeField] private GameObject bindingPrefab;
    [SerializeField] private GameObject bindingPanel;
    [SerializeField] private EventSystem _eventSystem;

    private Dictionary<string, TMP_Text[]> _buttonToLabel;
    private Dictionary<NeoInput.NeoKeyCode, KeyCode[]> _newMapping;
    private Dictionary<Toggle, KeyCode> _toggleToKey = new Dictionary<Toggle, KeyCode>();

    void Start()
    {
        string[] buttonNames = NeoInput.StringKeyCodes.Keys.ToArray();
        _buttonToLabel = new Dictionary<string, TMP_Text[]>();
        _newMapping = NeoInput.keyCodesMap;
        for (int i = 0; i < buttonNames.Length; i++)
        {
            var buttonName = buttonNames[i];
            string bn = buttonName;
            var keyboardKey = _newMapping[NeoInput.StringKeyCodes[bn]][0];
            var gamepadKey = _newMapping[NeoInput.StringKeyCodes[bn]][1];
            GameObject button = Instantiate(bindingPrefab, bindingPanel.transform);
            TMP_Text actionNameText = button.transform.Find("Action Text (TMP)").GetComponent<TMP_Text>();
            actionNameText.text = bn;

            TMP_Text keyboardButtonText = button.transform.Find("Keyboard Button").GetComponentInChildren<TMP_Text>();
            keyboardButtonText.text = keyboardKey.ToString();

            Toggle keybindButton = button.transform.Find("Keyboard Button").GetComponent<Toggle>();
            keybindButton.onValueChanged.AddListener((mode) => { StartRebindFor(keybindButton, bn, 0, mode); });

            if (i == 0)
                _eventSystem.SetSelectedGameObject(keybindButton.gameObject);
            TMP_Text gamepadButtonText = button.transform.Find("Gamepad Button").GetComponentInChildren<TMP_Text>();
            Toggle gamepadButton = button.transform.Find("Gamepad Button").GetComponent<Toggle>();
            //This assumes that the first 4 elements are Up, Down, Left and right
            //Since those are bound to the analog stick for the gamepad, we can't change them in this UI
            if (i > 3)
            {
                gamepadButtonText.text = gamepadKey.ToString();
                gamepadButton.onValueChanged.AddListener((mode) => { StartRebindFor(gamepadButton, bn, 1, mode); });
            }
            else
            {
                gamepadButtonText.text = "Left Analog Stick";
                gamepadButton.interactable = false;
            }

            _buttonToLabel.Add(bn, new[] {keyboardButtonText, gamepadButtonText});
            _toggleToKey.Add(keybindButton, keyboardKey);
            _toggleToKey.Add(gamepadButton, gamepadKey);


        }
    }

    public Toggle[] TogglesWithKey(KeyCode key)
    {
        List<Toggle> toggles = new List<Toggle>(5);
        foreach (var item in _toggleToKey)
        {
            if (item.Value == key)
            {
                toggles.Add(item.Key);
            }
        }
        return toggles.ToArray();
    }
    
    public void Save()
    {
        NeoInput.keyCodesMap = _newMapping;
        Close();
    }

    public void Close()
    {
        SceneManager.UnloadSceneAsync("KeybindingsMenu");
    }

    private void StartRebindFor(Toggle button, string buttonName, int index, bool mode)
    {
        if (mode)
        {
            StopAllCoroutines();
            StartCoroutine(Rebind(button, buttonName, index));
        }
    }

    private IEnumerator Rebind(Toggle button, string buttonName, int index)
    {
        //skip a frame to avoid toggle keypress to count as rebind
        yield return null;
        if (buttonName != null)
        {
            // This is all necessary because Unity has no simple way to know *which* key was pressed
            while (Input.anyKeyDown == false)
            {
                yield return null;
            }

            // This is every possible key that could have been pressed
            foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    _newMapping[NeoInput.StringKeyCodes[buttonName]][index] = key;
                    _buttonToLabel[buttonName][index].text = key.ToString();
                    _toggleToKey[button] = key;
                    button.isOn = false;
                    var withSameKey = TogglesWithKey(key);
                    if (withSameKey.Length > 1)
                    {
                        foreach (var item in withSameKey)
                        {
                            var colors = item.colors;
                            colors.normalColor = Color.red;
                            item.colors = colors;
                        }
                    }
                    else
                    {
                        var colors = button.colors;
                        colors.normalColor = new Color(0.5f, .5f, .5f,1f);
                        button.colors = colors;
                    }
                    break;
                }
            }


        }
    }
}