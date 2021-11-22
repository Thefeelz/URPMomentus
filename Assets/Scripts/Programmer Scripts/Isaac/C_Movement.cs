using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class C_Movement : MonoBehaviour
{
    [SerializeField] float playerAcceleration = 10f;
    [SerializeField] public float maxPlayerSpeedRunning = 10f;
    [SerializeField] float maxPlayerSpeedJumping = 10f;
    [Range(0f, 1f)][SerializeField] float playerDeceleration = 0.9f;
    [SerializeField] float playerJumpPower = 10f;
    [SerializeField] float playerStrafeSpeed = 10f;
    [SerializeField] float fallMultiplier = 2.5f;

    public bool isGrounded = true;
    P_WallRun wallrunner;
    public bool wallRunning() => wallrunner.isWallRunning;
    float distanceToGround;
    float currentRunSpeed;
    bool jumping = false;

    Rigidbody rb;
    CharacterStats playerStats;
    Animator anim;


    bool moveForward = false;
    bool moveBackward = false;
    bool moveSidetoSide = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        distanceToGround = GetComponentInChildren<Collider>().bounds.extents.y;
        playerStats = GetComponent<CharacterStats>();
        anim = GetComponent<Animator>();
        wallrunner = GetComponent<P_WallRun>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForGrounded();
        GetUserInput();
    }
    void FixedUpdate()
    {
        // This if statement gives us a more "Mario" Like jump, making gravity do a little more work once we reach the apex of a jump
        if(rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
    }

    void CheckForGrounded()
    {
        // Shoot a Ray at the ground that is half the length of our body to see if we are touching the ground
        if (Physics.Raycast(transform.position, -transform.up, distanceToGround + .05f))
        {
            
            isGrounded = true;
        }
        else
            isGrounded = false;
    }
    void GetUserInput()
    {
        // Testing Purposes to restore health
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            playerStats.ReplenishHealth(10f);
        }
        // Character Movement (Checks for them to be Grounded)
        if (isGrounded)
        {
            // Decelerate the Character (Will enter the decelerate function IF the velocity is not 0 AND the user is pressing no movement keys
            if (rb.velocity.z != 0 && ((!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))))
            {
                Decelerate();
            }
            // ===================================================IMPORTANT=======================================================
            // Strafe the Character (These can be not used, added to for force, etc...we will play with them and see what is good)

            if (Input.GetKeyDown(KeyCode.E))
            {
                rb.AddRelativeForce(Vector3.right * playerStrafeSpeed, ForceMode.Impulse);
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                rb.AddRelativeForce(Vector3.right * -playerStrafeSpeed, ForceMode.Impulse);
            }
        }
    }
    public void Jump()
    {
        if (isGrounded && !wallRunning())
        {
            jumping = true;
            StartCoroutine(TurnOffJumpDelay());
            rb.AddForce(Vector3.up * playerJumpPower, ForceMode.VelocityChange);
        }
        else if(isGrounded && wallRunning() && !wallrunner.wallLeft)
        {
            rb.MovePosition(-transform.right + transform.position);
            rb.AddForce((transform.up - transform.right) * (playerJumpPower), ForceMode.VelocityChange);
        }
        else if (isGrounded && wallRunning() && wallrunner.wallLeft)
        {
            rb.MovePosition(transform.right + transform.position);
            rb.AddForce((transform.up + transform.right) * (playerJumpPower), ForceMode.VelocityChange);
        }
    }

    private void Decelerate()
    {
        // Check to see if any movement keys are being pressed (Will only pass through if we are not grounded)
        if(!moveForward && !moveBackward && !moveSidetoSide && rb.velocity != Vector3.zero)
        {
            // Slowly chunk velocity by a constant less than 1
            rb.velocity *= playerDeceleration;
            if (rb.velocity.sqrMagnitude < 0.25f)
                rb.velocity = Vector3.zero;
        }
    }
    //Rotate the rigid body to be more inline with the physics system instead of rotating the transform
    public void StrafeCharacter(int rotationDirection)
    {
        rb.AddForce(transform.right * rotationDirection * playerAcceleration, ForceMode.VelocityChange);
        moveSidetoSide = true;
    }

    public void MoveForward()
    {
        /* @ Isaac impement code
         *  
         */
        if (!jumping || !isGrounded)
            rb.AddForce(transform.forward * playerAcceleration, ForceMode.VelocityChange);
        else
            rb.AddForce(-transform.forward * (playerAcceleration / 3) * Time.deltaTime, ForceMode.VelocityChange);
        moveForward = true;
        moveBackward = false;
    }

    public void MoveBackwards()
    {
        /* @ Isaac impement code
         * 
         */
        if (!jumping || !isGrounded)
            rb.AddForce(-transform.forward * playerAcceleration, ForceMode.VelocityChange);
        else
            rb.AddForce(-transform.forward * (playerAcceleration / 3), ForceMode.VelocityChange);
        moveBackward = true;
        moveForward = false;
    }

    public void SetMoveForwardFalse() { moveForward = false; }
    public void SetMoveBackwardsFalse() { moveBackward = false; }
    public void SetMoveSidetoSideFalse() { moveSidetoSide = false; }

    IEnumerator TurnOffJumpDelay()
    {
        yield return new WaitForSeconds(0.5f);
        jumping = false;
    }
}