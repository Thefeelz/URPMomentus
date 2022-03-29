using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnAndOffScripts : MonoBehaviour
{
    [SerializeField] bool groundSlide, wallRun, movement, input, airDash, bladeDance, mouselook, swordThrow, swordSlash, containedHeat, playerAttack, onEnter, onExit, onStay, turnOn;
    P_GroundSlide groundSlideAbility;
    P_WallRun wallRunAbility;
    P_Movement movementAbility;
    P_Input inputAbility;
    A_AirDash airDashAbility;
    A_BladeDance bladeDanceAbility;
    A_SwordSlash swordSlashAbility;
    A_ContainedHeat containedHeatAbility;
    mouseLook mouseLookAbility;
    A_SwordThrow swordThrowAbility;
    CharacterStats player;
    PlayerAttack playerAttackAbility;
    

    private void Awake()
    {
        player = FindObjectOfType<CharacterStats>();
        groundSlideAbility = player.GetComponent<P_GroundSlide>();
        wallRunAbility = player.GetComponent<P_WallRun>();
        movementAbility = player.GetComponent<P_Movement>();
        inputAbility = player.GetComponent<P_Input>();
        airDashAbility = player.GetComponent<A_AirDash>();
        bladeDanceAbility = player.GetComponent<A_BladeDance>();
        mouseLookAbility = player.GetComponent<mouseLook>();
        swordThrowAbility = player.GetComponent<A_SwordThrow>();
        swordSlashAbility = player.GetComponent<A_SwordSlash>();
        containedHeatAbility = player.GetComponent<A_ContainedHeat>();
        playerAttackAbility = player.GetComponent<PlayerAttack>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(onEnter)
        {
            if(other.GetComponentInParent<P_Input>())
            {
                if (turnOn)
                    TurnOnScripts();
                else
                    TurnOffScripts();
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (onStay)
        {
            if (other.GetComponentInParent<P_Input>())
            {
                if (turnOn)
                    TurnOnScripts();
                else
                    TurnOffScripts();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (onExit)
        {
            if (other.GetComponentInParent<P_Input>())
            {
                if (turnOn)
                    TurnOnScripts();
                else
                    TurnOffScripts();
            }
        }
    }
    public void TurnOnScripts()
    {
        if (groundSlide) { groundSlideAbility.enabled = true; }
        if (wallRun) { wallRunAbility.enabled = true; }
        if (movement) { movementAbility.enabled = true; }
        if (input) { inputAbility.enabled = true; }
        if (airDash) { airDashAbility.enabled = true; }
        if (bladeDance) { bladeDanceAbility.enabled = true; }
        if (mouselook) { mouseLookAbility.enabled = true; }
        if (swordThrow) { swordThrowAbility.enabled = true; }
        if (playerAttack) { playerAttackAbility.enabled = true; }
        if (swordSlash) { swordSlashAbility.enabled = true; }
        if (containedHeat) { containedHeatAbility.enabled = true; }
    }
    public void TurnOnScripts(AbilityToManipulate enumz)
    {
        if (enumz == AbilityToManipulate.groundSlide) { groundSlideAbility.enabled = true; return; }
        if (enumz == AbilityToManipulate.wallRun) { wallRunAbility.enabled = true; return; }
        if (enumz == AbilityToManipulate.movement) { movementAbility.enabled = true; return; }
        if (enumz == AbilityToManipulate.input) { inputAbility.enabled = true; return; }
        if (enumz == AbilityToManipulate.airDash) { airDashAbility.enabled = true; return; }
        if (enumz == AbilityToManipulate.bladeDance) { bladeDanceAbility.enabled = true; return; }
        if (enumz == AbilityToManipulate.mouselook) { mouseLookAbility.enabled = true; return; }
        if (enumz == AbilityToManipulate.swordThrow) { swordThrowAbility.enabled = true; return; }
        if (enumz == AbilityToManipulate.playerAttack) { playerAttackAbility.enabled = true; return; }
        if (enumz == AbilityToManipulate.swordSlash) { swordSlashAbility.enabled = true; return; }
        if (enumz == AbilityToManipulate.containedHeat) { containedHeatAbility.enabled = true; return; }
    }

    public void TurnOffScripts()
    {
        if (groundSlide) { groundSlideAbility.enabled = false; }
        if (wallRun) { wallRunAbility.enabled = false; }
        if (movement) { movementAbility.enabled = false; }
        if (input) { inputAbility.enabled = false; }
        if (airDash) { airDashAbility.enabled = false; }
        if (bladeDance) { bladeDanceAbility.enabled = false; }
        if (mouselook) { mouseLookAbility.enabled = false; }
        if (swordThrow) { swordThrowAbility.enabled = false; }
        if (playerAttack) { playerAttackAbility.enabled = false; }
        if (swordSlash) { swordSlashAbility.enabled = false; }
        if (containedHeat) { containedHeatAbility.enabled = false; }
    }
    public void TurnOffScripts(AbilityToManipulate enumz)
    {
        if (enumz == AbilityToManipulate.groundSlide) { groundSlideAbility.enabled = false; return; }
        if (enumz == AbilityToManipulate.wallRun) { wallRunAbility.enabled = false; return; }
        if (enumz == AbilityToManipulate.movement) { movementAbility.enabled = false; return; }
        if (enumz == AbilityToManipulate.input) { inputAbility.enabled = false; return; }
        if (enumz == AbilityToManipulate.airDash) { airDashAbility.enabled = false; return; }
        if (enumz == AbilityToManipulate.bladeDance) { bladeDanceAbility.enabled = false; return; }
        if (enumz == AbilityToManipulate.mouselook) { mouseLookAbility.enabled = false; return; }
        if (enumz == AbilityToManipulate.swordThrow) { swordThrowAbility.enabled = false; return; }
        if (enumz == AbilityToManipulate.playerAttack) { playerAttackAbility.enabled = false; return; }
        if (enumz == AbilityToManipulate.swordSlash) { swordSlashAbility.enabled = false; return; }
        if (enumz == AbilityToManipulate.containedHeat) { containedHeatAbility.enabled = false; return; }
    }
    public void ToggleGroundSlide(bool value)
    {
        groundSlideAbility.enabled = value;
    }
    public void ToggleWallRun(bool value)
    {
        wallRunAbility.enabled = value;
    }
    public void ToggleMovement(bool value)
    {
        movementAbility.enabled = value;
    }
    public void ToggleInput(bool value)
    {
        inputAbility.enabled = value;
    }
    public void ToggleAirDash(bool value)
    {
        airDashAbility.enabled = value;
    }
    public void ToggleBladeDance(bool value)
    {
        bladeDanceAbility.enabled = value;
    }
    public void ToggleMouseLook(bool value)
    {
        mouseLookAbility.enabled = value;
    }
    public void ToggleSwordThrow(bool value)
    {
        swordThrowAbility.enabled = value;
    }
    public void ToggleAttack(bool value)
    {
        playerAttackAbility.enabled = value;
    }
    public void ToggleSwordSlash(bool value)
    {
        swordSlashAbility.enabled = value;
    }
    public void ToggleContainedHeat(bool value)
    {
        containedHeatAbility.enabled = value;
    }
}
