using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E3_Patrol : PatrolState
{
    private Enemy3 mEnemy; // the enemy
    private float speed; // speed enemy moves
    private float step; // used to calculate how far to move each frame
    private float theta; // degrees
    private float x; // x position for target
    private float y; // y position for target
    private float z; // z position for target
    private Vector3 pos; // target position
    private bool patrol = true; // bool for if it is or isnt patroling
    private Vector3 vectorD; // vector to find distance
    private float distance; // distance to target, calculated differently so it has its own variable 
    private bool targetSet = false;
    private Vector3 divePos;

    public E3_Patrol(Entity mEntity, FiniteStateMachine mStateMachine, D_Patrol patrolData, D_Entity entityData, Enemy3 mEnemy) : base(mEntity, mStateMachine, patrolData, entityData)
    {
        speed = 10;
        this.mEnemy = mEnemy;
    }
    public override void StateEnter()
    {
        
        base.StateEnter();
        // default position setter
        theta = 0;
        x = Mathf.Cos(theta) * patrolData.radius + mEnemy.anchor.transform.position.x;
        y = mEntity.transform.position.y;
        z = Mathf.Sin(theta) * patrolData.radius + mEnemy.anchor.transform.position.z;
        pos.x = x;
        pos.y = y;
        pos.z = z;
        
    }

    public override void StateExit()
    {
        Debug.Log("adios");
        base.StateExit();
    }
    public override void LogicUpdate()
    {

        step = speed * Time.deltaTime;
        // update position if patrol is true
        if (patrol == true)
        {
            mEntity.transform.position = Vector3.MoveTowards(mEntity.transform.position, pos, step);
            base.LogicUpdate();
            if (Vector3.Distance(mEntity.transform.position, pos) <= .5 && Vector3.Distance(mEntity.transform.position, pos) >= -.5)
            {
                setNewSpot();
            }
        }
        //dive bomb if patrol is fale
        else
        {
            mEntity.transform.position = Vector3.MoveTowards(mEntity.transform.position, divePos, step);
            if(mEntity.DistanceToPosition(divePos) <= 1)
            {
                mEnemy.gameObject.SetActive(false); 
            }
        }

        //calculates distance to player while ignoring Y, when close enough it will dive bomb
        vectorD = mEntity.myTarget.transform.position - mEntity.transform.position;
        vectorD.y = 0;
        distance = vectorD.magnitude;
        if(distance <= 5 && targetSet == false)
        {
            divePos = mEntity.myTarget.transform.position;
            patrol = false;
            targetSet = true;
        }


        

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private void setNewSpot()
    {
        // TODO: Maybe make it a way point system not circle
        theta += .05f;
        if (theta == 360)
        {
            theta = 0;
        }
        x = Mathf.Cos(theta) * patrolData.radius + mEnemy.anchor.transform.position.x;
        y = mEntity.transform.position.y;
        z = Mathf.Sin(theta) * patrolData.radius + mEnemy.anchor.transform.position.z;
        pos.x = x;
        pos.y = y;
        pos.z = z;
        
    }
}
