using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Em_Melee : MeleeState
{
    Enemy_Melee mEnemy;
    
    public Em_Melee(Entity mEntity, FiniteStateMachine mStateMachine, D_Entity entityData, Enemy_Melee mEnemy) : base(mEntity, mStateMachine, entityData)
    {
        this.mEnemy = mEnemy;
    }

    public override void StateEnter()
    {
        base.StateEnter();
        mEnemy.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
        mEnemy.GetComponent<UnityEngine.AI.NavMeshObstacle>().enabled = true;
        mEntity.facePlayer();
        mEntity.mAnimator.SetBool("chasing", false);
        mEntity.mAnimator.SetBool("meleeAttack", true);
        start = false;
        Strike();
    }

    public override void StateExit()
    {
        base.StateExit();
        mEnemy.GetComponent<UnityEngine.AI.NavMeshObstacle>().enabled = false ;
        mEnemy.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
        mEntity.mAnimator.SetBool("meleeAttack", false);
        mEntity.mAnimator.SetBool("chasing", true);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(start == true && Time.time - strikeStart >= 2)
        {
            mEnemy.stateMachine.ChangeState(mEnemy.moveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    
}
