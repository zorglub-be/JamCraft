using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 70;
    [SerializeField] private float m_MovementSmoothing = 0.1f;
    [SerializeField] private bool normalizedMovement = true;
    [SerializeField] private GameObject upObject;
    [SerializeField] private GameObject leftObject;
    [SerializeField] private GameObject rightObject;
    [SerializeField] private GameObject downObject;
    [SerializeField] private bool allowDiagonalMovement = true;
    [SerializeField] private bool lookAtMouse = false;
    private Vector2 movementDirection;
    private Rigidbody2D _rigidbody2D;
    private Animator currentAnimator;

    private enum Direction { Up, Right, Down, Left };

    private Direction currentDirection;
    private Direction previousDirection;
    private float angle = 180;
    private float speed;

    private Vector2 axisVector = Vector2.zero;
    private Vector3 currentVelocity = Vector3.zero;

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        upObject.SetActive(false);
        leftObject.SetActive(false);
        rightObject.SetActive(false);
        downObject.SetActive(true);

        currentAnimator = downObject.GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            currentAnimator.Play("Swing", 0);
        }

        if (Input.GetButton("Fire2"))
        {
            currentAnimator.Play("Thrust", 0);
        }

        if (Input.GetButton("Fire3") || Input.GetButton("Fire1") && Input.GetButton("Fire2") )
        {
            currentAnimator.Play("Bow", 0);
        }
        // get speed from the rigid body to be used for animator parameter Speed
        speed = _rigidbody2D.velocity.magnitude;

        // Get input axises
        axisVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        //normalize it for good topdown diagonal movement
        if (normalizedMovement == true)
        {
            axisVector.Normalize();
        }

        // Find out which direction to face and do what is appropiate
        //

        
        if (lookAtMouse)
        {
            currentDirection = Direction.Down;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 facingDirection = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        angle = Mathf.Atan2(facingDirection.y, facingDirection.x) * Mathf.Rad2Deg + 90;
        transform.eulerAngles = new Vector3(0, 0, angle);
        }
        else
        {
            transform.eulerAngles = Vector3.zero;
            //Debug.Log(axisVector);
             //Only update angle of direction if input axises are pressed
            if (!(axisVector.x == 0 && axisVector.y == 0))
            {
                // Find out what direction angle based on input axises
                angle = Mathf.Atan2(axisVector.x, axisVector.y) * Mathf.Rad2Deg;

                // Round out to prevent jittery direction changes.
                angle = Mathf.RoundToInt(angle);
            }
            if (angle > -45 && angle < 45)  // UP
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

            else if (angle <= -45 && angle >= -135)  // LEFT
            {
                currentDirection = Direction.Left;
            }
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

                currentAnimator = upObject.GetComponent<Animator>();
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

        // Set speed parameter to the animator
        currentAnimator.SetFloat("Speed", speed);

        // Set current direction as previous
        previousDirection = currentDirection;


    }

    void FixedUpdate()
    {
        // Move our character
        Move();
        //TurnTowardsMouse();
    }

    private void TurnTowardsMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 facingDirection = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        float angle = Mathf.Atan2(facingDirection.y, facingDirection.x) * Mathf.Rad2Deg + 90;
        transform.eulerAngles = new Vector3(0, 0, angle);

    }


    public void Move()
    {
        if (allowDiagonalMovement)
        {
            movementDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            //movementDirection.Normalize();
        }
        else
        {
            if (Input.GetAxis("Vertical") > 0)
            {
                movementDirection = Vector2.up;
                //return;
            }

            else if (Input.GetAxis("Horizontal") > 0)
            {
                movementDirection = Vector2.right;
                //return;
            }

            else if (Input.GetAxis("Vertical") < 0)
            {
                movementDirection = Vector2.down;
                //return;
            }

            else if (Input.GetAxis("Horizontal") < 0)
            {
                movementDirection = Vector2.left;
                //return;
            }
            else
            {
                movementDirection = Vector2.zero;
            }
        }
        // Set target velocity to smooth towards
        Vector2 targetVelocity = new Vector2(movementDirection.x * moveSpeed * 10f, movementDirection.y * moveSpeed * 10) * Time.fixedDeltaTime;

        // Smoothing out the movement
        _rigidbody2D.velocity = Vector3.SmoothDamp(_rigidbody2D.velocity, targetVelocity, ref currentVelocity, m_MovementSmoothing);
        //_rigidbody2D.velocity = movementDirection * moveSpeed * Time.deltaTime;
    }
}
