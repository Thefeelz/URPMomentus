using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class P_Movement : MonoBehaviour
{
    [SerializeField] public float maxPlayerSpeedRunning = 10f;
    [Range(0f, 1f)][SerializeField] float playerDeceleration = 0.9f;
    [SerializeField] float playerJumpPower = 10f;
    [SerializeField] float playerStrafeSpeed = 10f;
    [SerializeField] float fallMultiplier = 2.5f;
    [Range(0f, 1f)][SerializeField] float inAirControlMultiplier = 0.25f;

    public bool isGrounded = true;
    P_WallRun wallrunner;
    public bool wallRunning() => wallrunner.isWallRunning;
    float distanceToGround;

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
        Debug.DrawRay(transform.position + new Vector3(0, distanceToGround, 0), -Vector3.up * (distanceToGround + .15f), Color.red, .1f);
        if (Physics.Raycast(transform.position + new Vector3(0, distanceToGround, 0), -Vector3.up, distanceToGround + .15f))
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
        }
    }
    public void Jump()
    {
        if (isGrounded && !wallRunning())
        {
            rb.AddForce(Vector3.up * playerJumpPower, ForceMode.VelocityChange);
        }
        else if(isGrounded && wallRunning() && !wallrunner.wallLeft)
        {
            rb.MovePosition(-transform.right + transform.position);
            rb.AddForce((transform.up - (transform.right * 0.5f)) * (playerJumpPower), ForceMode.Impulse);
        }
        else if (isGrounded && wallRunning() && wallrunner.wallLeft)
        {
            rb.MovePosition(transform.right + transform.position);
            rb.AddForce((transform.up + (transform.right * 0.5f)) * (playerJumpPower), ForceMode.Impulse);
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

    public void HandleMovement(Vector3 movementVector)
    {
        if(wallRunning()) { return; }
        if (isGrounded)
        {
            movementVector *= maxPlayerSpeedRunning;
            rb.velocity = new Vector3(movementVector.x, rb.velocity.y, movementVector.z);
        }
        else
        {
            if((Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.z)) > maxPlayerSpeedRunning) { return; }
            movementVector *= inAirControlMultiplier;
            rb.AddForce(movementVector, ForceMode.Impulse);
        }
    }
    ////Rotate the rigid body to be more inline with the physics system instead of rotating the transform
    //public void StrafeCharacter(int rotationDirection)
    //{
    //    float velocity = 0;
    //    if (isGrounded)
    //    {
    //        velocity = maxPlayerSpeedRunning;
    //    }
    //    else
    //    {
    //        velocity = maxPlayerSpeedRunning * inAirControlMultiplier;
    //    }
    //    if (moveForward)
    //    {
    //        Vector3 newVelocity = new Vector3(transform.forward.x + (transform.right.x * rotationDirection), 0, transform.forward.z + (transform.right.z * rotationDirection)).normalized;
    //        newVelocity *= velocity;
    //        rb.velocity = new Vector3(0, rb.velocity.y, 0) + newVelocity;
    //    }
    //    else if (moveBackward)
    //    {
    //        Vector3 newVelocity = new Vector3(transform.forward.x + (transform.right.x * -rotationDirection), 0, transform.forward.z + (transform.right.z * -rotationDirection)).normalized;
    //        newVelocity *= velocity;
    //        rb.velocity = new Vector3(0, rb.velocity.y, 0) - newVelocity;
    //    }
    //    else
    //    {
    //        Vector3 forwardVelocity = new Vector3(transform.right.x * rotationDirection, 0, transform.right.z * rotationDirection) * velocity;
    //        rb.velocity = new Vector3(0, rb.velocity.y, 0) + forwardVelocity;
    //    }

    //    moveSidetoSide = true;
    //}

    //public void MoveForward()
    //{
    //    if (wallRunning()) { return; }
    //    float velocity = 0;
    //    if (isGrounded)
    //    {
    //        velocity = maxPlayerSpeedRunning;
    //        Vector3 forwardVelocity = new Vector3(transform.forward.x, 0, transform.forward.z) * velocity;
    //        rb.velocity = new Vector3(0, rb.velocity.y, 0) + forwardVelocity;
    //    }
    //    else
    //    {
    //        velocity = playerInAirForce;
    //        rb.AddForce(transform.forward * velocity * inAirControlMultiplier, ForceMode.VelocityChange);
    //    }



    //    moveForward = true;
    //    moveBackward = false;
    //}

    //public void MoveBackwards()
    //{
    //    if (wallRunning()) { return; }
    //    float velocity = 0;
    //    if (isGrounded)
    //    {
    //        velocity = maxPlayerSpeedRunning;
    //        Vector3 forwardVelocity = new Vector3(transform.forward.x, 0, transform.forward.z) * velocity;
    //        rb.velocity = new Vector3(0, rb.velocity.y, 0) - forwardVelocity;
    //    }
    //    else
    //    {
    //        velocity = playerInAirForce;
    //        rb.AddForce(transform.forward * -velocity * inAirControlMultiplier, ForceMode.VelocityChange);
    //    }

    //    moveForward = true;
    //    moveBackward = false;
    //}

    public void SetMoveForwardFalse() { moveForward = false; }
    public void SetMoveBackwardsFalse() { moveBackward = false; }
    public void SetMoveSidetoSideFalse() { moveSidetoSide = false; }

    public void SetPlayerSpeed(float newSpeed) { maxPlayerSpeedRunning = newSpeed; }
    public void SetPlayerJump(float newJump) { playerJumpPower = newJump; }
    public void SetPlayerInAirControl(float newControl) { inAirControlMultiplier = Mathf.Clamp01(newControl); }
    public void SetFallMultiplier(float newMultiplier) { fallMultiplier = newMultiplier; }
}