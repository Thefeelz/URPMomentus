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
    Animator myAnim;
    PauseMenu pauseMenu;
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
        pauseMenu = FindObjectOfType<PauseMenu>();
        myAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        GetUserInputNonPhysics();
        //Physics.Linecast(transform.position + transform.forward, transform.position + transform.forward * 2, out hit);
        //Debug.DrawLine(new Vector3(transform.position.x + transform.forward.x, transform.position.y, transform.position.z + transform.forward.z), new Vector3((transform.position.x + transform.forward.x * 2f) , transform.position.y, (transform.position.z + transform.forward.z * 2f)), Color.cyan, 1f);
    }

    private void FixedUpdate()
    {
        if(!PauseMenu.GameIsPaused)
            GetUserInputPhysics();
    }

    void GetUserInputNonPhysics()
    {
        if (!PauseMenu.GameIsPaused)
        {
            // ========================================
            // ==========OVERCHARGE ABILITIES==========
            // ========================================
            if (Input.GetKeyDown(KeyCode.Alpha1) && bladeDance.enabled)
            {
                if (bladeDance.Ability_BladeDance())
                    coolDownManager.AddCooldownToList(bladeDance);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKey(KeyCode.JoystickButton4) && !swordThrow.stuck && swordThrow.enabled)
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
            {
                if (airDash.UseAirDash())
                    coolDownManager.AddCooldownToList(airDash);
            }
            // =================================
            // ==========PLAYER ATTACK==========
            // =================================
            if (Input.GetMouseButtonDown(0) || Input.GetKey(KeyCode.JoystickButton5) && playerAttack.enabled) { playerAttack.BasicAttack(); }
            if (Input.GetMouseButton(1) || Input.GetKey(KeyCode.JoystickButton4) && playerAttack.enabled) { playerAttack.BasicDefense(); }
            else { playerAttack.SwordBlockComplete(); }

            // ======================================
            // ==========CHARACTER MOVEMENT==========
            // ======================================

            // ==========Jump==========
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0)) { movement.Jump(); } // @Isaac added an or statement for looking for the a button
                                                                                                                   // ==========Ground Dash==========
            if (Input.GetKeyDown(KeyCode.LeftShift) && groundSlide.GetSliding() && groundSlide.enabled) { groundSlide.UseGroundDash(0.5f); }

            // ====================================
            // ==========MENU / UI THANGS==========
            // ====================================
            if (Input.GetKeyDown(KeyCode.H) || Input.GetKeyDown(KeyCode.JoystickButton6)) { helpScreen.gameObject.SetActive(!helpScreen.gameObject.activeSelf); } // @Isaac added an or statement for looking for the select button
        }
        //if(Input.GetKeyDown(KeyCode.Escape)) { pauseMenu.PauseGame(); }
    }

    void GetUserInputPhysics()
    {
        // ======================================
        // ==========CHARACTER MOVEMENT==========
        // ======================================
        if (!movement.enabled) { return; }
        Vector3 charMovementVector = Vector3.zero;
        // ==========Move Forward==========
        if(Input.GetKey(KeyCode.W) || (Input.GetAxis("controllerUp") < 0)) { charMovementVector += transform.forward;}

        // ==========Move Backwards==========
        else if (Input.GetKey(KeyCode.S) || (Input.GetAxis("controllerDown") > 0)) { charMovementVector -= transform.forward * 0.5f; }
        else
        {
            movement.SetMoveForwardFalse();
            movement.SetMoveBackwardsFalse();
        }
        // ==========Strafe Right==========
        if(Input.GetKey(KeyCode.D) || (Input.GetAxis("controllerRight") > 0)) { charMovementVector += transform.right; }
        // ==========Strafe Left==========
        else if (Input.GetKey(KeyCode.A) || (Input.GetAxis("controllerLeft") < 0)) { charMovementVector -= transform.right; }
        else
        {
            movement.SetMoveSidetoSideFalse();
        }
        if(charMovementVector == Vector3.zero) 
        {
            myAnim.SetBool("running", false);
            return; 
        }
        myAnim.SetBool("running", true);
        movement.HandleMovement(charMovementVector.normalized);
    }
}
