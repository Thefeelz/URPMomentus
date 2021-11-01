﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveState : State
{
    // Start is called before the first frame update

    protected D_moveState stateData;
    protected D_Entity entityData;
    // the distance from the entity to the player
    protected float playerDistance;
    

    public MoveState(Entity mEntity, FiniteStateMachine mStateMachine, D_moveState stateData, D_Entity entityData) : base(mEntity, mStateMachine)
    {
          
        this.stateData = stateData;
        this.entityData = entityData;
    }
    protected virtual void Move()
    {
        // movement code here
        mEntity.agent.SetDestination(mEntity.myTarget.transform.position);

    }
    public override void StateEnter()
    {
        Debug.Log("Move enter");
        base.StateEnter();
        mEntity.gameObject.GetComponent<NavMeshAgent>().enabled = true;

        mEntity.agent.SetDestination(mEntity.myTarget.transform.position);
    }

    public override void StateExit()
    {
        base.StateExit();
    }

    public override void LogicUpdate()
    {

        base.LogicUpdate();
        
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}