using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//TODO: This should be a singleton and belong to the game state maybe?
public class InputManager : MonoBehaviour
{
    private Dictionary<string, KeyCode> _buttonKeys;

    private void Awake()
    {
        _buttonKeys = new Dictionary<string, KeyCode>();
        
        //TODO: Read to and write from PlayerPrefs
        _buttonKeys["Left Action"] = KeyCode.LeftArrow;
        _buttonKeys["Right Action"] = KeyCode.RightArrow;
        _buttonKeys["Combo Action"] = KeyCode.UpArrow;
        _buttonKeys["Use Potion"] = KeyCode.DownArrow;
        _buttonKeys["Move Left"] = KeyCode.A;
        _buttonKeys["Move Right"] = KeyCode.D;
        _buttonKeys["Move Up"] = KeyCode.W;
        _buttonKeys["Move Down"] = KeyCode.S;
        _buttonKeys["Inventory"] = KeyCode.I;
        _buttonKeys["Pause/Menu"] = KeyCode.Escape;
    }

    public bool GetButtonDown(string buttonName)
    {
        if (_buttonKeys.ContainsKey(buttonName) == false)
        {
            return false;
        }

        return Input.GetKeyDown(_buttonKeys[buttonName]);
    }

    public string[] GetButtonNames()
    {
        return _buttonKeys.Keys.ToArray();
    }

    public string GetKeyNameForButton(string buttonName)
    {
        if (_buttonKeys.ContainsKey(buttonName) == false)
        {
            return "N/A";
        }

        return _buttonKeys[buttonName].ToString();
    }

    public void SetButtonForAction(string buttonName, KeyCode keyCode)
    {
        _buttonKeys[buttonName] = keyCode;
    }
}
