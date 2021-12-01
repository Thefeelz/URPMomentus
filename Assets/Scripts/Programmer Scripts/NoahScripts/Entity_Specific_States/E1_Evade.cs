using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class E1_Evade : EvadeState
{
    
    float x;
    Enemy1 mEnemy;
    public bool jumpStarted;
    public Vector3 targetPos; // the target position to jump to
    private float distance; // distance to travel
    private float height; // height of arc
    private float step;
    private float speed; // speed of travel
    private Vector3 startPos; // initial position when starting
    private float progress; // how far into jump
    private Vector3 direction; // direction of jump
    private bool jumping; // bool to check if it is jumping back or dashing forward
    public E1_Evade(Entity mEntity, FiniteStateMachine mStateMachine, D_Evade evadeData, D_Entity entityData, Enemy1 mEnemy) : base(mEntity, mStateMachine, evadeData, entityData)
    {
        x = mEntity.transform.position.x;
        this.mEnemy = mEnemy;
        distance = evadeData.distance;
        height = evadeData.height;
        speed = evadeData.speed;
    }

    public override void StateEnter()
    {
        base.StateEnter();
        jumpStarted = true;
        Debug.Log("evade entered");
        mEnemy.GetComponent<NavMeshAgent>().enabled = false;
        //resets variables each time
        //mEnemy.facePlayer();
        targetPos = mEnemy.transform.position - (mEnemy.transform.forward * distance);
        step = speed / mEnemy.DistanceToPosition(targetPos);
        startPos = mEnemy.transform.position;
        progress = 0;
        jumping = true;
        RaycastHit hit;
        if (Physics.Raycast(mEnemy.transform.position, mEnemy.transform.forward * -1, out hit, 100.0f))
        {
            Debug.Log("OH FUCK");
            jumping = false;
        }
        

    }

    public override void StateExit()
    {
        base.StateExit();
        mEnemy.GetComponent<NavMeshAgent>().enabled = true;
    }

    public override void LogicUpdate()
    {
        if (progress < 1)
        {
            progress = Mathf.Min(progress + Time.deltaTime * step, 1.0f);
            float parabola = 1.0f - 4.0f * (progress - 0.5f) * (progress - 0.5f);
            Vector3 nextPos = Vector3.Lerp(startPos, targetPos, progress);

            // Then add a vertical arc in excess of this.
            nextPos.y += parabola * height;

            // Continue as before.
            //mEnemy.transform.LookAt(nextPos, mEnemy.transform.forward);
            mEnemy.transform.position = nextPos;
            // if enemy is in the air then it is not grounded
            if (progress < 1.0)
            {
                mEnemy.grounded = false;
            }
        }
        if (progress >= 1.0 && mEnemy.grounded == true)
        {

            mEnemy.stateMachine.ChangeState(mEnemy.moveState);
        }
        
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        
    }

    
}
