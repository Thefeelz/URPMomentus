using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E2_SlowApproach : SlowApproach
{
    private Enemy2 mEnemy;
    private float theta;
    private float x; // current x
    private float y; // current y
    private float z; // current z
    private bool startHigher;
    //test variables
    private Vector3 targetDir;
    private float tX; // target x
    private float tY; // target y
    private float tZ; // target z
    private Vector3 tPos; // target position vec3
    //end test
    private int rndTime; // random amount of time
    private Vector3 pos;
    //assign all variables
    public E2_SlowApproach(Entity mEntity, FiniteStateMachine mStateMachine, D_SlowApproach slowData, D_Entity entityData, Enemy2 mEnemy) : base(mEntity, mStateMachine, slowData, entityData)
    {
        
        this.mEnemy = mEnemy;
    }

    public override void StateEnter()
    {
        
        base.StateEnter();
        rndTime = Random.Range(4,5);
        slowData.circleStart = false;
        // get the vector that goes from the player to the enemy and ignore its y
        targetDir = mEnemy.transform.position - mEnemy.myTarget.transform.position;
        targetDir.y = 0;
        //t is a vector that is always facing straight along the z axis
        Vector3 t = new Vector3(0, 0, 1);
        // this gets us an angle between the two vectors and adds 90 to it for some weird, but neccessary reason
        float angle = Vector3.Angle(targetDir, t) + 90;
        // converts the angle to radians as that is what Mathf uses
        theta = angle * (Mathf.PI / 180);
        // if the enemy started lower than the player then we need to multiple the cos of theta by negative one
        if (mEnemy.transform.position.x > mEnemy.myTarget.transform.position.x)
        {
            x = (Mathf.Cos(theta) * 5) * -1 + mEnemy.myTarget.transform.position.x;
            startHigher = true;
        }
        else
        {
             x = Mathf.Cos(theta) * 5 + mEnemy.myTarget.transform.position.x;
            startHigher = false;
        }
        // assigns the target position to the calculated x y and z
        y = mEnemy.transform.position.y;
        z = Mathf.Sin(theta) * 5 + mEnemy.myTarget.transform.position.z ;
        pos.x = x;
        pos.y = y;
        pos.z = z;
        mEnemy.agent.SetDestination(pos);
        Debug.Log(pos);
    }

    public override void StateExit()
    {
        base.StateExit();
    }


    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(Vector3.Distance(mEntity.transform.position, pos) <= .5 && Vector3.Distance(mEntity.transform.position, pos) >= -.5)
        {
            setNewSpot();
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

    private void setNewSpot()
    {
        if(slowData.circleStart == false)
        {
            slowData.circleStart = true;
            mEnemy.callCoroutine("attackState", rndTime);
        }
        // formula to keep moving in a circle
        // if we started lower than the player than theta must decrease
        if(startHigher == true)
        {
            theta -= .05f;
            x = (Mathf.Cos(theta) * 5) * -1 + mEntity.myTarget.transform.position.x;
            tX = (Mathf.Cos(theta + Mathf.PI) * 10) * -1 + mEntity.myTarget.transform.position.x;
        }
        else
        {
            theta += .05f;
            x = Mathf.Cos(theta) * 5 + mEntity.myTarget.transform.position.x;
            tX = Mathf.Cos(theta + Mathf.PI) * 10 + mEntity.myTarget.transform.position.x;
        }
        y = mEntity.transform.position.y;
        z = Mathf.Sin(theta) * 5 + mEntity.myTarget.transform.position.z;
        pos.x = x;
        pos.y = y;
        pos.z = z;
        // duplicating for target but adding pi. the target is where the enemy will jump towards when it attacks
        tY = mEntity.transform.position.y;
        tZ = Mathf.Sin(theta + Mathf.PI) * 10 + mEntity.myTarget.transform.position.z;
        tPos.x = tX;
        tPos.y = tY;
        tPos.z = tZ;
        mEnemy.targPos = tPos;
        mEntity.agent.SetDestination(pos);
    }

   

}
