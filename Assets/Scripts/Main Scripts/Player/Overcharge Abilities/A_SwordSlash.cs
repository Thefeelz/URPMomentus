using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_SwordSlash : A_OverchargeAbilities
{
    // The lightning material
    [SerializeField] GameObject swordSlashPrefab;

    // Fire location, simplifies math
    [SerializeField] Transform weaponFire;

    //  This is used to Increase the amount of electricity being generated by the sword
    ParticleSystem.EmissionModule thisSystem;

    // Material of the Swords and Hands to amplify the glow during the attack
    [SerializeField] Material[] emissionMaterial;

    // Enemies able to be hit till the ability dissapears
    [SerializeField] int enemiesAbleToHit;
    
    // This is the effect of the sword that is passivley on during the game, when this ability is used we have to turn off the base one and turn it back on
    // that is its sole purpose of being here
    SwordLightningEffect swordEffect;

    bool rampUp = false;
    bool dieDown = false;

    float duration = 0.5f;
    float elapsedTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        swordEffect = GetComponentInChildren<SwordLightningEffect>();
        thisSystem = GetComponentInChildren<ParticleSystem>().emission;
        swordSlashPrefab = GameManager.Instance.GetSwordSlashPrefab();
    }

    // Update is called once per frame
    void Update()
    {
        // Start Sword Glowy
        if (rampUp)
            RampUpEffects();
        // Stop Sword Glowy
        if (dieDown)
            DieDownEffects();
        // Update the UI
        if (abilityCooldownCurrent > 0)
            ui.UpdateSwordSlashFill((abilityCooldownMax - abilityCooldownCurrent) / abilityCooldownMax);
    }

    // Start the sword animation and effect, called by the animation
    public void StartSwordSlashEffect()
    {
        playerAnimator.SetBool("swordSpecial", false);
        swordEffect.SwordSlashControlsLightningAndGlow(true);
        rampUp = true;
        duration = 0.5f;
    }

    // Stop the animation and ramp it down, called by the animation
    public void StopSwordSlashEffect()
    {
        rampUp = false;
        dieDown = true;
        elapsedTime = 0;
        duration = 1.5f;
    }
    // Called by the Animation
    public void FireSwordSlash()
    {
        GameObject firedThing = Instantiate(swordSlashPrefab, weaponFire.position, Quaternion.Euler(transform.forward));
        
        firedThing.GetComponentInChildren<SwordSlashEffect>().SetVelocity(Camera.main.transform.forward, 10f);
        firedThing.GetComponentInChildren<SwordSlashEffect>().SetEnemiesAbleToHit(enemiesAbleToHit);
    }

    // Starts glowing the sword and setting the particle effect on the sword to be more intense
    void RampUpEffects()
    {
        elapsedTime += Time.deltaTime;
        thisSystem.rateOverTime = Mathf.Lerp(0, 50, elapsedTime / duration);
        for (int i = 0; i < emissionMaterial.Length; i++)
        {
            emissionMaterial[i].SetColor("_EmissionColor", Color.white * Mathf.Lerp(1, 10, elapsedTime / duration));
        }
    }

    // Starts to kill the glow on the sword and setting particle effects to go back to normal
    void DieDownEffects()
    {
        elapsedTime += Time.deltaTime;
        thisSystem.rateOverTime = Mathf.Lerp(50, 0, elapsedTime / duration);
        for (int i = 0; i < emissionMaterial.Length; i++)
        {
            emissionMaterial[i].SetColor("_EmissionColor", Color.white * Mathf.Lerp(10.2f, 1, elapsedTime / duration));
        }
        if(elapsedTime >= duration)
        {
            dieDown = false;
            swordEffect.SwordSlashControlsLightningAndGlow(false);
        }
    }

    // Called by the input
    public bool Ability_SwordSlash()
    {
        if(abilityReady)
        {
            playerAnimator.SetBool("swordSpecial", true);
            return true;
        }
        return false;
    }
}
