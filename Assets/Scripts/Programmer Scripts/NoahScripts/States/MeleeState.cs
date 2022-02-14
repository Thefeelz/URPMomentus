using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeState : State
{
    protected D_Entity entityData;
    public MeleeState(Entity mEntity, FiniteStateMachine mStateMachine, D_Entity entityData) : base(mEntity, mStateMachine)
    {
        this.entityData = entityData;
    }
    public override void StateEnter()
    {
        base.StateEnter();
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
