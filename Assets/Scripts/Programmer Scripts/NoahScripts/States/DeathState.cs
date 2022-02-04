using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : State
{
   
    public DeathState(Entity mEntity, FiniteStateMachine mStateMachine) : base(mEntity, mStateMachine)
    {
        
    }
    public override void StateEnter()
    {
        base.StateEnter();
        mEntity.agent.speed = 0;
        mEntity.mAnimator.SetBool("dead", true);
        
    }


    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

 
    public override void StateExit()
    {
        base.StateExit();
    }

   
}
