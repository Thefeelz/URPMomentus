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
    private float step2; // step for testfire
    private float speed; // speed of travel
    private float speed2; // speed for testfire
    private Vector3 startPos; // initial position when starting
    private float progress; // how far into jump
    private Vector3 direction; // direction of jump
    private bool jumping; // bool to check if it is jumping back or dashing forward
    private float prog2; // progress for test fire
    public E1_Evade(Entity mEntity, FiniteStateMachine mStateMachine, D_Evade evadeData, D_Entity entityData, Enemy1 mEnemy) : base(mEntity, mStateMachine, evadeData, entityData)
    {
        x = mEntity.transform.position.x;
        this.mEnemy = mEnemy;
        distance = evadeData.distance;
        height = evadeData.height;
        speed = evadeData.speed;
        speed2 = 20;
        mEnemy.canEvade = true; // even though it is set to true by default nothing will happen till check is ran
    }

    public override void StateEnter()
    {
        base.StateEnter();
        mEnemy.testFire.transform.position = mEnemy.transform.position;
        jumpStarted = true;
        prog2 = 0; // progress of test
        Debug.Log("evade entered");
        mEnemy.GetComponent<NavMeshAgent>().enabled = false;
        //resets variables each time
        mEnemy.facePlayer();
        targetPos = mEnemy.transform.position - (mEnemy.transform.forward * distance);
        step = speed / mEnemy.DistanceToPosition(targetPos);
        step2 = speed2 / mEnemy.DistanceToPosition(targetPos);
        startPos = mEnemy.transform.position;
        progress = 0;
        jumping = false;
        //RaycastHit hit;
        //if (Physics.Raycast(mEnemy.transform.position, mEnemy.transform.forward * -1, out hit, 10.0f ))
        //{
        //    Debug.Log(hit.collider.gameObject.name);
            
        //        Debug.Log("OH FUCK");
        //        jumping = false;
            
        //}
        

    }

    public override void StateExit()
    {
        base.StateExit();
        prog2 = 0;
        mEnemy.evadeCool();
        mEnemy.GetComponent<NavMeshAgent>().enabled = true;
    }

    public override void LogicUpdate()
    {
        Vector3 path = mEnemy.transform.TransformDirection(Vector3.forward * -1) * 10;
        Debug.DrawRay(mEnemy.transform.position, path, Color.green);
        if (progress < 1 && jumping == true)
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
        // test

        if (progress >= 1.0 && mEnemy.grounded == true)
        {

            mEnemy.stateMachine.ChangeState(mEnemy.moveState);
        }
        if(mEnemy.canEvade == false)
        {
            jumping = false;
            mEnemy.Die();
        }
        //if(jumping == false)
        //{
        //    //mEnemy.stateMachine.ChangeState(mEnemy.aimState);
        //}

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        // THIS IS THE TEST FIRE VERSION! IT WILL BE CALLED FIRST!
        if (prog2 < 1)
        {
            prog2 = Mathf.Min(prog2 + Time.deltaTime * step2, 1.0f);
            float parabola = 1.0f - 4.0f * (prog2 - 0.5f) * (prog2 - 0.5f);
            Vector3 nextPos = Vector3.Lerp(startPos, targetPos, prog2);

            // Then add a vertical arc in excess of this.
            nextPos.y += parabola * height;

            // Continue as before.
            //mEnemy.transform.LookAt(nextPos, mEnemy.transform.forward);
            mEnemy.testFire.transform.position = nextPos;
            Debug.Log(prog2);


        }
        if(prog2 >= 1 && mEnemy.canEvade == true)
        {
            jumping = true;
        }

    }

   


    
}
