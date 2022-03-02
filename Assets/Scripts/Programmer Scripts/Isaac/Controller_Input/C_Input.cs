using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ==== Controller version of Brandon's Code edited by Isaac Schulz =====

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
    A_ContainedHeat containedHeat;
    A_SwordSlash swordSlash;

    Rigidbody rb;
    Animator myAnim;
    PauseMenu pauseMenu;
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
        containedHeat = GetComponent<A_ContainedHeat>();
        swordSlash = GetComponent<A_SwordSlash>();
        rb = GetComponent<Rigidbody>();
        pauseMenu = FindObjectOfType<PauseMenu>();
        myAnim = GetComponent<Animator>();
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
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKey(KeyCode.JoystickButton3) && bladeDance.enabled) 
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
        if ((Input.GetKeyDown(KeyCode.Alpha3)) || ( Input.GetKey(KeyCode.JoystickButton1)) && containedHeat.enabled)
        {
            if (containedHeat.Ability_ContainedHeat())
                coolDownManager.AddCooldownToList(containedHeat);
        }
        if ((Input.GetKeyDown(KeyCode.Q)) || Input.GetKey(KeyCode.JoystickButton3) && swordSlash.enabled)
        {
            if (swordSlash.Ability_SwordSlash())
                coolDownManager.AddCooldownToList(swordSlash);
                Debug.Log("Either Q or Y was pressed");
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.JoystickButton2))
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
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.JoystickButton2) && groundSlide.GetSliding()) { groundSlide.UseGroundDash(0.5f); }

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
