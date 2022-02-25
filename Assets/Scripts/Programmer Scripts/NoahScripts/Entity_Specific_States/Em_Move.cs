using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
        mEntity.mAnimator.SetBool("chasing", true);
        mEntity.mAnimator.SetBool("stationary", false);
       
    }

    public override void StateExit()
    {
        base.StateExit();
    }

    public override void LogicUpdate()
    {
        //mEnemy.agent.avoidancePriority = mEnemy.avoid;
        Move();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    protected override void Move()
    {
        //base.Move();
        Locations l = mEnemy.mLocations;
        float minDist = Mathf.Infinity;
        for(int i = 0; i < 8; i++)
        {
            if(l.lSpotsValid[i] && l.lSpotsTaken[i] != true)
            {

            }
        }
    }
}
