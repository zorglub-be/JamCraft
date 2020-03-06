using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour, IPlayerInput
{
    public float Vertical => Input.GetAxis("Vertical");
    public float Horizontal => Input.GetAxis("Horizontal");
    
    public bool Attack1 => Input.GetButton("Fire1");
    public bool Attack2 => Input.GetButton("Fire2");
    public bool Attack3 => Input.GetButton("Fire3");

    public bool Interact => Input.GetButton("Submit");

    public bool PausePressed => Input.GetKeyDown(KeyCode.Escape);
    
}
