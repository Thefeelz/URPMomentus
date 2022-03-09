using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Shield : A_OverchargeAbilities
{
    bool shielded = false;
    [SerializeField] Shield shield;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (shielded)
            player.A_RemoveHealth(overchargeCost * Time.deltaTime);
    }

    public void ActivateShield()
    {
        if(player.GetPlayerOvercharge() <= 0) 
        {
            if(shielded)
                DeactivateShield();
            return; 
        } 
        shielded = true;
        shield.ActivateCollider();
        playerAnimator.SetBool("swordBlock", true);
    }

    public void DeactivateShield()
    {
        shielded = false;
        shield.DeactivateCollider();
        playerAnimator.SetBool("swordBlock", false);
    }

    public bool GetShielded() { return shielded; }
}
