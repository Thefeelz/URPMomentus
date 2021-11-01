using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlowApproach : State
{
    protected D_SlowApproach slowData;
    protected D_Entity entityData;
    

    //assigns variables
    public SlowApproach(Entity mEntity, FiniteStateMachine mStateMachine, D_SlowApproach stateData, D_Entity entityData) : base(mEntity, mStateMachine)
    {
        slowData = stateData;
        this.entityData = entityData;
    }
    public override void StateEnter()
    {
        base.StateEnter();
        mEntity.gameObject.GetComponent<NavMeshAgent>().enabled = true;
        mEntity.agent.speed = slowData.speed;
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

    protected virtual void Move()
    {
        // movement code here
        //mEntity.agent.SetDestination(mEntity.myTarget.transform.position);
    }
}
