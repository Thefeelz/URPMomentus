using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Input : MonoBehaviour
{
    [SerializeField] Canvas helpScreen;
    P_CoolDownManager coolDownManager;
    P_GroundSlide groundSlide;
    C_Movement movement;
    PlayerAttack playerAttack;

    A_BladeDance bladeDance;
    A_AirDash airDash;
    A_SwordThrow swordThrow;

    Rigidbody rb;
    // Start is called before the first frame update
    void Awake()
    {
        coolDownManager = GetComponent<P_CoolDownManager>();
        groundSlide = GetComponent<P_GroundSlide>();
        movement = GetComponent<C_Movement>();
        bladeDance = GetComponent<A_BladeDance>();
        airDash = GetComponent<A_AirDash>();
        swordThrow = GetComponent<A_SwordThrow>();
        playerAttack = GetComponent<PlayerAttack>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        GetUserInputNonPhysics();
    }

    private void FixedUpdate()
    {
        GetUserInputPhysics();
    }

    void GetUserInputNonPhysics()
    {
        // ========================================
        // ==========OVERCHARGE ABILITIES==========
        // ========================================
        if (Input.GetKeyDown(KeyCode.Alpha1)) 
        { 
            if(bladeDance.Ability_BladeDance()) 
                coolDownManager.AddCooldownToList(bladeDance); 
        }
        if(Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKey(KeyCode.JoystickButton4) && !swordThrow.stuck)
        {
            swordThrow.ThrowSword();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKey(KeyCode.JoystickButton4) && swordThrow.stuck)
        {
            swordThrow.FlyToSword();
        }
        if(Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.JoystickButton2))
        {
            if (airDash.UseAirDash())
                coolDownManager.AddCooldownToList(airDash);
        }
        // =================================
        // ==========PLAYER ATTACK==========
        // =================================
        if(Input.GetMouseButtonDown(0) || Input.GetKey(KeyCode.JoystickButton5)) { playerAttack.BasicAttack(); }
        if (Input.GetMouseButton(1) || Input.GetKey(KeyCode.JoystickButton4)) { playerAttack.BasicDefense(); }
        else { playerAttack.SwordBlockComplete(); }

        // ======================================
        // ==========CHARACTER MOVEMENT==========
        // ======================================
        
        // ==========Jump==========
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0)) { movement.Jump(); }
        // ==========Ground Dash==========
        if (Input.GetKeyDown(KeyCode.LeftShift) && groundSlide.GetSliding()) { groundSlide.UseGroundDash(0.5f); }

        // ====================================
        // ==========MENU / UI THANGS==========
        // ====================================
        if (Input.GetKeyDown(KeyCode.H) || Input.GetKeyDown(KeyCode.JoystickButton6)) { helpScreen.gameObject.SetActive(!helpScreen.gameObject.activeSelf); }
    }

    void GetUserInputPhysics()
    {
        // Check to see if we are going to fast, stops input if we are going to fast
        if(rb.velocity.sqrMagnitude > (movement.maxPlayerSpeedRunning * movement.maxPlayerSpeedRunning)) { return; }
        // ======================================
        // ==========CHARACTER MOVEMENT==========
        // ======================================
        
        // ==========Move Forward==========
        if(Input.GetKey(KeyCode.W) || (Input.GetAxis("controllerUp") < 0)) { movement.MoveForward();}

        // ==========Move Backwards==========
        else if (Input.GetKey(KeyCode.S) || (Input.GetAxis("controllerDown") > 0)) { movement.MoveBackwards(); }
        else
        {
            movement.SetMoveForwardFalse();
            movement.SetMoveBackwardsFalse();
        }
        // ==========Strafe Right==========
        if(Input.GetKey(KeyCode.D) || (Input.GetAxis("controllerRight") > 0)) { movement.StrafeCharacter(1); }
        // ==========Strafe Left==========
        else if (Input.GetKey(KeyCode.A) || (Input.GetAxis("controllerLeft") < 0)) { movement.StrafeCharacter(-1); }
        else
        {
            movement.SetMoveSidetoSideFalse();
        }
    }
}
