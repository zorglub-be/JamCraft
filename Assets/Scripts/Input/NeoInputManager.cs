using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class NeoInputManager : SingletonMB<NeoInputManager>
{
    private Dictionary<KeyCode, IntEvent> actionKeys = new Dictionary<KeyCode, IntEvent>();
    private Dictionary<string, IntEvent> actionNames = new Dictionary<string, IntEvent>();

    [SerializeField] public IntEvent OnUp;
    [SerializeField] public IntEvent OnLeft;
    [SerializeField] public IntEvent OnRight;
    [SerializeField] public IntEvent OnDown;
    [SerializeField] public IntEvent OnJump;
    [SerializeField] public IntEvent OnFire1;
    [SerializeField] public IntEvent OnFire2;
    [SerializeField] public IntEvent OnFire3;
    [SerializeField] public IntEvent OnFire4;

    protected override void Initialize()
    {
        actionNames.Add("Up", OnUp);
        actionKeys.Add(KeyCode.Z, OnUp);
        actionNames.Add("Left", OnLeft);
        actionKeys.Add(KeyCode.Q, OnLeft);
        actionNames.Add("Right", OnRight);
        actionKeys.Add(KeyCode.D, OnRight);
        actionNames.Add("Down", OnDown);
        actionKeys.Add(KeyCode.S, OnDown);
        actionNames.Add("Jump", OnJump);
        actionKeys.Add(KeyCode.Space, OnJump);
        actionNames.Add("Fire1", OnFire1);
        actionKeys.Add(KeyCode.J, OnFire1);
        actionNames.Add("Fire2", OnFire2);
        actionKeys.Add(KeyCode.K, OnFire2);
        actionNames.Add("Fire3", OnFire3);
        actionKeys.Add(KeyCode.L, OnDown);
        actionNames.Add("Fire4", OnFire4);
        actionKeys.Add(KeyCode.M, OnFire4);
    }

    protected override void Cleanup()
    {
        //do nothing
    }

    public void Rebind(string actionName, KeyCode key)
    {
        if (actionKeys.ContainsKey(key))
        {
            actionKeys[key] = actionNames[actionName];
        }
        else
        {
            actionKeys.Add(key, actionNames[actionName]);
        }
    }

    private void Update()
    {
        // This is every possible key that could have been pressed
        foreach (KeyCode key in actionKeys.Keys)
        {
            if (Input.GetKeyDown(key))
            {
                actionKeys[key]?.Invoke(1);
            }
/*
            if (Input.GetKeyUp(key))
            {
                actionKeys[key]?.Invoke(0);
            }*/
        }
    }
}