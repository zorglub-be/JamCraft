using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class SimpleCharacterController : MonoBehaviour {

    public float moveSpeed = 70;
    public float m_MovementSmoothing = 0.1f;
    public bool normalizedMovement = true;
    public GameObject upObject;
    public GameObject leftObject;
    public GameObject rightObject;
    public GameObject downObject;

    Rigidbody2D rb;
    Animator currentAnimator;

    enum Direction { Up, Right, Down, Left };
    enum Expression { Neutral, Angry, Smile, Surprised };

    Direction currentDirection;
    Direction previousDirection;
    float angle = 180;
    float speed;

    Vector2 axisVector = Vector2.zero;
    Vector3 currentVelocity = Vector3.zero;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        upObject.SetActive(false);
        leftObject.SetActive(false);
        rightObject.SetActive(false);
        downObject.SetActive(true);

        currentAnimator = downObject.GetComponent<Animator>();
    }

    void Update()
    {

        // get speed from the rigid body to be used for animator parameter Speed
        speed = rb.velocity.magnitude;

        // Get input axises
        axisVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        //normalize it for good topdown diagonal movement
        if (normalizedMovement == true)
        { 
            axisVector.Normalize();
        }

        // Find out which direction to face and do what is appropiate
        //

        // Only update angle of direction if input axises are pressed
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

            else if ( currentDirection == Direction.Down)
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


        // Check keys for actions and use appropiate function
        //
        if (Input.GetKey(KeyCode.Space))  // SWING ATTACK
        {
            PlayAnimation("Swing");
        }

        else if (Input.GetKey(KeyCode.C))  // THRUST ATTACK
        {
            PlayAnimation("Thrust");
        }

        else if (Input.GetKey(KeyCode.X))  // BOW ATTACK
        {
            PlayAnimation("Bow");
        }

        else if (Input.GetKey(KeyCode.V))  // SET NEUTRAL FACE
        {
            SetExpression(Expression.Neutral);
        }

        else if (Input.GetKey(KeyCode.B))  // SET ANGRY FACE
        {
            SetExpression(Expression.Angry);
        }
    }

    void FixedUpdate()
    {
        // Move our character
        Move();
    }

    void PlayAnimation(string animationName)
    {
        // Play given animation in the current directions animator
        currentAnimator.Play(animationName, 0);
    }

    void SetExpression(Expression expressionToSet)
    {
        // convert enum to int for the animator paremeter.
        int expressionNumber = (int)expressionToSet;

        // If the current direction is not up change expression (Up direction doesn't show any expressions)
        if (!(currentDirection == Direction.Up)) // UP
        {
            currentAnimator.SetInteger("Expression", expressionNumber);
        }

    }

    void Move()
    {
        // Set target velocity to smooth towards
        Vector2 targetVelocity = new Vector2(axisVector.x * moveSpeed * 10f, axisVector.y * moveSpeed * 10) * Time.fixedDeltaTime;

        // Smoothing out the movement
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref currentVelocity, m_MovementSmoothing);
    }
}
