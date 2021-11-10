using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy2 : Entity
{
    //the states that this enemy variation has
    public Vector3 targPos;

    public E2_Move moveState { get; private set; }
    public E2_SlowApproach slowState { get; private set; }
    public E2_Evade evadeState { get; private set; }
    public E2_Knockback knockbackState { get; private set; }
    public E2_Attack attackState { get; private set; }
    
    public bool jumpStarted;
 
    // various state datas
    [SerializeField]
    private D_moveState moveData;
    [SerializeField]
    private D_SlowApproach slowData;
    [SerializeField]
    private D_Evade evadeData;
    [SerializeField]
    private D_Knockback knockbackData;
    [SerializeField]
    private D_Attack attackData;

    private bool hitBackStart;






    public override void Awake()
    {
        Debug.Log("test");
        base.Awake();
        Physics.IgnoreLayerCollision(9, 8);
        // dfines all the states that this entity has
        moveState = new E2_Move(this, stateMachine, moveData, entityData, this);
        slowState = new E2_SlowApproach(this, stateMachine, slowData, entityData, this);
        evadeState = new E2_Evade(this, stateMachine, evadeData, entityData, this);
        knockbackState = new E2_Knockback(this, stateMachine, knockbackData, entityData, this);
        attackState = new E2_Attack(this, stateMachine, attackData, entityData, this);
        // sets the health

        //makes the move state the entitys initial state
        stateMachine.InitializeStateMachine(moveState);

    }

    //resets state when enemy is recycled
    private void OnEnable()
    {
        stateMachine.InitializeStateMachine(moveState);
    }

    public override void Update()
    {
        base.Update();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    public override float DistanceToPlayer()
    {
        return base.DistanceToPlayer();
    }


    // calls when damage is taken
    public override void Damage(float amountDamage)
    {
        // Damage() is called in the base class with the amount of damage taken passed
        base.Damage(amountDamage);
        hitBackStart = true;
        // enemy enters knock back state
        stateMachine.ChangeState(knockbackState);

        // in .5 seconds the knock back will end
        Invoke("ResetHitBack", .5f);

        if (health <= 0)
        {
            Die();
        }
    }
    public void jumpBack()
    {
        //changes back to move state in .5 seconds
        Invoke("ResetHitBack", 1f);
    }
    // invokes the function of the passed string, after the passed time
    public void invokeFunction(float time, string function)
    {
        Invoke(function, time);
        //function(param1, param2)
    }
    private void ResetHitBack()
    {
        stateMachine.ChangeState(slowState);
    }
    private void AttackEnter()
    {
        stateMachine.ChangeState(attackState);
    }


    //calls coroutine with passed parameters
    public override void callCoroutine(string sName, float fTime)
    {
        StartCoroutine(stateSwitch(sName, fTime));
    }

    //uses a string and a float with a switch statement to change to the state of the passed name, after fTime, if conditions are met
    IEnumerator stateSwitch(string sName, float fTime)
    {
        yield return new WaitForSeconds(fTime);
        switch(sName)
        {
            case "attackState":
                if(this.stateMachine.currentState == slowState)
                {
                    RaycastHit hit;
                    Vector3 point = myTarget.transform.position - transform.position;
                    // enemy will only jump if it is not currently phasing. Otherwise it will generate a new random time
                    if (Physics.Raycast(transform.position, point, out hit, Mathf.Infinity) && hit.transform.tag != "Walls")
                    {
                        Debug.DrawRay(transform.position, point * hit.distance, Color.red);
                        Debug.Log("yes jump");
                        this.stateMachine.ChangeState(attackState);
                    }
                    else
                    {
                        Debug.Log("no jumping allowed");
                        slowData.circleStart = false;
                    }
                }
                break;

            case "slowState":
                this.stateMachine.ChangeState(slowState);
                break;
            case "moveState":
                this.stateMachine.ChangeState(moveState);
                break;

        }
    }

    public override void Die()
    {
        base.Die();
        myPool.queueObject("Melee", this.gameObject);
    }
}
