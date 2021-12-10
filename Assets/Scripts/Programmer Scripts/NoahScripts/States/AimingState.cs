using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AimingState : State
{
    
    protected bool canShoot = true;
    protected D_Entity entityData;
    protected D_Aiming aimData;

    public AimingState(Entity mEntity, FiniteStateMachine mStateMachine, D_Aiming aimData, D_Entity entityData) : base(mEntity, mStateMachine)
    {
        this.entityData = entityData;
        this.aimData = aimData;
    }

    public override void StateEnter()
    {
        base.StateEnter();
        // disables navmesh to completely cancel movement and allow custom rotation

        
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

    public virtual void Aim()
    {
    
    }

   
}
