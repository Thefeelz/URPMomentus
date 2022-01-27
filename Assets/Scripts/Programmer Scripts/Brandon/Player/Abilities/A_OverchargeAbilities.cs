using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class A_OverchargeAbilities : MonoBehaviour
{
    [SerializeField] protected int overchargeCost;
    [SerializeField] protected float abilityCooldownMax;
    [SerializeField] protected Image abilityCooldownButton;
    public float abilityCooldownCurrent = 0f;
    public bool abilityReady = true;
    protected PlayerUI ui;
    protected CharacterStats player;
    protected Animator playerAnimator;
    protected mouseLook mouseLook;
    protected P_Movement playerMovement;
    // Start is called before the first frame update
    protected virtual void Awake()
    {
        ui = GetComponent<PlayerUI>();
        player = GetComponent<CharacterStats>();
        playerAnimator = GetComponent<Animator>();
        mouseLook = GetComponent<mouseLook>();
        playerMovement = GetComponent<P_Movement>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    public void StartAbilityCooldown()
    {
        player.RemoveHealth(overchargeCost);
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
}
