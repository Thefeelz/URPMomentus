using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1_Knockback : KnockbackState
{
    private Enemy1 mEnemy;
    public E1_Knockback(Entity mEntity, FiniteStateMachine mStateMachine, D_Knockback knockbackData, D_Entity entityData, Enemy1 mEnemy) : base(mEntity, mStateMachine, knockbackData, entityData)
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
        if (progress >= 1.0 && mEntity.grounded == true)
        {

            mEntity.stateMachine.ChangeState(mEnemy.moveState);
            
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
