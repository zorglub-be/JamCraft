using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootStrapper 
{
/// Commented by Zorglub: the input system should not be instantiated at runtime, instead it should be placed in the
/// first scene we load in a game. I changed PlayerInput to inherit from SingletonMB
/* 
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    public static void Initialize()
    {
        var inputGameObject = new GameObject("[INPUT SYSTEM]");
        inputGameObject.AddComponent<PlayerInput>();
        GameObject.DontDestroyOnLoad(inputGameObject);
    }
*/
}