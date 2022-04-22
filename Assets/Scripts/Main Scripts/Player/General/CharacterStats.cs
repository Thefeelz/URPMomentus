using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] float playerMaxHealth = 100;
    [SerializeField] float playerCurrentHealth;
    [SerializeField] float playerMaxOvercharge = 100;
    [SerializeField] float playerCurrentOvercharge = 0;

    [SerializeField] int playerAttack = 10;
    [SerializeField] int playerDefense = 10;
    [SerializeField] Animator canvasAnimator;

    Animator playerAnim;
    PlayerUI ui;
    GameManager gameManager;

    //Damage Audio
    public FMODUnity.EventReference damageAudioRef;


    void Start()
    {
        playerAnim = GetComponent<Animator>();
        ui = GetComponent<PlayerUI>();
        playerCurrentHealth = playerMaxHealth;
        gameManager = FindObjectOfType<GameManager>();
        if (!gameManager)
            Debug.LogWarning("There is NO GameManager in the Scene, make sure to Add One");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// Stupid Ass name that I used to "Replenish Health", I am sorry for the name of my function
    /// </summary>
    /// <param name="amountToRestore"></param>
    public void ReplenishHealth(float amountToRestore)
    {
        float difference = 0f;
        // Check to see if health is full with the restore
        if(playerCurrentHealth + amountToRestore >= playerMaxHealth)
        {
            difference = playerCurrentHealth + amountToRestore - playerMaxHealth;
            // Set Player Current Health to Max Health
            playerCurrentHealth = playerMaxHealth;

            // Check to see if Overcharge wil be full
            if((playerCurrentOvercharge + difference) >= playerMaxOvercharge)
            {
                // Set overcharge to max amount
                playerCurrentOvercharge = playerMaxOvercharge;
                GetComponent<PlayerUI>().UpdateUIColors();
                return;
            }
            else /// Player Overcharge will not overfill
            {
                playerCurrentOvercharge += difference;
                GetComponent<PlayerUI>().UpdateUIColors();
                return;
            }
        } 
        else
        {
            playerCurrentHealth += amountToRestore;
            GetComponent<PlayerUI>().UpdateUIColors();
        }
    }

    /// <summary>
    /// Remove Health from player, damage is from an enemy that is melee or using melee attacks at the time, no Overcharge, if you want to remove Overcharge, use 'A_RemoveHealth()'
    /// Melee attacks CANNOT be deflected by the shield
    /// </summary>
    /// <param name="amountToRemove"></param>
    public void RemoveHealthMelee(float amountToRemove)
    {
        FMODUnity.RuntimeManager.PlayOneShot(damageAudioRef, transform.position);
        // if (playerAnim.GetBool("swordBlock")) { return; }
        ui.UpdateDamageOverlayOnDamageTaken();
        if (amountToRemove >= playerCurrentHealth)
        {
            Die();
            return;
        }
        playerCurrentHealth -= amountToRemove;
        GetComponent<PlayerUI>().UpdateUIColors();
    }

    /// <summary>
    /// Remove Health from player, damage is from an enemy that is ranged, no Overcharge, if you want to remove Overcharge, use 'A_RemoveHealth()'
    /// Ranged Attacks CAN be deflected by the shield
    /// </summary>
    /// <param name="amountToRemove"></param>
    public void RemoveHealthRanged(float amountToRemove)
    {
        if (playerAnim.GetBool("swordBlock")) { return; }

        FMODUnity.RuntimeManager.PlayOneShot(damageAudioRef, transform.position);
        ui.UpdateDamageOverlayOnDamageTaken();
        if (amountToRemove >= playerCurrentHealth)
        {
            Die();
            return;
        }
        playerCurrentHealth -= amountToRemove;
        GetComponent<PlayerUI>().UpdateUIColors();
    }

    /// <summary>
    /// Remove Health AND Overcharge, if you want to remove Health only, use 'RemoveHealth()'
    /// </summary>
    /// <param name="amountToRemove"></param>
    public void A_RemoveHealth(float amountToRemove)
    {
        float difference = 0f;
        if (playerCurrentOvercharge > 0)
        {
            if (playerCurrentOvercharge - amountToRemove > 0)
            {
                playerCurrentOvercharge -= amountToRemove;
            }
            else
            {
                difference = playerCurrentOvercharge - amountToRemove;
                playerCurrentOvercharge = 0;
                playerCurrentHealth += difference;
                GetComponent<PlayerUI>().UpdateUIColors();
            }
        }
        else
        {
            playerCurrentHealth -= amountToRemove;
            if (playerCurrentHealth <= 0)
            {
                Die();
            }
            GetComponent<PlayerUI>().UpdateUIColors();
        }
    }
    public void SetPlayerHealth(float newPlayerHealth)
    {
        GetComponent<PlayerUI>().UpdateUIColors();
        playerCurrentHealth = newPlayerHealth;
    }
    public float GetPlayerHealth()
    {
        return playerCurrentHealth;
    }
    public void SetPlayerOvercharge(float newPlayerOvercharge)
    {
        GetComponent<PlayerUI>().UpdateUIColors();
        playerCurrentOvercharge = newPlayerOvercharge;
    }
    public float GetPlayerOvercharge()
    {
        return playerCurrentOvercharge;
    }
    public void SetPlayerMaxHealth(float newPlayerMaxHealth)
    {
        playerMaxHealth = newPlayerMaxHealth;
    }
    public float GetPlayerMaxHealth()
    {
        return playerMaxHealth;
    }
    public void SetPlayerMaxOvercharge(float newPlayerMaxOvercharge)
    {
        playerMaxOvercharge = newPlayerMaxOvercharge;
    }
    public float GetPlayerMaxOvercharge()
    {
        return playerMaxOvercharge;
    }
    public void SetPlayerStrength(int newPlayerAttack)
    {
        playerAttack = newPlayerAttack;
    }
    public int GetPlayerAttack()
    {
        return playerAttack;
    }

    private void Die()
    {
        GetComponent<P_Input>().enabled = false;
        playerCurrentHealth = 0;
        GetComponent<PlayerUI>().UpdateUIColors();
        canvasAnimator.SetBool("dead", true);
        gameManager.PlayerDead();
    }

    IEnumerator BackToMainScreen()
    {
        yield return new WaitForSeconds(16f);
        FindObjectOfType<SceneController>().MainMenu();
    }
}
