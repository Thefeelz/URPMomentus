using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaitingState : State
{
    protected D_Entity entityData;
    protected MeleeState eMeleeState; // SHOULD be able to just have a universal waiting state assigning a child MeleeState to the parent melee state object
    protected MoveState eMoveState;
    protected RaycastHit send;

    public WaitingState(Entity mEntity, FiniteStateMachine mStateMachine, D_Entity entityData, MeleeState eMeleeState, MoveState eMoveState) : base(mEntity, mStateMachine)
    {
        this.entityData = entityData;
        this.eMeleeState = eMeleeState;
        this.eMoveState = eMoveState;
    }

    public override void StateEnter()
    {
        base.StateEnter();
        Physics.Raycast(mEntity.transform.position + Vector3.up + (mEntity.transform.forward * 0.5f), mEntity.transform.forward, out send, 1f);
        if (send.collider != null && send.collider.GetComponentInParent<Entity>())
        {
            mEntity.GetComponent<NavMeshAgent>().enabled = false;
            mEntity.GetComponent<NavMeshObstacle>().enabled = true;
        }
    }

    public override void StateExit()
    {
        base.StateExit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (mEntity.DistanceToPlayer() <= 2)
        {
            mEntity.stateMachine.ChangeState(eMeleeState);
        }
        Physics.Raycast(mEntity.transform.position + Vector3.up + (mEntity.transform.forward * 0.5f), mEntity.transform.forward, out send, 1f);
        Debug.DrawRay(mEntity.transform.position + Vector3.up + (mEntity.transform.forward * 0.5f), mEntity.transform.forward, Color.green);
        if (send.collider != null && send.collider.GetComponentInParent<Entity>() && send.collider.GetComponentInParent<NavMeshObstacle>().enabled == true)
        {
            mEntity.GetComponent<NavMeshAgent>().enabled = false;
            mEntity.GetComponent<NavMeshObstacle>().enabled = true;
        }
        else
        {
            mEntity.GetComponent<NavMeshObstacle>().enabled = false;
            mEntity.GetComponent<NavMeshAgent>().enabled = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

}
