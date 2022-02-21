using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1_Move : MoveState
{
    private Enemy1 mEnemy;
    
    public E1_Move(Entity mEntity, FiniteStateMachine mStateMachine, D_moveState stateData, D_Entity entityData, Enemy1 enemy) : base(mEntity, mStateMachine, stateData, entityData)
    {

        mEnemy = enemy;
    }

    public override void StateEnter()
    {
        base.StateEnter();
        mEntity.mAnimator.SetBool("chasing", true);
        mEntity.mAnimator.SetBool("stationary", false);
        
    }

    public override void StateExit()
    {
        base.StateExit();
    }

    // every logic update the base logicupdate is called, the move function is called, and the distance to the target is checked as to see if
    // the enemy is close enough to change states
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        if (mEntity.DistanceToPlayer() <= entityData.aimDistance)
        {
            mStateMachine.ChangeState(mEnemy.aimState);
        }
        else
        {
            Move();
        }

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
