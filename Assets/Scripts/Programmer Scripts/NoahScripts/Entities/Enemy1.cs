using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy1 : Entity
{
    //The states that Enemy1 can enter
    public E1_Move moveState { get; private set; }
    public E1_SlowApproach slowState { get; private set; }
    public E1_AimingState aimState { get; private set; }
    public E1_Evade evadeState { get; private set; }
    public E1_Knockback knockbackState { get; private set; }

    public DeathState deathState { get; private set; }
    // script for functions and  that involve ranged attacking
    public RangedBehavior rangedBehavior { get; private set; }

    public AmmoPool ammo;
    // a bool for keeping track of when enemy can shoot
    public bool canShoot;
    // bullet prefab
    public GameObject bulletObj;
    // object used as canon
    public GameObject canon;
    // data files for each state
    [SerializeField]
    private D_moveState moveData;
    [SerializeField]
    private D_SlowApproach slowData;
    [SerializeField]
    private D_Aiming aimData;   
    [SerializeField]
    private D_Evade evadeData;    
    [SerializeField]
    private D_Knockback knockbackData;

    public FMODUnity.EventReference bulletRef;
    FMOD.Studio.EventInstance shooting;

    public bool linking;
    public float origSpeed = 3;
    public float linkSpeed = 1;
    public float cdTime = 8;
    public bool canEvade; // can the enemy evade
    public bool canEvadeState; // can the enemy enter the evade state
    public float evadeTime; // time last evade started
    public float evadeLimit; // how long the evade cooldown is
    public GameObject spawner;
    public bool shootNowDaddy;  // start shooting
    public bool returnToRun; // lets it exit the shoot state back to run
    public bool canJump; // tells it if it ca jump
    

    //called on Awake
    public override void Awake()
    {

        // dfines all the states that this entity has
        base.Awake();
        shooting = FMODUnity.RuntimeManager.CreateInstance(bulletRef);
        moveState = new E1_Move(this, stateMachine, moveData, entityData, this);
        slowState = new E1_SlowApproach(this, stateMachine, slowData, entityData, this);
        aimState = new E1_AimingState(this, stateMachine, aimData, entityData, this);
        evadeState = new E1_Evade(this, stateMachine, evadeData, entityData, this);
        knockbackState = new E1_Knockback(this, stateMachine, knockbackData, entityData, this);
        deathState = new DeathState(this, stateMachine);
        ammo = this.gameObject.GetComponent<AmmoPool>();
        //makes the move state the entitys initial state
        stateMachine.InitializeStateMachine(moveState);
        canShoot = true;
        //this.gameObject.SetActive(false);
        defaultState = moveState;
        queueName = "Ranged";


    }

    public override void Update()
    {
        base.Update();
        //timer cooldown for when it can evade again
        if (Time.time > evadeTime + evadeLimit)
        {
            canEvadeState = true;
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        // controls the enemies speed when jumping between links
        if (agent.isOnOffMeshLink && linking == false)
        {
            linking = true;
            agent.speed = agent.speed * linkSpeed;
        }
        else if (agent.isOnNavMesh && linking == true)
        {
            linking = false;
            agent.velocity = Vector3.zero;
            agent.speed = origSpeed;
        }
    }
    // gets the distance to the player
    public override float DistanceToPlayer()
    {
        return base.DistanceToPlayer();
    }

    //check if still used
    public void StartCool(GameObject bullet)
    {
        StartCoroutine(Cooldown(bullet));
    }
    IEnumerator Cooldown(GameObject bullet)
    {
        yield return new WaitForSeconds(3f);
        canShoot = true;
    }
    
    // calls when damage is taken
    public override void Damage(float amountDamage)
    {
        // Damage() is called in the base class with the amount of damage taken passed
        //base.Damage(amountDamage);
        // enemy enters knock back state
        //stateMachine.ChangeState(knockbackState);
        
        

        // if the enemy dropps to zero or below it will die
        if(health <= 0 || gameObject.GetComponent<EnemyStats>().getCurrentHealth() <= 0)
        {
            stateMachine.ChangeState(deathState);
            StartCoroutine(resetSpawn());
        }
        
        

    }
    // Called when enemy health reaches 0
    
    
    //currently disabled
    public void jumpBack()
    {
        //changes back to move state in .5 seconds
        Invoke("ResetHitBack", 1f);
    }
    //resets Enemy state: currently disabled
    private void ResetHitBack()
    {
        stateMachine.ChangeState(moveState);
    }

    // debug to test collisions. Not used anymore, but may be in future
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name== "deathWall")
        {
            Debug.LogWarning("Oh I am now dead, thank you forever");
            Die();
        }
    }

    IEnumerator resetSpawn()
    {
        yield return new WaitForSeconds(3f);
        mAnimator.SetBool("dead", false);
        mAnimator.SetBool("chasing", true);
        GetComponentInParent<EnemyTriggerGroup>().enemyDead += 1;
        if (GetComponentInParent<EnemyTriggerGroup>().spawned < GetComponentInParent<EnemyTriggerGroup>().enemySize)
        {
            GetComponentInParent<EnemyTriggerGroup>().spawned += 1;
            
            GetComponent<EnemyStats>().NoahAIAddToActiveList();
            stateMachine.ChangeState(moveState);
            transform.position = (spawner.transform.position);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    public void bangbang()
    {
        FMODUnity.RuntimeManager.PlayOneShot(bulletRef, transform.position);
    }

    
}

