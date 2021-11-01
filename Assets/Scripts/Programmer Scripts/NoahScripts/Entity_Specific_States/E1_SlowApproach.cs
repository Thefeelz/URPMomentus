using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1_SlowApproach : SlowApproach
{
    private Enemy1 mEnemy;
    //assign all variables
    public E1_SlowApproach(Entity mEntity, FiniteStateMachine mStateMachine, D_SlowApproach stateData, D_Entity entityData, Enemy1 mEnemy) : base(mEntity, mStateMachine, stateData, entityData)
    {
        this.mEntity = mEntity;
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
        // if the enemy is within ranged distance switch to aim state
        if (mEntity.DistanceToPlayer() <= entityData.aimDistance)
        {
            mStateMachine.ChangeState(mEnemy.aimState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
