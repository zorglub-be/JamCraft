using System;
using System.Collections.Generic;
using UnityEngine;

public static class NeoInput
{
    // this is for the dead zone
    private static float _axisDeadZone = 0.1f;

    // basic controls are configure as Qwerty for keyboard and PS4 Dual shock for joystick (on Windows)
    public static Dictionary<NeoKeyCode, KeyCode[]> keyCodesMap = new Dictionary<NeoKeyCode, KeyCode[]>
    {
        // Directions
        {NeoKeyCode.Up, new []{KeyCode.W}},
        {NeoKeyCode.Down, new []{KeyCode.S}},
        {NeoKeyCode.Left, new []{KeyCode.A}},
        {NeoKeyCode.Right, new []{KeyCode.D}},
        
        // Controls
        {NeoKeyCode.PrimaryAttack, new []{KeyCode.J, KeyCode.Joystick1Button1}}, // Cross
        {NeoKeyCode.SecondaryAttack, new []{KeyCode.K, KeyCode.Joystick1Button0}}, // Square
        {NeoKeyCode.SpecialAttack, new []{KeyCode.L, KeyCode.Joystick1Button3}}, // Triangle
        {NeoKeyCode.Interact, new []{KeyCode.Return, KeyCode.Joystick1Button2}}, // Circle
        {NeoKeyCode.NextPrimary, new []{KeyCode.Q, KeyCode.Joystick1Button4}}, //L1
        {NeoKeyCode.PreviousPrimary, new []{KeyCode.Z, KeyCode.Joystick1Button6}}, //L2
        {NeoKeyCode.NextSecondary, new []{KeyCode.E, KeyCode.Joystick1Button5}}, //R1
        {NeoKeyCode.PreviousSecondary, new []{KeyCode.C, KeyCode.Joystick1Button7}}, //R2
        {NeoKeyCode.Pause, new []{KeyCode.Escape, KeyCode.Joystick1Button9}}, // Options

        // Inventory
        {NeoKeyCode.ToggleInventory, new []{KeyCode.I, KeyCode.Joystick1Button11}}, //R3
        {NeoKeyCode.Use, new []{KeyCode.J, KeyCode.Joystick1Button1}}, // Cross
        {NeoKeyCode.Select, new []{KeyCode.Space, KeyCode.Joystick1Button0}}, // Square
        {NeoKeyCode.Drop, new []{KeyCode.Space, KeyCode.Joystick1Button2}}, // Circle
        {NeoKeyCode.Craft, new []{KeyCode.Return, KeyCode.Joystick1Button3}}, // Triangle
    };

    private static Dictionary<AxisCode, Axis> axisMapper = new Dictionary<AxisCode, Axis>
    {
        {AxisCode.Horizontal, new Axis("Joystick Horizontal", NeoKeyCode.Right, NeoKeyCode.Left)},
        {AxisCode.Vertical, new Axis("Joystick Vertical", NeoKeyCode.Up, NeoKeyCode.Down)},
    };
    
    

    public static float GetAxis(AxisCode axisCode)
    {
        var axis = axisMapper[axisCode];
        var value = Input.GetAxis(axis.axisName);
        if (Mathf.Abs(value) > _axisDeadZone)
            return Mathf.Sign(value);
        return KeyValue(axis.positiveKey) - KeyValue(axis.negativeKey);
    }
    
    /// <summary>
    /// Updates an axis value managed by the calling object and returns true if it is a new press of the axis.
    /// </summary>
    /// <param name="axisCode">the axis to read</param>
    /// <param name="axisValue">the axis value managed by the calling object</param>
    /// <param name="lastUseTime">the last time a new press of the axis was detected</param>
    /// <param name="useDelay">the delay between valid axis inputs during a single axis press</param>
    /// <param name="realtime">if set, the delay will be real time instead of game time</param>
    /// <returns>Returns true if it is a new press of the axis. Else returns false</returns>
    public static bool UpdateTimedAxis(AxisCode axisCode, ref float axisValue, ref float lastUseTime, float useDelay, bool realtime = false)
    {
        var time = realtime ? Time.realtimeSinceStartup : Time.time;
        if (axisValue != 0f && time - lastUseTime < useDelay)
        {
            axisValue = GetAxis(axisCode);
            return false;
        }
        axisValue = GetAxis(axisCode);
        if (Mathf.Approximately(axisValue, 0) == false)
        {
            lastUseTime = time;
            return true;
        }
        return false;
    }    
    
    public static int KeyValue(NeoKeyCode key)
    {
        return GetKey(key) ? 1 : 0;
    }

    public static bool GetKey(NeoKeyCode key)
    {
        foreach (var k in keyCodesMap[key])
        {
            if (Input.GetKey(k))
                return true;
        }
        return false;
    }
    public static bool GetKeyDown(NeoKeyCode key)
    {
        foreach (var k in keyCodesMap[key])
        {
            if (Input.GetKeyDown(k))
                return true;
        }
        return false;
    }
    public static bool GetKeyUp(NeoKeyCode key)
    {
        foreach (var k in keyCodesMap[key])
        {
            if (Input.GetKeyUp(k))
                return true;
        }
        return false;
    }

    private class Axis
    {
        public string axisName;
        public NeoKeyCode positiveKey;
        public NeoKeyCode negativeKey;

        public Axis(string axisName, NeoKeyCode positiveKey, NeoKeyCode negativeKey)
        {
            this.axisName = axisName;
            this.positiveKey = positiveKey;
            this.negativeKey = negativeKey;
        }
    }
    public enum AxisCode
    {
        Horizontal,
        Vertical,
    }
    public enum NeoKeyCode
    {
        Up,
        Down,
        Left,
        Right,
        Use,
        PrimaryAttack,
        Select,
        ToggleInventory,
        Craft,
        NextPrimary,
        PreviousPrimary,
        NextSecondary,
        PreviousSecondary,
        Pause,
        SecondaryAttack,
        SpecialAttack,
        Interact,
        Drop
    }
}
