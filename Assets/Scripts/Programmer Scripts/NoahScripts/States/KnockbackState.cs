using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KnockbackState : State
{

    public KnockbackState(Entity mEntity, FiniteStateMachine mStateMachine, D_Knockback knockbackData, D_Entity entityData) : base(mEntity, mStateMachine)
    {

    }


    // Start is called before the first frame update
    public override void StateEnter()
    {
        base.StateEnter();
        //mEntity.agent.enabled = false;
        //disable navmesh
        mEntity.GetComponent<NavMeshAgent>().enabled = false;
        //clear velocity
        mEntity.GetComponent<Rigidbody>().velocity = Vector3.zero;
        // shoot enemy up and away from player wiht a small amount of force
        mEntity.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * 1.5f, ForceMode.Impulse);
        Vector3 dir = (mEntity.transform.position - mEntity.myTarget.transform.position).normalized;
        mEntity.gameObject.GetComponent<Rigidbody>().AddForce(dir * 3f, ForceMode.Impulse);
        
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
