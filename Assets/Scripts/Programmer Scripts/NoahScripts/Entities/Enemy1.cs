﻿using System.Collections;
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

    public bool linking;
    public float origSpeed = 3;
    public float linkSpeed = 1;
    public float cdTime = 8;
    public bool canEvade;

    //called on Awake
    public override void Awake()
    {

        // dfines all the states that this entity has
        base.Awake();
        moveState = new E1_Move(this, stateMachine, moveData, entityData, this);
        slowState = new E1_SlowApproach(this, stateMachine, slowData, entityData, this);
        aimState = new E1_AimingState(this, stateMachine, aimData, entityData, this);
        evadeState = new E1_Evade(this, stateMachine, evadeData, entityData, this);
        knockbackState = new E1_Knockback(this, stateMachine, knockbackData, entityData, this);
        ammo = this.gameObject.GetComponent<AmmoPool>();
        //makes the move state the entitys initial state
        stateMachine.InitializeStateMachine(moveState);
        canShoot = true;
        this.gameObject.SetActive(false);


    }

    public override void Update()
    {
        base.Update();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
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
    public void StartCool(GameObject bullet)
    {
        StartCoroutine(Cooldown(bullet));
    }
    IEnumerator Cooldown(GameObject bullet)
    {
        yield return new WaitForSeconds(3f);
        bullet.SetActive(false);
        ammo.enqueBullet(bullet);
        canShoot = true;
    }
    public void evadeCool()
    {
        StartCoroutine(evadeCD());
    }
    IEnumerator evadeCD()
    {
        yield return new WaitForSeconds(cdTime);
        canEvade = true;
    }
    // calls when damage is taken
    public override void Damage(float amountDamage)
    {
        // Damage() is called in the base class with the amount of damage taken passed
        base.Damage(amountDamage);
        // enemy enters knock back state
        stateMachine.ChangeState(knockbackState);
        
        // in .5 seconds the knock back will end
        Invoke("ResetHitBack", .5f);
        if(health <= 0)
        {
            myPool.queueObject("Ranged", this.gameObject);
        }
        

    }
    // Called when enemy health reaches 0
    
    
    public void jumpBack()
    {
        //changes back to move state in .5 seconds
        Invoke("ResetHitBack", 1f);
    }
    //resets Enemy state
    private void ResetHitBack()
    {
        stateMachine.ChangeState(moveState);
    }
}
