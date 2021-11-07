using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vinMovement : MonoBehaviour
{
    Rigidbody playerBody;
    private float zAxis;
    private float xAxis;
    private Camera cam;

    //Holds respawn location
    private Vector3 spawn;

    //Used for movement, is a set value that will effect all player states
    public float speed;
    
    //Jump
    private bool jumpAllowed;
    [SerializeField] private int jumpForce=5, wallJumpForce;

    private bool isGrounded;

    //Multipliers, change player's speed based on their state
    private float multiplier;
    public float groundMultiplier = 1f;
    public float airMultiplier;
    public float wallMultiplier;

    //Wallrunning section
    [SerializeField] private LayerMask parkourWall;
    [SerializeField] private float wallrunForce, minWallrunSpeed, maxWallrunSpeed, maxLean, leanRate;
    private bool isWallrunning, isWallRight, isWallLeft, rotAdjusted=false, burst=false;
    private float leanPos = 0;
    RaycastHit hitLeftWall, hitRightWall;
        

    //Start is called before the first frame update
    void Start()
    {
        spawn = transform.position;                     /// set spawn point equal to position at start
        playerBody = GetComponent<Rigidbody>();         /// get reference to the object's rigidbody
        cam = GetComponentInChildren<Camera>();         /// get reference to camera which should be a child
        multiplier = groundMultiplier;                  /// set base multiplier
    }

    
    void Update()
    {
        //Check for player input as soon as possible
        GetKeyInput();

        //If not wallrunning Set the playerbody rotation based on the camera
        if (!isWallrunning)
        {
            SetRotation();                                                     
        }
        
        //If on ground, change state, multiplier, drag, and allow jumps
        if (Physics.Raycast(transform.position, -Vector3.up, 1.2f))            
        {
            isGrounded = true;
            multiplier = groundMultiplier;
            playerBody.drag = 2;
            jumpAllowed = true;
        }
        else
        {
            isGrounded = false;
        }

        //If in the air, use airMultiplier and turn off jumping
        if (!isGrounded && !isWallrunning)                                       
        {
            multiplier = airMultiplier;
            jumpAllowed = false;
        }

        //Check if there is a wall nearby
        WallrunCheck();

             

        // JUMP SECTION //

        //Normal Jump, add an impulse force upwards
        if (Input.GetKeyDown(KeyCode.Space) && jumpAllowed)                     
        {
            playerBody.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
        }

        //Wall jump, apply both an impulse force upwards and outwards from wall  
        if (Input.GetKeyDown(KeyCode.Space) && jumpAllowed && isWallRight)                      /// jump right to left
        {
            playerBody.AddForce(-transform.right * wallJumpForce, ForceMode.Impulse);
            playerBody.AddForce(transform.forward * wallJumpForce, ForceMode.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.Space) && jumpAllowed && isWallLeft)                       /// jump left to right
        {
            playerBody.AddForce(transform.right * wallJumpForce, ForceMode.Impulse);
            playerBody.AddForce(transform.forward * wallJumpForce, ForceMode.Impulse);
        }
    }

    //Update is called once per frame
    void FixedUpdate()

    // each frame, apply physics for more consistent and smoother movement
    {
        ApplyMovement();
    }

    // Called every update, checks what direction the player wants to go in and store it
    public void GetKeyInput()                                                  
    {
        zAxis = Input.GetAxis("Vertical");           /// forward (w) and back (s)
        xAxis = Input.GetAxis("Horizontal");         /// left (a) and right (d)
    }
    
   
    //Called every fixed update, uses the stored inputs from GetKeyInput and applies the forces to the player
    void ApplyMovement()
    {
        //Based on input, creates a vector direction in world space 
        Vector3 forwardVelocity =transform.forward* zAxis;                          /// converts the vertical input from local to world space
        Vector3 horizontalVelocity = transform.right * xAxis;                       /// converts the horizontal input from local to world space
        Vector3 directionalVelocity = forwardVelocity + horizontalVelocity;         /// makes a vector of the desired direction in world space

        directionalVelocity = directionalVelocity.normalized;                       /// normalizes the vector direction to avoid speed strafing

        //Modifies vector based on speed and multiplier
        Vector3 desiredVelocity = directionalVelocity* speed * multiplier;          /// adds in the base speed and the state multiplier
        desiredVelocity += new Vector3(0, playerBody.velocity.y, 0);                /// allows for gravity to act normally
        
        //applies created force to player
        playerBody.AddForce(desiredVelocity);
    }

    //Called every update, sets the rotation of the player's body equal to the camera rotation
    /// when tried in fixedUpdate, pieces attached to the player apear to "jitter" with camera movement
    void SetRotation()                                                          
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, cam.transform.eulerAngles.y, transform.eulerAngles.z);
    }


    
    // WALLRUN SECTION //

    //Called in update, sends out a raycast looking for objects with "parkourWall" layer. Then, if all conditions are true, it starts wallrun
    private void WallrunCheck()
    {
        isWallRight = Physics.Raycast(transform.position, transform.right, out hitRightWall, .8f, parkourWall);         /// checks right side
        isWallLeft = Physics.Raycast(transform.position, -transform.right, out hitLeftWall, .8f, parkourWall);          /// checks left side
        if ((isWallRight || isWallLeft) && !isGrounded)                                                                 /// CONDITION: must be next to wall and not be on the ground
        {
            StartWallrun();
        }

        //Ends the wallrun if raycasts don't hit a surface
        else StopWallrun(); 
    }


    private void StartWallrun()
    {
        //Change state, multiplier, drag, and allow jumps
        isWallrunning = true;
        multiplier = wallMultiplier;         
        playerBody.drag = 2;
        jumpAllowed = true;                 

        
        if (isWallRight)
        {
            //Checks if the rotation of the player's body has been set parallel to the wall
            if (!rotAdjusted)
            {
                //Creates a parallel vector using the cross-product of the player and the normal vector of the wall. Source:https://answers.unity.com/questions/989625/how-to-set-an-objects-rotation-parallel-to-a-wall.html
                Vector3 temp = Vector3.Cross(transform.up, hitRightWall.normal);
                transform.rotation = Quaternion.LookRotation(temp);
                rotAdjusted = true;
            }

            //The current lean(leanPos) grows until it reaches maximum lean(maxLean)
            if (leanPos < maxLean)
            {
                //The rate of change (leanRate) is adjusted based on time since the last frame which makes it smoother
                leanPos = leanPos + leanRate * Time.deltaTime;  
            }
            //Smoothly rotates the camera for the "tilt" effect. Uses Slerp which slows down the rotation at the beginning & end so it's not so "jarring" 
            cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, cam.transform.rotation * Quaternion.Euler(0, 0, leanPos), 1f);
        }

        //Flips values for a left-sided wallrun
        else if (isWallLeft)
        {
            if (!rotAdjusted)
            {
                Vector3 temp = Vector3.Cross(transform.up, hitLeftWall.normal);
                transform.rotation = Quaternion.LookRotation(-temp);
                rotAdjusted = true;
            }

            if (leanPos > -maxLean)
            {
                leanPos = leanPos - leanRate * Time.deltaTime;
            }
            cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, cam.transform.rotation * Quaternion.Euler(0, 0, leanPos), 1f);        
        }


        //Will turn gravity back on if player is moving too slow
        if (playerBody.velocity.magnitude < minWallrunSpeed)
        {
            playerBody.useGravity = true;
        }
        else
        {
            playerBody.useGravity = false;
        }


        //When the player being the wallrun, they are given a burst of speed
        if (playerBody.velocity.magnitude < maxWallrunSpeed && !burst)
        {
            playerBody.AddForce(transform.forward * wallrunForce * Time.deltaTime, ForceMode.VelocityChange);           ///VelocityChange ignores mass

            //When player reaches the max speed, the burst ends
            if (playerBody.velocity.magnitude >= maxWallrunSpeed-1)
            {
                burst = true;
            }

            //Initially pushes player into wall so they stay on and not bounce off 
            if (isWallRight)
            {
                playerBody.AddForce(transform.right * wallrunForce * Time.deltaTime);                                   ///wall is on the right
            }
            else
            {
                playerBody.AddForce(-transform.right * wallrunForce * Time.deltaTime);                                  ///wall is on the left
            }
        }
    }

    //Resets wallrun variables: turn gravity back on, deactivate wallrunning state, rotation is no longer adjusted, lean is back to vertical, burst has not been applied
    private void StopWallrun()
    {
        playerBody.useGravity = true;                                           
        isWallrunning = false;                                                  
        rotAdjusted = false;
        leanPos = 0;
        burst = false;
    }



    // RESPAWN //

    //Resets player if they fall off the map using the postion
    private void OnTriggerEnter(Collider other)                                 
    {
        if (other.tag == "catchPlayer")
        {
            transform.position = spawn;
        }
    }
}
