using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class E2_Move : MoveState
{
    private Enemy2 mEnemy;

    public E2_Move(Entity mEntity, FiniteStateMachine mStateMachine, D_moveState stateData, D_Entity entityData, Enemy2 enemy) : base(mEntity, mStateMachine, stateData, entityData)
    {

        mEnemy = enemy;
    }

    public override void StateEnter()
    {
        Debug.Log("Move enter");
        base.StateEnter();
        mEnemy.gameObject.GetComponent<NavMeshAgent>().enabled = true;
        //mEnemy.gameObject.GetComponentInChildren<Animator>().SetBool("chasing", true);
        mEnemy.gameObject.GetComponent<NavMeshAgent>().speed = stateData.moveSpeed;

        mEntity.agent.SetDestination(mEntity.myTarget.transform.position);
        
        

    }

    public override void StateExit()
    {
        Debug.Log("exiting Move");
        base.StateExit();
    }

    // every logic update the base logicupdate is called, the move function is called, and the distance to the target is checked as to see if
    // the enemy is close enough to change states
    public override void LogicUpdate()
    {
        Debug.Log("FUCK");
        base.LogicUpdate();
        if (mEntity.DistanceToPlayer() <= 5)
        {
            
            mStateMachine.ChangeState(mEnemy.slowState);
        }
        else
        {
            Debug.Log(mEntity.DistanceToPlayer());
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
