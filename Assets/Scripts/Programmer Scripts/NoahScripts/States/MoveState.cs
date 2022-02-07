using System.Collections;
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
        if (Vector3.Distance(mEntity.agent.destination, mEntity.myTarget.transform.position) > 1)
        {
            mEntity.agent.SetDestination(mEntity.myTarget.transform.position);
        }

    }
    public override void StateEnter()
    {

        base.StateEnter();
        mEntity.gameObject.GetComponent<NavMeshAgent>().enabled = true;
        //mEntity.agent.speed = stateData.moveSpeed;
        mEntity.agent.speed = 3;
        if (mEntity.gameObject.GetComponent<NavMeshAgent>().enabled == true)
        {
            mEntity.agent.SetDestination(mEntity.myTarget.transform.position);
        }
    }

    public override void StateExit()
    {
        base.StateExit();
    }


    // fix this, target position is being called when it shouldnt
    public override void LogicUpdate()
    {

        base.LogicUpdate();
        if(mEntity.agent.destination == null)
        {
            mEntity.agent.SetDestination(mEntity.myTarget.transform.position);
        }
        
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
