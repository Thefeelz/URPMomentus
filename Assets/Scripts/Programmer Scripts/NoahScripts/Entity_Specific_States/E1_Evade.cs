using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class E1_Evade : EvadeState
{
    
    float x; // check if used
    Enemy1 mEnemy;
    public bool jumpStarted;
    public Vector3 targetPos; // the target position to jump to
    private float distance; // distance to travel
    private float height; // height of arc
    private float step; // step for actual jump
    private float stepTest; // step for testfire
    private float speed; // speed of travel
    private float speed2; // speed for testfire
    private Vector3 startPos; // initial position when starting
    private float progress; // how far into jump
    private Vector3 direction; // direction of jump: see if used
    private bool jumping = false; // bool to check if it is jumping back or dashing forward
    private float prog2; // progress for test fire
    public E1_Evade(Entity mEntity, FiniteStateMachine mStateMachine, D_Evade evadeData, D_Entity entityData, Enemy1 mEnemy) : base(mEntity, mStateMachine, evadeData, entityData)
    {
        x = mEntity.transform.position.x;
        this.mEnemy = mEnemy;
        distance = evadeData.distance;
        height = evadeData.height;
        speed = evadeData.speed;
        speed2 = 10;
        
    }

    public override void StateEnter()
    {
        base.StateEnter();
        mEnemy.canEvade = true; // even though it is set to true by default nothing will happen till check is ran
        mEnemy.testFire.transform.position = mEnemy.transform.position;
        jumpStarted = true;
        prog2 = 0; // progress of testfire 
        Debug.Log("evade entered");
        mEnemy.GetComponent<NavMeshAgent>().enabled = false; //disables nav mesh
        //resets variables each time
        mEnemy.facePlayer(); // makes the enemy face the player !!!STILL BUGGY!!!
        targetPos = mEnemy.transform.position - (mEnemy.transform.forward * distance); // position that arc goes to
        step = speed / mEnemy.DistanceToPosition(targetPos); // used to calculate rate of arc. complicated bullshit
        stepTest = speed2 / mEnemy.DistanceToPosition(targetPos); // more complicated bullshit
        startPos = mEnemy.transform.position; // keeps track of the starting position of the evade
        progress = 0; // progress resets each time
        
        

    }

    public override void StateExit()
    {
        base.StateExit();
        jumping = false;
        prog2 = 0;
        mEnemy.gameObject.GetComponent<Rigidbody>().useGravity = false;
        mEnemy.GetComponent<NavMeshAgent>().enabled = true;
    }

    public override void LogicUpdate()
    {
        //Debug.Log(jumping);
        Vector3 path = mEnemy.transform.TransformDirection(Vector3.forward * -1) * 10;
        Debug.DrawRay(mEnemy.transform.position, path, Color.green);
        jumpArc();
        // test

        if (progress >= 1.0 && mEnemy.grounded == true)
        {

            mEnemy.stateMachine.ChangeState(mEnemy.moveState);
        }
        //if(mEnemy.canEvade == false)
        //{
        //    jumping = false;
        //    mEnemy.Die();
        //}
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
            prog2 = Mathf.Min(prog2 + Time.deltaTime * stepTest, 1.0f);
            float parabola = 1.0f - 4.0f * (prog2 - 0.5f) * (prog2 - 0.5f);
            Vector3 nextPos = Vector3.Lerp(startPos, targetPos, prog2);

            // Then add a vertical arc in excess of this.
            nextPos.y += parabola * height;

            // Continue as before.
            //mEnemy.transform.LookAt(nextPos, mEnemy.transform.forward);
            mEnemy.testFire.transform.position = nextPos;
            Debug.Log(prog2);


        }
        // local position means relative to parent(specific enemy object)
        if(mEnemy.testFire.transform.localPosition.y <= 0.10 && prog2 >= 1 && mEnemy.canEvade == true)
        {
            jumping = true;
            mEnemy.testFire.transform.localPosition = new Vector3(0, 0, 0);
        }

    }

    private void jumpArc()
    {
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

            //gravity cant be enabled when it starts, must be enabled shortly after
            if(progress > .1)
            {
                mEnemy.gameObject.GetComponent<Rigidbody>().useGravity = true;
            }
            // if enemy is in the air then it is not grounded
            if (progress < 1.0)
            {
                mEnemy.grounded = false;
            }
        }
    }

   


    
}
