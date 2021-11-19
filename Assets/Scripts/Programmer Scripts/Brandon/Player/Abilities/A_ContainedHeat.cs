using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_ContainedHeat : A_OverchargeAbilities
{
    [SerializeField] GameObject containedHeatPrefab;
    [SerializeField] ParticleSystem containedHeatParticleEffect;
    [SerializeField] float containedHeatDuration;
    [SerializeField] float containedHeatEndingRadius;
    [SerializeField] float containedHeatSpeed;

    bool expand = false;
    GameObject instantiatedContainedHeat;
    Material myMat;

    // Start is called before the first frame update
    void Start()
    {
        myMat = containedHeatPrefab.GetComponent<MeshRenderer>().sharedMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        if (expand)
            Expand();
        if (abilityCooldownCurrent > 0)
            ui.UpdateContainedHeatFill((abilityCooldownMax - abilityCooldownCurrent) / abilityCooldownMax);
    }

    public bool Ability_ContainedHeat()
    {
        if (!abilityReady) { return false; }
        mouseLook.enabled = false;
        playerMovement.enabled = false;
        playerAnimator.SetBool("groundAttack", true);
        return true;
    }

    public void StartContainedHeat()
    {
        playerAnimator.SetBool("groundAttack", false);
        expand = true;
        instantiatedContainedHeat = Instantiate(containedHeatPrefab, transform.position, Quaternion.identity);
        ParticleSystem particleThing = Instantiate(containedHeatParticleEffect, transform.position, transform.rotation);
        Destroy(particleThing.transform.gameObject, 2f);
        Destroy(instantiatedContainedHeat, containedHeatDuration);
    }
    void Expand()
    {
        if (instantiatedContainedHeat.transform.localScale.x < containedHeatEndingRadius)
        {
            instantiatedContainedHeat.transform.localScale += instantiatedContainedHeat.transform.localScale * Time.deltaTime * containedHeatSpeed;
            instantiatedContainedHeat.transform.eulerAngles += new Vector3(1, 1, 1);
            
        }
        else
        {
            expand = false;
            mouseLook.enabled = true;
            playerMovement.enabled = true;
            playerAnimator.SetBool("groundAttack", false);
        }
    }
}
