using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpecialUseState : State
{
    public SpecialUseState(Entity mEntity, FiniteStateMachine mStateMachine) : base(mEntity, mStateMachine)
    {

    }

    public override void StateEnter()
    {
        base.StateEnter();
        if (mEntity.specialUseBool == true)
        {
            mEntity.GetComponent<NavMeshAgent>().enabled = false;
            mEntity.GetComponent<Rigidbody>().velocity = Vector3.zero;
            mEntity.GetComponentInChildren<Animator>().speed = 0;
        }
        else
        {
            if(mEntity.gameObject.GetComponent<Enemy1>())
            {
                mEntity.stateMachine.ChangeState(mEntity.defaultState);
            }
        }
    }

    public override void StateExit()
    {
        
        base.StateExit();
        mEntity.specialUseBool = false;
        mEntity.GetComponentInChildren<Animator>().speed = 1;
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
