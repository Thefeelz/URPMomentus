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
        mEntity.GetComponent<NavMeshAgent>().enabled = false;
        mEntity.GetComponent<Rigidbody>().velocity = Vector3.zero;
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
