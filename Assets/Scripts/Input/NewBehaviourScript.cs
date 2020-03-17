using System.Collections.Generic;
using UnityEngine;

public static class NeoInput
{
    // basic controls are configure as Qwerty for keyboard and PS4 Dual shock for joystick (on Windows)
    public static Dictionary<NeoKeyCode, KeyCode[]> _keyCodesMap = new Dictionary<NeoKeyCode, KeyCode[]>()
    {
        {NeoKeyCode.Up, new []{KeyCode.W}},
        {NeoKeyCode.Down, new []{KeyCode.S}},
        {NeoKeyCode.Left, new []{KeyCode.A}},
        {NeoKeyCode.Right, new []{KeyCode.D}},
        {NeoKeyCode.Use, new []{KeyCode.K, KeyCode.Joystick1Button0}}, // Square
        {NeoKeyCode.Attack, new []{KeyCode.J, KeyCode.Joystick1Button1}}, // Cross
        {NeoKeyCode.Select, new []{KeyCode.Space, KeyCode.Joystick1Button2}}, // Circle
        {NeoKeyCode.Craft, new []{KeyCode.Return, KeyCode.Joystick1Button3}}, // Triangle
        {NeoKeyCode.ToggleInventory, new []{KeyCode.I, KeyCode.Joystick1Button11}}, //R3
        {NeoKeyCode.NextPrimary, new []{KeyCode.Q, KeyCode.Joystick1Button5}}, //R1
        {NeoKeyCode.PreviousPrimary, new []{KeyCode.Z, KeyCode.Joystick1Button4}}, //L1
        {NeoKeyCode.NextSecondary, new []{KeyCode.E, KeyCode.Joystick1Button7}}, //R2
        {NeoKeyCode.PreviousSecondary, new []{KeyCode.C, KeyCode.Joystick1Button6}}, //L2
        {NeoKeyCode.Pause, new []{KeyCode.Escape, KeyCode.Joystick1Button9}}, // Options
    };
    
    public static float HorizontalAxis()
    {
        var value = Input.GetAxisRaw("Joystick Horizontal");
        if (Mathf.Approximately(value, 0))
            return KeyValue(NeoKeyCode.Right) - KeyValue(NeoKeyCode.Left);
        return value;
    }
    public static float VerticalAxis()
    {
        var value = Input.GetAxisRaw("Joystick Vertical");
        if (Mathf.Approximately(value, 0))
            return KeyValue(NeoKeyCode.Up) - KeyValue(NeoKeyCode.Down);
        return value;
    }

    
    public static int KeyValue(NeoKeyCode key)
    {
        return GetKey(key) ? 1 : 0;
    }

    public static bool GetKey(NeoKeyCode key)
    {
        foreach (var k in _keyCodesMap[key])
        {
            if (Input.GetKey(k))
                return true;
        }
        return false;
    }
    public static bool GetKeyDown(NeoKeyCode key)
    {
        foreach (var k in _keyCodesMap[key])
        {
            if (Input.GetKeyDown(k))
                return true;
        }
        return false;
    }
    public static bool GetKeyUp(NeoKeyCode key)
    {
        foreach (var k in _keyCodesMap[key])
        {
            if (Input.GetKeyUp(k))
                return true;
        }
        return false;
    }

    
    public enum NeoKeyCode
    {
        Up,
        Down,
        Left,
        Right,
        Use,
        Attack,
        Select,
        ToggleInventory,
        Craft,
        NextPrimary,
        PreviousPrimary,
        NextSecondary,
        PreviousSecondary,
        Pause,
    }
}
