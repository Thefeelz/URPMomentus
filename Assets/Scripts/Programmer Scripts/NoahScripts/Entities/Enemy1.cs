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
    // script for functions and  that involve ranged attacking
    public RangedBehavior rangedBehavior { get; private set; }
    // a bool for keeping track of when enemy can shoot
    public bool canShoot { get; private set; }
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

    //called on Awake
    public override void Awake()
    {
        base.Awake();
        // dfines all the states that this entity has
        moveState = new E1_Move(this, stateMachine, moveData, entityData, this);
        slowState = new E1_SlowApproach(this, stateMachine, slowData, entityData, this);
        aimState = new E1_AimingState(this, stateMachine, aimData, entityData, this);
        evadeState = new E1_Evade(this, stateMachine, evadeData, entityData, this);
        knockbackState = new E1_Knockback(this, stateMachine, knockbackData, entityData, this);

        //makes the move state the entitys initial state
        stateMachine.InitializeStateMachine(moveState);
        canShoot = true;
    }

    public override void Update()
    {
        base.Update();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    // gets the distance to the player
    public override float DistanceToPlayer()
    {
        return base.DistanceToPlayer();
    }
    public void StartCool()
    {
        StartCoroutine(Cooldown());
    }
    IEnumerator Cooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(3f);
        canShoot = true;
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
