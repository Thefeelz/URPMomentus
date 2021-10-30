using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class P_Movement : MonoBehaviour
{
    [SerializeField] float playerAcceleration = 10f;
    [SerializeField] float maxPlayerSpeedRunning = 10f;
    [SerializeField] float maxPlayerSpeedJumping = 10f;
    [Range(0f, 1f)][SerializeField] float playerDeceleration = 0.9f;
    [SerializeField] float playerJumpPower = 10f;
    [SerializeField] float playerStrafeSpeed = 10f;
    [SerializeField] float fallMultiplier = 2.5f;

    public bool isGrounded = true;
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
    }

    // Update is called once per frame
    void Update()
    {
        CheckForGrounded();
        GetUserInput();
    }
    void FixedUpdate()
    {
        // This is to toggle on and off running animation based on our velocity
        if(rb.velocity.magnitude > 2f)
        {
            anim.SetBool("running", true);
        }
        else
        {
            anim.SetBool("running", false);
        }
        // If statement to check our speed by its sqrMag (cheaper function) and decrease it by multiplying it by itself and a constant less than 1
        if(rb.velocity.sqrMagnitude > (currentRunSpeed * currentRunSpeed) && isGrounded)
        {
            rb.velocity *= 0.9f;
        }
        // If statement that will only be effected if we are in the air (currentRunSpeed is different if we are jumping)
        else if (rb.velocity.sqrMagnitude > (currentRunSpeed * currentRunSpeed))
        {
            rb.velocity *= 0.9f;
        }
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
            // Since we are limiting our max run speed, this effects our force when we jump, to counter this
            // I added in a jumping bool that allows our max speed while jumping to be increased so we get a full jump
            // This bool is turned off by a coroutine 0.5 seconds after a jump
            if(!jumping)
                currentRunSpeed = maxPlayerSpeedRunning;
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
        if (isGrounded)
        {
            jumping = true;
            StartCoroutine(TurnOffJumpDelay());
            currentRunSpeed = maxPlayerSpeedJumping;
            rb.AddForce(Vector3.up * playerJumpPower, ForceMode.VelocityChange);
        }
    }

    private void Decelerate()
    {
        // Check to see if any movement keys are being pressed (Will only pass through if we are not grounded)
        if(!moveForward && !moveBackward && !moveSidetoSide)
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
        //rb.rotation = rb.rotation * Quaternion.Euler(0, playerRotateSpeed * rotationDirection * Time.deltaTime, 0);
        rb.AddForce(transform.right * rotationDirection * playerAcceleration * Time.deltaTime, ForceMode.VelocityChange);
        moveSidetoSide = true;
    }

    public void MoveForward()
    {
        if (!jumping || !isGrounded)
            rb.AddForce(transform.forward * playerAcceleration * Time.deltaTime, ForceMode.VelocityChange);
        else
            rb.AddForce(transform.forward * (playerAcceleration / 2) * Time.deltaTime, ForceMode.VelocityChange);
        moveForward = true;
        moveBackward = false;
    }
    public void MoveBackwards()
    {
        if(!jumping || !isGrounded)
            rb.AddForce(-transform.forward * playerAcceleration * Time.deltaTime, ForceMode.VelocityChange);
        else
            rb.AddForce(-transform.forward * (playerAcceleration / 2) * Time.deltaTime, ForceMode.VelocityChange);
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