using UnityEngine;

public class PlayerInput : SingletonMB<PlayerInput>, IPlayerInput
{
    public static IPlayerInput Instance { get; private set; }
    
    public float Vertical => Input.GetAxis("Vertical");
    public float Horizontal => Input.GetAxis("Horizontal");


    /// 
    /// Comment by Zorglub: below is an example of how we could set up the playerinput to work with NeoInputManager
    /// Down, Left, Right, Up etc... could register as listeners to the OnDown, OnLeft, OnRight delegates
    /// 

    /*
    public float Vertical => Up - Down;

    public float Horizontal => Right - Left;

    public int Down
    {
        get => _down;
        set => _down = value;
    }

    public int Left
    {
        get => _left;
        set => _left = value;
    }

    public int Right
    {
        get => _right;
        set => _right = value;
    }
    
    public int Up { 
        get => _up; 
        set => _up = value; 
    }
    
    [SerializeField] private int _down;
    [SerializeField] private int _left;
    [SerializeField] private int _right;
    [SerializeField] private int _up;
     
    */
    

    public bool Attack1 => Input.GetButton("Fire1");
    public bool Attack2 => Input.GetButton("Fire2");
    public bool Attack3 => Input.GetButton("Fire3");

    public bool Interact => Input.GetButton("Submit");

    public bool PausePressed => Input.GetKeyDown(KeyCode.Escape);



    protected override void Initialize()
    {
    }

    protected override void Cleanup()
    {
        //do nothing
    }
}
