using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class E1_Evade : EvadeState
{
    
    float x;
    Enemy1 mEnemy;
    public bool jumpStarted;
    public E1_Evade(Entity mEntity, FiniteStateMachine mStateMachine, D_Evade evadeData, D_Entity entityData, Enemy1 mEnemy) : base(mEntity, mStateMachine, evadeData, entityData)
    {
        x = mEntity.transform.position.x;
        this.mEnemy = mEnemy;
    }

    public override void StateEnter()
    {
        base.StateEnter();
        jumpStarted = true;
        Debug.Log("evade entered");
        // makes the enemy jump back using force
        mEntity.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * 5f, ForceMode.Impulse);
        Vector3 dir = (mEntity.transform.position - mEntity.myTarget.transform.position).normalized;
        mEntity.gameObject.GetComponent<Rigidbody>().AddForce(dir * 3f , ForceMode.Impulse);
        mEnemy.jumpBack();
    }

    public override void StateExit()
    {
        base.StateExit();
    }

    public override void LogicUpdate()
    {
        
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
