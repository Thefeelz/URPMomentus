using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EvadeState : State
{
    D_Entity entityData;
    
    public EvadeState(Entity mEntity, FiniteStateMachine mStateMachine, D_Evade evadeData, D_Entity entityData) : base(mEntity, mStateMachine)
    {
        this.entityData = entityData;
    }

    public override void StateEnter()
    {
        //disable navmeshagent when entering
        mEntity.gameObject.GetComponent<NavMeshAgent>().enabled = false;

        base.StateEnter();
    }

    public override void StateExit()
    {
        //reset velocity so its not moving back 
        mEntity.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //turn back on the navmesh
        mEntity.GetComponent<NavMeshAgent>().enabled = true;
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
