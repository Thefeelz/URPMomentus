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
}
