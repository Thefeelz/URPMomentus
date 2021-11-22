using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Input : MonoBehaviour
{
    [SerializeField] Canvas helpScreen;
    P_CoolDownManager coolDownManager;
    P_GroundSlide groundSlide;
    P_Movement movement;
    PlayerAttack playerAttack;

    A_BladeDance bladeDance;
    A_AirDash airDash;
    A_SwordThrow swordThrow;
    A_ContainedHeat containedHeat;
    A_SwordSlash swordSlash;

    Rigidbody rb;
    // Start is called before the first frame update
    void Awake()
    {
        coolDownManager = GetComponent<P_CoolDownManager>();
        groundSlide = GetComponent<P_GroundSlide>();
        movement = GetComponent<P_Movement>();
        bladeDance = GetComponent<A_BladeDance>();
        airDash = GetComponent<A_AirDash>();
        swordThrow = GetComponent<A_SwordThrow>();
        playerAttack = GetComponent<PlayerAttack>();
        containedHeat = GetComponent<A_ContainedHeat>();
        swordSlash = GetComponent<A_SwordSlash>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        GetUserInputNonPhysics();
        //Physics.Linecast(transform.position + transform.forward, transform.position + transform.forward * 2, out hit);
        Debug.DrawLine(new Vector3(transform.position.x + transform.forward.x, transform.position.y, transform.position.z + transform.forward.z), new Vector3((transform.position.x + transform.forward.x * 2f) , transform.position.y, (transform.position.z + transform.forward.z * 2f)), Color.cyan, 1f);
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
        if (Input.GetKeyDown(KeyCode.Alpha1) && bladeDance.enabled) 
        { 
            if(bladeDance.Ability_BladeDance()) 
                coolDownManager.AddCooldownToList(bladeDance); 
        }
<<<<<<< HEAD
        if(Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKey(KeyCode.JoystickButton4) && !swordThrow.stuck) // @Isaac added an or statement for looking for the y button
        {
            swordThrow.ThrowSword();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKey(KeyCode.JoystickButton3) && swordThrow.stuck)// @Isaac added an or statement for looking for the y button
        {
            swordThrow.FlyToSword();
        }
        if(Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.JoystickButton2))// @Isaac added an or statement for looking for the x button
=======
        if(Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKey(KeyCode.JoystickButton4) && !swordThrow.stuck && swordThrow.enabled)
        {
            swordThrow.ThrowSword();
        }
        if ((Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKey(KeyCode.JoystickButton4)) && swordThrow.stuck && swordThrow.enabled)
        {
            swordThrow.FlyToSword();
        }
        if ((Input.GetKeyDown(KeyCode.Alpha3)) && containedHeat.enabled)
        {
            if (containedHeat.Ability_ContainedHeat())
                coolDownManager.AddCooldownToList(containedHeat);
        }
        if ((Input.GetKeyDown(KeyCode.Q)) && swordSlash.enabled)
        {
            if (swordSlash.Ability_SwordSlash())
                coolDownManager.AddCooldownToList(swordSlash);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.JoystickButton2) && airDash.enabled)
>>>>>>> f7396486674a7cc8df352bee940c7ef18a7b0524
        {
            if (airDash.UseAirDash())
                coolDownManager.AddCooldownToList(airDash);
        }
        // =================================
        // ==========PLAYER ATTACK==========
        // =================================
<<<<<<< HEAD
        if(Input.GetMouseButtonDown(0) || Input.GetKey(KeyCode.JoystickButton5)) { playerAttack.BasicAttack(); } // @Isaac added an or statement for looking for the shoulder button
        if (Input.GetMouseButton(1) || Input.GetKey(KeyCode.JoystickButton4)) { playerAttack.BasicDefense(); }// @Isaac added an or statement for looking for the shoulder button
=======
        if(Input.GetMouseButtonDown(0) || Input.GetKey(KeyCode.JoystickButton5) && playerAttack.enabled) { playerAttack.BasicAttack(); }
        if (Input.GetMouseButton(1) || Input.GetKey(KeyCode.JoystickButton4) && playerAttack.enabled) { playerAttack.BasicDefense(); }
>>>>>>> f7396486674a7cc8df352bee940c7ef18a7b0524
        else { playerAttack.SwordBlockComplete(); }

        // ======================================
        // ==========CHARACTER MOVEMENT==========
        // ======================================
        
        // ==========Jump==========
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0)) { movement.Jump(); } // @Isaac added an or statement for looking for the a button
        // ==========Ground Dash==========
        if (Input.GetKeyDown(KeyCode.LeftShift) && groundSlide.GetSliding() && groundSlide.enabled) { groundSlide.UseGroundDash(0.5f); }

        // ====================================
        // ==========MENU / UI THANGS==========
        // ====================================
        if (Input.GetKeyDown(KeyCode.H) || Input.GetKeyDown(KeyCode.JoystickButton6)) { helpScreen.gameObject.SetActive(!helpScreen.gameObject.activeSelf); } // @Isaac added an or statement for looking for the select button
    }

    void GetUserInputPhysics()
    {
        // ======================================
        // ==========CHARACTER MOVEMENT==========
        // ======================================
        if (!movement.enabled) { return; }
        Vector3 charMovementVector = Vector3.zero;
        // ==========Move Forward==========
<<<<<<< HEAD
        if(Input.GetKey(KeyCode.W) || (Input.GetAxis("controllerUp") < 0)) { movement.MoveForward(); } // @Isaac added an or statement for looking for the left joystick up input

        // ==========Move Backwards==========
        else if (Input.GetKey(KeyCode.S) || (Input.GetAxis("controllerDown") > 0)) { movement.MoveBackwards(); } // @Isaac added an or statement for looking for the left joystick downn input
=======
        if(Input.GetKey(KeyCode.W) || (Input.GetAxis("controllerUp") < 0)) { charMovementVector += transform.forward;}

        // ==========Move Backwards==========
        else if (Input.GetKey(KeyCode.S) || (Input.GetAxis("controllerDown") > 0)) { charMovementVector -= transform.forward * 0.5f; }
>>>>>>> f7396486674a7cc8df352bee940c7ef18a7b0524
        else
        {
            movement.SetMoveForwardFalse();
            movement.SetMoveBackwardsFalse();
        }
        // ==========Strafe Right==========
<<<<<<< HEAD
        if(Input.GetKey(KeyCode.D) || (Input.GetAxis("controllerRight") > 0)) { movement.StrafeCharacter(1); } // @Isaac added an or statement for looking for the left joystick right input
        // ==========Strafe Left==========
        else if (Input.GetKey(KeyCode.A) || (Input.GetAxis("controllerLeft") < 0)) { movement.StrafeCharacter(-1); }// @Isaac added an or statement for looking for the left joystic left input
=======
        if(Input.GetKey(KeyCode.D) || (Input.GetAxis("controllerRight") > 0)) { charMovementVector += transform.right; }
        // ==========Strafe Left==========
        else if (Input.GetKey(KeyCode.A) || (Input.GetAxis("controllerLeft") < 0)) { charMovementVector -= transform.right; }
>>>>>>> f7396486674a7cc8df352bee940c7ef18a7b0524
        else
        {
            movement.SetMoveSidetoSideFalse();
        }
        if(charMovementVector == Vector3.zero) { return; }
        movement.HandleMovement(charMovementVector.normalized);
    }
}
