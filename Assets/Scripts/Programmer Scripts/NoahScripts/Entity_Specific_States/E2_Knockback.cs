using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E2_Knockback : KnockbackState
{
    public E2_Knockback(Entity mEntity, FiniteStateMachine mStateMachine, D_Knockback knockbackData, D_Entity entityData, Enemy2 mEnemy) : base(mEntity, mStateMachine, knockbackData, entityData)
    {

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
