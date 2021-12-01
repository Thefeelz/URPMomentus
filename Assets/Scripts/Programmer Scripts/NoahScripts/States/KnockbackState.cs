using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KnockbackState : State
{
    public Vector3 targetPos; // the target position to jump to
    protected float distance;
    protected float height;
    protected float step;
    protected float speed;
    protected Vector3 startPos;
    protected float progress;
    public KnockbackState(Entity mEntity, FiniteStateMachine mStateMachine, D_Knockback knockbackData, D_Entity entityData) : base(mEntity, mStateMachine)
    {
        distance = knockbackData.distance;
        height = knockbackData.height;
        speed = knockbackData.speed;
    }


    // Start is called before the first frame update
    public override void StateEnter()
    {
        base.StateEnter();
        //mEntity.agent.enabled = false;
        //disable navmesh
        mEntity.GetComponent<NavMeshAgent>().enabled = false;
        //clear velocity
        // test code
        targetPos = mEntity.transform.position - (mEntity.transform.forward * distance);
        step = speed / mEntity.DistanceToPosition(targetPos);
        startPos = mEntity.transform.position;
        progress = 0;

    }

    public override void StateExit()
    {
        //reset velocity so its not moving back 
        mEntity.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //turn back on the navmesh
        mEntity.GetComponent<NavMeshAgent>().enabled = true;
        base.StateExit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (progress < 1)
        {
            progress = Mathf.Min(progress + Time.deltaTime * step, 1.0f);
            float parabola = 1.0f - 4.0f * (progress - 0.5f) * (progress - 0.5f);
            Vector3 nextPos = Vector3.Lerp(startPos, targetPos, progress);

            // Then add a vertical arc in excess of this.
            nextPos.y += parabola * height;

            // Continue as before.
            //mEnemy.transform.LookAt(nextPos, mEnemy.transform.forward);
            mEntity.transform.position = nextPos;
            // if enemy is in the air then it is not grounded
            if (progress < 1.0)
            {
                mEntity.grounded = false;
            }
        }
        // LAST BIT IS IN CHILDREN!!!
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
