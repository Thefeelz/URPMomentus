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
    // Start is called before the first frame update
    protected virtual void Awake()
    {
        ui = GetComponent<PlayerUI>();
        player = GetComponent<CharacterStats>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    public void StartAbilityCooldown()
    {
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
