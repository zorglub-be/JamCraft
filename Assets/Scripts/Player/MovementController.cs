using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] public float moveSpeed = 70;
    [SerializeField] public float m_MovementSmoothing = 0.1f;
    [SerializeField] private bool normalizedMovement = true;
    [SerializeField] private GameObject upObject;
    [SerializeField] private GameObject leftObject;
    [SerializeField] private GameObject rightObject;
    [SerializeField] private GameObject downObject;
    
    //public IPlayerInput PlayerInput = new PlayerInput();
    private IMover _mover;
    
    private Rigidbody2D _rigidbody2D;
    private Animator currentAnimator;

    private enum Direction { Up, Right, Down, Left };

    private Direction currentDirection;
    private Direction previousDirection;
    private float angle = 180;
    private float speed;
//    public PlayerInput PlayerInput;  //commented by Zorglub: we don't need this anymore
    private Vector2 axisVector = Vector2.zero;
    
    public float Horizontal { get; set; }
    public float Vertical { get; set; }

    private void Awake()
    {
     
        _mover = new Mover(this);
        _rigidbody2D = GetComponent<Rigidbody2D>();
        upObject.SetActive(false);
        leftObject.SetActive(false);
        rightObject.SetActive(false);
        downObject.SetActive(true);

        currentAnimator = downObject.GetComponent<Animator>();
        
    }
    
    void Update()
    {
        speed = _rigidbody2D.velocity.magnitude;

        // Get input axises
        
        // Modified by Zorglub: we don't want to read directly from input, instead we allow external components to set
        // the axis value. It's also bad for memory management to create a new vector every time, instead we update the
        // existing one
        
//        axisVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        axisVector.x = Horizontal;
        axisVector.y = Vertical;
            
        //normalize it for good topdown diagonal movement
        if (normalizedMovement == true)
        {
            axisVector.Normalize();
        }

        GetDirection();
        // Set speed parameter to the animator
        currentAnimator.SetFloat("Speed", speed);
    }



    private void GetDirection()
    {
        // Find out which direction to face and do what is appropiate
        //
        //Only update angle of direction if input axises are pressed
        if (!(axisVector.x == 0 && axisVector.y == 0))
        {
            // Find out what direction angle based on input axises
            angle = Mathf.Atan2(axisVector.x, axisVector.y) * Mathf.Rad2Deg;
            // Round out to prevent jittery direction changes.
            angle = Mathf.RoundToInt(angle);
        }

        if (angle > -45 && angle < 45) // UP
        {
            currentDirection = Direction.Up;
        }

        else if (angle < -135 || angle > 135) // DOWN
        {
            currentDirection = Direction.Down;
        }

        else if (angle >= 45 && angle <= 135) // RIGHT
        {
            currentDirection = Direction.Right;
        }

        else if (angle <= -45 && angle >= -135) // LEFT
        {
            currentDirection = Direction.Left;
        }

        // Did direction change?
        if (previousDirection != currentDirection)
        {
            if (currentDirection == Direction.Up)
            {
                // Activate appropiate game object
                upObject.SetActive(true);
                rightObject.SetActive(false);
                leftObject.SetActive(false);
                downObject.SetActive(false);

                currentAnimator = upObject.GetComponent<Animator>(); // note by Zorglub: we could cache the animator for better performance
            }

            else if (currentDirection == Direction.Down)
            {
                // Activate appropiate game object
                upObject.SetActive(false);
                rightObject.SetActive(false);
                leftObject.SetActive(false);
                downObject.SetActive(true);

                currentAnimator = downObject.GetComponent<Animator>();
            }

            else if (currentDirection == Direction.Right)
            {
                // Activate appropiate game object
                upObject.SetActive(false);
                rightObject.SetActive(true);
                leftObject.SetActive(false);
                downObject.SetActive(false);

                currentAnimator = rightObject.GetComponent<Animator>();
            }

            else if (currentDirection == Direction.Left)
            {
                // Activate appropiate game object
                upObject.SetActive(false);
                rightObject.SetActive(false);
                leftObject.SetActive(true);
                downObject.SetActive(false);

                currentAnimator = leftObject.GetComponent<Animator>();
            }
        }
        // Set current direction as previous
        previousDirection = currentDirection;
    }

    private void FixedUpdate()
    {
        _mover.Tick();
    }
}
