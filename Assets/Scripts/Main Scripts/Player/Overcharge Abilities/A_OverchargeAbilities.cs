using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class A_OverchargeAbilities : MonoBehaviour
{
    [SerializeField] protected int overchargeCost;
    [SerializeField] protected float abilityCooldownMax;
    [SerializeField] protected Image abilityCooldownButton;
    [SerializeField] protected TMP_Text debugImage;
    public float abilityCooldownCurrent = 0f;
    public bool abilityReady = true;
    protected PlayerUI ui;
    protected CharacterStats player;
    protected Animator playerAnimator;
    protected mouseLook mouseLook;
    protected P_Movement playerMovement;
    protected C_Movement cMovement;
    // Start is called before the first frame update
    protected virtual void Awake()
    {
        ui = GetComponent<PlayerUI>();
        player = GetComponent<CharacterStats>();
        playerAnimator = GetComponent<Animator>();
        mouseLook = GetComponent<mouseLook>();
        playerMovement = GetComponent<P_Movement>();
        cMovement = GetComponent<C_Movement>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    public void StartAbilityCooldown()
    {
        player.A_RemoveHealth(overchargeCost);
        abilityReady = false;
        abilityCooldownCurrent = abilityCooldownMax;
    }

    public void DecrementCooldownDuration(float timeToDecrement)
    {
        abilityCooldownCurrent -= timeToDecrement;
    }

    public void StartUIImageFlash()
    {
        if(abilityCooldownButton)
            abilityCooldownButton.GetComponent<UI_AbilityIcon>().ButtonAbilityReadyStart();
    }

    protected void TogglePlayerMovementAndAnimator(bool value)
    {

        if (GetComponent<P_Movement>())
        {
            playerAnimator.enabled = value;
            GetComponent<P_Movement>().enabled = value;
            GetComponent<mouseLook>().enabled = value;
        }
        else if (cMovement)
        {
            playerAnimator.enabled = value;
            GetComponent<C_Movement>().enabled = value;
            GetComponent<XboxLook>().enabled = value;
        }
        if (!value)
            playerMovement.SetPlayerCurrentSpeed(0);
    }

    protected void TogglePlayerMovement(bool value)
    {
        if (GetComponent<P_Movement>())
        {
            // GetComponent<P_Movement>().enabled = value;
            GetComponent<P_Input>().SetFreezeMovement(!value);
        }
        else if (cMovement)
        {
            // GetComponent<C_Movement>().enabled = value;
            GetComponent<P_Input>().SetFreezeMovement(value); ;
        }
        if (!value)
            playerMovement.SetPlayerCurrentSpeed(0);
    }

    protected void TogglePlayerMovmentAndMouseLook(bool value)
    {
        if (GetComponent<P_Movement>())
        {
            GetComponent<P_Movement>().enabled = value;
            GetComponent<mouseLook>().enabled = value;
        }
        else if (cMovement)
        {
            GetComponent<C_Movement>().enabled = value;
            GetComponent<XboxLook>().enabled = value;
        }
        if (!value)
            playerMovement.SetPlayerCurrentSpeed(0);
    }
    public int GetOverchargeCost() { return overchargeCost; }

    protected float CalculateOverChargeDistanceExtra(float baseDistance)
    {
        float oc = player.GetPlayerOvercharge();
        if (oc > 75f)
            return baseDistance * 1.5f;
        if (oc > 50f)
            return baseDistance * 1.2f;
        if (oc > 25f)
            return baseDistance * 1.1f;
        return baseDistance;
    }

    protected int CalculateEnemiesHit(int maxEnemiesHit)
    {
        float oc = player.GetPlayerOvercharge();
        if (oc > 75)
            return maxEnemiesHit;
        if (oc > 50)
            return Mathf.RoundToInt(maxEnemiesHit * 0.75f);
        if (oc > 25)
            return Mathf.RoundToInt(maxEnemiesHit * 0.5f);
        return 1;
    }

    protected void DisplayDebugImage()
    {
        if(!debugImage) { return; }
        debugImage.gameObject.SetActive(true);
        StartCoroutine(TurnOff());
    }

    IEnumerator TurnOff()
    {
        yield return new WaitForSeconds(1f);
        debugImage.gameObject.SetActive(false);
    }
}
