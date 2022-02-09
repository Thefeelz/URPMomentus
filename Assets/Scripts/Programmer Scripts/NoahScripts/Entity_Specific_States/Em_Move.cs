using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Em_Move : MoveState
{
    private Enemy_Melee mEnemy;
    public Em_Move(Entity mEntity, FiniteStateMachine mStateMachine, D_moveState stateData, D_Entity entityData, Enemy_Melee mEnemy) : base(mEntity, mStateMachine, stateData, entityData)
    {
        this.mEnemy = mEnemy;
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

    protected override void Move()
    {
        base.Move();
    }
}
