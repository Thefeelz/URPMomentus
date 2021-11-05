using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E2_SlowApproach : SlowApproach
{
    private Enemy2 mEnemy;
    private float theta;
    private float x;
    private float y;
    private float z;
    private int rndTime;
    private bool circleStart;
    private Vector3 pos;
    //assign all variables
    public E2_SlowApproach(Entity mEntity, FiniteStateMachine mStateMachine, D_SlowApproach stateData, D_Entity entityData, Enemy2 mEnemy) : base(mEntity, mStateMachine, stateData, entityData)
    {
        
        this.mEnemy = mEnemy;
    }

    public override void StateEnter()
    {
        
        base.StateEnter();
        rndTime = Random.Range(6, 10);
        circleStart = false;
        float pX = mEnemy.myTarget.transform.position.x;
        float pZ = mEnemy.myTarget.transform.position.z;

        // sets the theta based on the quadrant that the enemy is in, in relation to the player
        if(mEnemy.transform.position.x <= pX && mEnemy.transform.position.z <= pZ)
        {
            theta = (3 * Mathf.PI) / 2;
        }
        else if (mEnemy.transform.position.x <= pX && mEnemy.transform.position.z > pZ)
        {
            theta = Mathf.PI;
        }
        else if (mEnemy.transform.position.x > pX && mEnemy.transform.position.z <= pZ)
        {
            theta = 0;
        } 
        else if (mEnemy.transform.position.x > pX && mEnemy.transform.position.z > pZ)
        {
            theta = Mathf.PI / 2;
        }

        
        x = Mathf.Cos(theta) * 5 + mEntity.myTarget.transform.position.x;
        y = mEntity.transform.position.y;
        z = Mathf.Sin(theta) * 5 + mEntity.myTarget.transform.position.z;
        pos.x = x;
        pos.y = y;
        pos.z = z;
        mEntity.agent.SetDestination(pos);
    }

    public override void StateExit()
    {
        base.StateExit();
    }


    public override void LogicUpdate()
    {
        base.LogicUpdate();
        // if the enemy is within ranged distance switch to aim state
       // if (mEntity.DistanceToPlayer() >= entityData.rapidDistance)
       // {
       //     mStateMachine.ChangeState(mEnemy.moveState);
       // }
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
        if(circleStart == false)
        {
            circleStart = true;
            mEnemy.invokeFunction(rndTime, "AttackEnter");
        }
        theta += .05f;
        if(theta == 360)
        {
            theta = 0;
        }
        x = Mathf.Cos(theta) * 5 + mEntity.myTarget.transform.position.x;
        y = mEntity.transform.position.y;
        z = Mathf.Sin(theta) * 5 + mEntity.myTarget.transform.position.z;
        pos.x = x;
        pos.y = y;
        pos.z = z;
        mEntity.agent.SetDestination(pos);
    }

}
