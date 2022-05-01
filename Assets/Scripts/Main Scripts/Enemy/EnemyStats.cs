using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStats : MonoBehaviour
{
    //Vincent touched this on 11/5/2021

    [SerializeField] int maxHealth = 10;
    [SerializeField] int currentHealth;
    [SerializeField] int enemyArmor = 5;
    [SerializeField] int energyAmount = 10;
    [SerializeField] Canvas enemyCanvas;
    [SerializeField] Image healthBar;
    [SerializeField] ParticleSystem deathEffect;

    [SerializeField] GameObject[] objectsToTurnOnWhenDead;
    
    bool triggeredDead = false;
    bool ableToBeAttacked = true;

    protected EnemyChaseState chase;
    CharacterStats player;
    GameManager gameManager;
    Entity mEntity;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        if (GetComponent<Entity>())
        {
            mEntity = gameObject.GetComponent<Entity>();
        }
        StartCoroutine(AddEnemyToList());
        // NOTE: This is set to get component in children at the time of its creation, it may change, if there are errors in the future
        // it could be due to the fact that we are looking for the animator in the children if it gets moved elsewhere.

        if (!GetComponent<Entity>())
        {
            chase = GetComponent<EnemyChaseState>();
        }
        currentHealth = maxHealth;
        player = FindObjectOfType<CharacterStats>();
    }

    // Update is called once per frame
    void Update()
    {
        //if(enemyCanvas)
        //    enemyCanvas.transform.LookAt(player.transform);   
    }

    public void TakeDamage(int damageToTake)
    {
        if (!ableToBeAttacked) { return; }
        currentHealth -= damageToTake;
        if(GetComponent<EnemyCaptionSpawner>())
        {
            GetComponent<EnemyCaptionSpawner>().SpawnDamageCaption();
        }
        // healthBar.fillAmount = (float)currentHealth / maxHealth;
        if (currentHealth <= 0 && !triggeredDead && !GetComponent<Entity>())
        {
            triggeredDead = true;
            if (objectsToTurnOnWhenDead.Length > 0)
                TurnOnObjects();
            StartCoroutine(DestroySelf());
        }
        // all Damage does is subtract health
        else if (GetComponent<Entity>() && !triggeredDead)
        {
            currentHealth = 0;
            triggeredDead = true;
            mEntity.Damage(damageToTake);
            StartCoroutine(DestroySelf());
        }
    }

    public void NoahAIAddToActiveList()
    {
        if(gameManager)
            gameManager.AddEnemyToList(this);
        currentHealth = maxHealth;
        triggeredDead = false;
    }
    public int getMaxHealth()
    {
        return maxHealth;
    }
    public int getCurrentHealth()
    {
        return currentHealth;
    }
    public int setCurrentHealth(int health)
    {
        this.currentHealth = health;
        return currentHealth;
    }
    void TurnOnObjects()
    {
        for(int i = 0; i < objectsToTurnOnWhenDead.Length; i++)
        {
            objectsToTurnOnWhenDead[i].SetActive(true);
        }
    }
    IEnumerator DestroySelf()
    {
        if (deathEffect)
        {
            ParticleSystem newObject = Instantiate(deathEffect, transform.position, Quaternion.identity);
            newObject.Play();
            Destroy(newObject.gameObject, 2f);
        }
        else
            Debug.LogError("There is no death effect linked to this prefab, make sure you link some sort of death VFX from the 'Visual Effects Folder'");
        
        player.ReplenishHealth(energyAmount);

        if (GetComponent<EnemyTriggerChild>())
            GetComponent<EnemyTriggerChild>().AlertParentGroupOfDeath();

        if (GetComponent<EnemyChaseState>())
        {
            chase.SetStateToDead();
            if (GetComponentInChildren<Collider>().attachedRigidbody)
            {
                GetComponentInChildren<Collider>().attachedRigidbody.isKinematic = true;
                GetComponentInChildren<Collider>().enabled = false;
            }
            yield return new WaitForSeconds(10f);
            
            gameManager.RemoveFromActiveList(this);
            Destroy(gameObject);
        }
        else if (GetComponent<Turret>())
        {
            GetComponent<Turret>().TurnOnTurretFire();
            yield return new WaitForSeconds(10f);
            gameManager.RemoveFromActiveList(this);
        }
        else if (GetComponent<Entity>())
        {
            yield return new WaitForSeconds(1f);
            gameManager.RemoveFromActiveList(this);
        }
        else if (GetComponent<BomberEnemy>())
        {
            // TODO, VINCENT, yah can put your stuff for you AI here, you can follow the WaitForSeconds which just allows enemies to be visible for "x" amount of seconds
            // after they die.
            GetComponent<BomberEnemy>();
            yield return new WaitForSeconds(0.5f);
            gameManager.RemoveFromActiveList(this);
        }

    }

    public void SetStateToPlayerDead()
    {
        if(GetComponent<Turret>())
        {
            GetComponent<Turret>().SetStateToPlayerDead();
        }
        else if (GetComponent<Entity>())
        {
            // TODO Noah Add your State here for when the player is dead
        }
        else if (GetComponent<BomberEnemy>())
        {
            GetComponent<BomberEnemy>().SetStateToAsleep();
        }

        else if (GetComponent<EnemyChaseState>())
        {
            GetComponent<EnemyChaseState>().SpecialInUse(true);
        }
    }
    IEnumerator AddEnemyToList()
    {
        yield return new WaitForSeconds(0.25f);
        GameManager.Instance.AddEnemyToList(this);
    }

    public void SetAbleToBeAttacked(bool value) { ableToBeAttacked = value; }
    public bool GetAbleToBeAttacked() { return ableToBeAttacked; }
}
