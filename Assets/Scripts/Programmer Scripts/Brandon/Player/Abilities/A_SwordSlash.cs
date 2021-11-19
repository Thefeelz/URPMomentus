using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_SwordSlash : A_OverchargeAbilities
{
    [SerializeField] GameObject swordSlashPrefab;
    [SerializeField] Transform weaponFire;
    ParticleSystem.EmissionModule thisSystem;
    [SerializeField] Material[] emissionMaterial;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (rampUp)
            RampUpEffects();
        if (dieDown)
            DieDownEffects();
        if (abilityCooldownCurrent > 0)
            ui.UpdateSwordSlashFill((abilityCooldownMax - abilityCooldownCurrent) / abilityCooldownMax);
    }
    public void StartSwordSlashEffect()
    {
        playerAnimator.SetBool("swordSpecial", false);
        swordEffect.SwordSlashControlsLightningAndGlow(true);
        rampUp = true;
        duration = 0.5f;
    }
    public void StopSwordSlashEffect()
    {
        rampUp = false;
        dieDown = true;
        elapsedTime = 0;
        duration = 1.5f;
    }
    public void FireSwordSlash()
    {
        GameObject firedThing = Instantiate(swordSlashPrefab, weaponFire.position, Quaternion.Euler(transform.forward));
        firedThing.GetComponentInChildren<SwordSlashEffect>().SetVelocity(transform.forward, 10f);
    }
    void RampUpEffects()
    {
        elapsedTime += Time.deltaTime;
        thisSystem.rateOverTime = Mathf.Lerp(0, 50, elapsedTime / duration);
        for (int i = 0; i < emissionMaterial.Length; i++)
        {
            emissionMaterial[i].SetColor("_EmissionColor", Color.white * Mathf.Lerp(1, 10, elapsedTime / duration));
        }
    }
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
