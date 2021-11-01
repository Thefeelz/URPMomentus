using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State
{
    protected D_Patrol patrolData;
    protected D_Entity entityData;
    public PatrolState(Entity mEntity, FiniteStateMachine mStateMachine, D_Patrol patrolData, D_Entity entityData) : base(mEntity, mStateMachine)
    {
        this.patrolData = patrolData;
        this.entityData = entityData;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void StateEnter()
    {
        base.StateEnter();
    }

    public override void StateExit()
    {
        base.StateExit();
    }
}
