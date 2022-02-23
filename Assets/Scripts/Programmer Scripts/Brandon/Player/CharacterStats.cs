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

    void Start()
    {
        playerCurrentHealth = playerMaxHealth;
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
                return;
            }
            else /// Player Overcharge will not overfill
            {
                playerCurrentOvercharge += difference;
                return;
            }
        } 
        else
        {
            playerCurrentHealth += amountToRestore;
        }
    }

    /// <summary>
    /// Remove Health, not Overcharge, if you want to remove Overcharge, use 'A_RemoveHealth()'
    /// </summary>
    /// <param name="amountToRemove"></param>
    public void RemoveHealth(float amountToRemove)
    {
        if(amountToRemove > playerCurrentHealth)
        {
            Die();
            return;
        }
        playerCurrentHealth -= amountToRemove;
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
            }
        }
        else
        {
            playerCurrentHealth -= amountToRemove;
            if (playerCurrentHealth <= 0)
            {
                Die();
            }
        }
    }
    public void SetPlayerHealth(float newPlayerHealth)
    {
        playerCurrentHealth = newPlayerHealth;
    }
    public float GetPlayerHealth()
    {
        return playerCurrentHealth;
    }
    public void SetPlayerOvercharge(float newPlayerOvercharge)
    {
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
        canvasAnimator.SetBool("dead", true);
        StartCoroutine(BackToMainScreen());
    }

    IEnumerator BackToMainScreen()
    {
        yield return new WaitForSeconds(6f);
        SceneManager.LoadScene(0);
    }
}
