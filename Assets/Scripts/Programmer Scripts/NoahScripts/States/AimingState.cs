using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AimingState : State
{
    
    protected bool canShoot = true;
    protected D_Entity entityData;

    public AimingState(Entity mEntity, FiniteStateMachine mStateMachine, D_Aiming aimData, D_Entity entityData) : base(mEntity, mStateMachine)
    {
        this.entityData = entityData;
    }

    public override void StateEnter()
    {
        base.StateEnter();
        // disables navmesh to completely cancel movement and allow custom rotation
        mEntity.gameObject.GetComponent<NavMeshAgent>().enabled = false;
        
    }

    public override void StateExit()
    {
        base.StateExit();
        mEntity.gameObject.GetComponent<NavMeshAgent>().enabled = true;
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
        //the enemy rotates to face the player
        //Vector3 newDirection = Vector3.RotateTowards(mEntity.transform.forward, entity.myTarget.transform.position, 2 * Time.deltaTime, 0.0f);
        //mEntity.transform.rotation = Quaternion.LookRotation(newDirection);
        //mEntity.transform.LookAt(mEntity.myTarget.transform);
    }

   
}
