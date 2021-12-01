using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustMovementStats : MonoBehaviour
{
    [SerializeField] bool playerSpeed, playerJump, playerInAirControl, fallMultiplier, onEnter, onExit, onStay;
    [SerializeField] float playerSpeedValue, playerJumpValue, fallMultiplierValue;
    [Range(0, 1)] [SerializeField] float playerInAirControlValue;
    P_Movement playerMoves;
    // Start is called before the first frame update
    void Start()
    {
        playerMoves = FindObjectOfType<P_Movement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(onEnter && other.GetComponentInParent<P_Input>())
        {
            ChangeValues();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (onStay && other.GetComponentInParent<P_Input>())
        {
            ChangeValues();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (onExit && other.GetComponentInParent<P_Input>())
        {
            ChangeValues();
        }
    }
    void ChangeValues()
    {
        if (playerSpeed) { playerMoves.SetPlayerSpeed(playerSpeedValue); }
        if (playerJump) { playerMoves.SetPlayerJump(playerJumpValue); }
        if (playerInAirControl) { playerMoves.SetPlayerInAirControl(playerInAirControlValue); }
        if (fallMultiplier) { playerMoves.SetFallMultiplier(fallMultiplierValue); }
    }
}
