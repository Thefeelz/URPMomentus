using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class A_AirDash : A_OverchargeAbilities
{
    // Last Edit: Brandon Hinz 10/26/21
    // Player Ground Slide Class

    // ==========Variables to change in the inspector==========

    // The Distance our player will dash
    [SerializeField] float slideDistance = 10f;
    // The effect that is on the screen (Multiple will be placed in the scene, drag and drop which one we want per effect)
    [SerializeField] Volume postProcessingEffects;
    // This is the return to Normal screen time from the Post Processing Effect
    [SerializeField] float returnToNormalScreenTime = 0.5f;
    // The duration it takes for the player to finish the dash (Literally is the movement speed of the player)
    [SerializeField] float dashDuration = 0.5f;
    //Air dash noise reference
    [SerializeField] FMODUnity.EventReference AirDashRef;

    // ==========Variables to Cache in the Awake Function==========

    // A call to our rigid body to effect it
    Rigidbody rb;

    // ==========Private Variables to be used in the code ==========

    // Bool for the transition to the normal screen (Lerps off the Post Processing Effect)
    bool returnToNormalScreen = false;
    // Bool to start the actual slide in the 'Update' Function
    bool dashing = false;
    // Elapsed time for the slide (Used for the Lerp Function)
    float elapsedTime = 0f;
    // Elapsed time for the removal of the Post Processing Effect (Used in a Lerp Function)
    float returnToNormalScreenElapsedTime = 0f;
    // Field Of View The Player Wants
    float fovCurrent = 60f;

    // Variable to store the starting position of the player
    Vector3 startingPos;
    // Variable to store the ending position of the player
    Vector3 endingPos;

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        // Cache our Variables
        playerMovement = GetComponent<P_Movement>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        // This will be true if we are sliding, false if we are not
        if (dashing)
            Dash();
        // If a post processing volume is attached to this ability, this will return the screen smoothly back to normal
        if (returnToNormalScreen)
            ReturnToNormalScreen();

        if (abilityCooldownCurrent > 0)
            ui.UpdateAirDashFill((abilityCooldownMax - abilityCooldownCurrent) / abilityCooldownMax, overchargeCost);
    }

    // =================================================================
    // ========FUNCTION CALLED IN THE P_INPUT.CS TO START Dash==========
    // =================================================================
    public bool UseAirDash()
    {
        if (playerMovement && player.GetPlayerOvercharge() > overchargeCost)
        {
            // Check to make sure the player is grounded
            if (!playerMovement.isGrounded && abilityReady)
            {
                // Sliding is set to true to allow the sliding function to be called in the 'Update' function
                dashing = true;
                // A helper function that will do some checks to make sure we dont bug out in the game
                SetUpDash();
                return true;
            }
        }
        else if (cMovement && player.GetPlayerOvercharge() > overchargeCost)
        {
            // Check to make sure the player is grounded
            if (!cMovement.isGrounded && abilityReady)
            {
                // Sliding is set to true to allow the sliding function to be called in the 'Update' function
                dashing = true;
                // A helper function that will do some checks to make sure we dont bug out in the game
                SetUpDash();
                return true;
            }
        }
        return false;
    }

    void Dash()
    {
        FMODUnity.RuntimeManager.PlayOneShot(AirDashRef, transform.position);
        // While dashing, since we are Lerping we need a constant number to Lerp against
        elapsedTime += Time.deltaTime;
        // Set the lerp progress to a variable so we dont have to calculate it 3 times, just once
        float lerpPos = elapsedTime / dashDuration;
        // Move the rigid body forward by the current lerp amount
        rb.MovePosition(Vector3.Lerp(startingPos, endingPos, lerpPos));
        // Lerp our camera effects as well so they increase to full intensity while we are sliding
        postProcessingEffects.weight = Mathf.Lerp(0, 1, lerpPos);
        Camera.main.fieldOfView = Mathf.Lerp(fovCurrent, 100, lerpPos);
        // If our slide is finished, set everything back to default values and trigger return to normal screen for our camera position and camera effects
        if (elapsedTime >= dashDuration)
        {
            dashing = false;
            elapsedTime = 0;
            returnToNormalScreen = true;
        }
    }

    void SetUpDash()
    {
        // Starting position set to our players current position
        startingPos = transform.position;
        // Ending position set to our position plus the distance forward we determine in the inspector
        float newDistance = CalculateOverChargeDistanceExtra(slideDistance);
        endingPos = transform.position + ((Camera.main.transform.forward * newDistance) - Camera.main.transform.forward);
        CalculateDashDistance(newDistance);
    }

    void CalculateDashDistance(float newDistance)
    {
        // Raycast hit to store our raycast hit information
        RaycastHit hit;
        // A raycast that shoots out from our feet forward relative to where we are facing
        Physics.Raycast(transform.position + transform.up, Camera.main.transform.forward, out hit, newDistance);

        Debug.Log(hit);
        // If the raycast hits nothing, go the full length of the slide and return
        if (hit.collider == null || hit.collider.GetComponentInParent<P_CoolDownManager>()) { return; }
        // If we are at this point, we HIT something with our raycast, see if the raycast hit point distance is less than our slide distance
        if (Vector3.Distance(transform.position, hit.point) < slideDistance)
        {
            Debug.Log("We hit " + hit.collider.name);
            // If it is, set our ending position to where the ray made contact
            endingPos = hit.point - Camera.main.transform.forward;
            endingPos = new Vector3(endingPos.x, transform.position.y, endingPos.z);
        }
    }

    void ReturnToNormalScreen()
    {
        returnToNormalScreenElapsedTime += Time.deltaTime;
        float lerpPos = returnToNormalScreenElapsedTime / returnToNormalScreenTime;
        postProcessingEffects.weight = Mathf.Lerp(1, 0, lerpPos);
        Camera.main.fieldOfView = Mathf.Lerp(100, fovCurrent, lerpPos);
        if (returnToNormalScreenTime <= returnToNormalScreenElapsedTime)
        {
            returnToNormalScreenElapsedTime = 0;
            returnToNormalScreen = false;
        }
    }

    public void UpdateFieldOfViewValue(float value)
    {
        fovCurrent = value;
    }
}
