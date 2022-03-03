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

    [SerializeField] GameObject[] objectsToTurnOnWhenDead;
    
    bool triggeredDead = false;

    protected EnemyChaseState chase;
    CharacterStats player;
    GameManager gameManager;
    Entity mEntity;

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<Entity>())
        {
            mEntity = gameObject.GetComponent<Entity>();
            Debug.Log("entity found");
        }
        gameManager = FindObjectOfType<GameManager>();
        gameManager.AddEnemyToList(this);
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
        else if (GetComponent<Entity>())
            mEntity.Damage(damageToTake);
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
        player.ReplenishHealth(energyAmount);
        if (GetComponent<EnemyChaseState>())
        {
            if (GetComponent<EnemyChaseState>())
                chase.SetStateToDead();
            if (GetComponentInChildren<Collider>().attachedRigidbody)
            {
                GetComponentInChildren<Collider>().attachedRigidbody.isKinematic = true;
                GetComponentInChildren<Collider>().enabled = false;
            }
            yield return new WaitForSeconds(10f);
            Debug.Log("Destroyed");
            gameManager.RemoveFromActiveList(this);
            Destroy(gameObject);
        }
        else if (GetComponent<Turret>())
        {
            GetComponent<Turret>().TurnOnTurretFire();
            gameManager.RemoveFromActiveList(this);
        }
    }
}