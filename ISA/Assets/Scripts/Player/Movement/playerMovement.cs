using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerMovement : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] movementLimiter moveLimit;
    private Rigidbody body;
    playerGround ground;
    public InputMap inputMap;
    private InputAction Move;

    [Header("Movement Stats")]
    [SerializeField, Range(0f, 20f)][Tooltip("Maximum movement speed")] public float maxSpeed = 10f;
    [SerializeField, Range(0f, 100f)][Tooltip("How fast to reach max speed")] public float maxAcceleration = 52f;
    [SerializeField, Range(0f, 100f)][Tooltip("How fast to stop after letting go")] public float maxDecceleration = 52f;
    [SerializeField, Range(0f, 100f)][Tooltip("How fast to stop when changing direction")] public float maxTurnSpeed = 80f;
    [SerializeField, Range(0f, 100f)][Tooltip("How fast to reach max speed when in mid-air")] public float maxAirAcceleration;
    [SerializeField, Range(0f, 100f)][Tooltip("How fast to stop in mid-air when no direction is used")] public float maxAirDeceleration;
    [SerializeField, Range(0f, 100f)][Tooltip("How fast to stop when changing direction when in mid-air")] public float maxAirTurnSpeed = 80f;
    [SerializeField][Tooltip("Friction to apply against movement on stick")] private float friction;

    [Header("Options")]
    [Tooltip("When false, the charcter will skip acceleration and deceleration and instantly move and stop")] public bool useAcceleration;
    public bool itsTheIntro = true;

    [Header("Calculations")]
    public Vector2 direction;
    private Vector2 desiredVelocity;
    public Vector3 velocity;
    private float maxSpeedChange;
    private float acceleration;
    private float deceleration;
    private float turnSpeed;

    [Header("Current State")]
    public bool onGround;
    public bool pressingKey;

    private void Awake()
    {
        //Find the character's Rigidbody and ground detection script
        body = GetComponent<Rigidbody>();
        ground = GetComponentInChildren<playerGround>();
        inputMap = new InputMap();
        inputMap.Enable();
        Move = inputMap.Map.Move;
        Move.Enable();


    }
 

    private void Update()
    {
        if (moveLimit.characterCanMove)
        {
            direction = Move.ReadValue<Vector2>();
            direction.x *= -1;
        }
        //Used to stop movement when the character is playing her death animation
        if (!moveLimit.characterCanMove && !itsTheIntro)
        {
            direction = Vector2.zero;
        }

        //Also tells us that we are currently pressing a direction button
        if (direction != Vector2.zero)
        {
            pressingKey = true;
            float rotationY = transform.eulerAngles.y * Mathf.Deg2Rad;
            float directionAngle = Vector2.SignedAngle(Vector2.up, direction) * Mathf.Deg2Rad;
            direction = new Vector2(Mathf.Sin(rotationY + directionAngle), Mathf.Cos(rotationY + directionAngle));
        }
        else
        {
            pressingKey = false;
        }

        //Calculate's the character's desired velocity - which is the direction you are facing, multiplied by the character's maximum speed
        //Friction is not used in this game
        desiredVelocity = direction * Mathf.Max(maxSpeed - friction, 0f);
    }

    private void FixedUpdate()
    {
        body.angularVelocity = Vector3.zero;
        //Fixed update runs in sync with Unity's physics engine

        //Get Kit's current ground status from her ground script
        onGround = ground.GetOnGround();

        //Get the Rigidbody's current velocity
        velocity = body.velocity;

        //Calculate movement, depending on whether "Instant Movement" has been checked
        if (useAcceleration)
        {
            runWithAcceleration();
        }
        else
        {
            if (onGround)
            {
                runWithoutAcceleration();
            }
            else
            {
                runWithAcceleration();
            }
        }
    }

    private void runWithAcceleration()
    {
        //Set our acceleration, deceleration, and turn speed stats, based on whether we're on the ground on in the air

        acceleration = onGround ? maxAcceleration : maxAirAcceleration;
        deceleration = onGround ? maxDecceleration : maxAirDeceleration;
        turnSpeed = onGround ? maxTurnSpeed : maxAirTurnSpeed;

        if (pressingKey)
        {
            //If the sign (i.e. positive or negative) of our input direction doesn't match our movement, it means we're turning around and so should use the turn speed stat.
            if (direction.normalized != new Vector2(velocity.x, velocity.z).normalized)
            {
                maxSpeedChange = turnSpeed * Time.deltaTime;
            }
            else
            {
                //If they match, it means we're simply running along and so should use the acceleration stat
                maxSpeedChange = acceleration * Time.deltaTime;
            }
        }
        else
        {
            //And if we're not pressing a direction at all, use the deceleration stat
            maxSpeedChange = deceleration * Time.deltaTime;
        }

        //Move our velocity towards the desired velocity, at the rate of the number calculated above
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.y, maxSpeedChange);

        //Update the Rigidbody with this new velocity
        body.velocity = velocity;

    }

    private void runWithoutAcceleration()
    {
        //If we're not using acceleration and deceleration, just send our desired velocity (direction * max speed) to the Rigidbody
        velocity.x = desiredVelocity.x;
        velocity.z = desiredVelocity.y;

        body.velocity = velocity;
    }
}
