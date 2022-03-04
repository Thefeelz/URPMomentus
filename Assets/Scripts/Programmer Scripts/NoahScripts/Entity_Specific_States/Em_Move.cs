using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Em_Move : MoveState
{
    private Enemy_Melee mEnemy;
    public NavMeshPath pathToUse;
    public Queue<Vector3> cornerQueue;
    public Vector3 currentDestination;
    public Vector3 direction;
    public Vector3 targetPos;
    public bool hasPath;
    private float currentDistance;
    private float updateTime = 1f;
    private float lastUpdate;
    public Em_Move(Entity mEntity, FiniteStateMachine mStateMachine, D_moveState stateData, D_Entity entityData, Enemy_Melee mEnemy) : base(mEntity, mStateMachine, stateData, entityData)
    {
        this.mEnemy = mEnemy;
    }


    public override void StateEnter()
    {
        base.StateEnter();
        mEntity.mAnimator.SetBool("chasing", true);
        mEntity.mAnimator.SetBool("stationary", false);
        calculate();

    }

    public override void StateExit()
    {
        base.StateExit();
    }

    public override void LogicUpdate()
    {
        //mEnemy.agent.avoidancePriority = mEnemy.avoid;
        //Move();
        //if (Vector3.Distance(mEntity.agent.destination, mEntity.myTarget.transform.position) <= 1)
        //{
        //    mEntity.GetComponent<NavMeshAgent>().enabled = false;
        //    mEntity.GetComponent<NavMeshObstacle>().enabled = true;
        //}
        //else
        //{
        //    mEntity.GetComponent<NavMeshAgent>().enabled = true;
        //    mEntity.GetComponent<NavMeshObstacle>().enabled = false;
        //}
        if ((Vector3.Distance(mEnemy.mLocations.lSpots[mEnemy.spot], targetPos) > 1))
        {
            Debug.Log("Update!!");
            calculate();
        }

    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public void calculate()
    {
        hasPath = false;
        mEnemy.GetComponent<NavMeshObstacle>().enabled = false;
        mEnemy.GetComponent<NavMeshAgent>().enabled = true;
        targetPos = mEnemy.mLocations.lSpots[mEnemy.spot];
        pathToUse = new NavMeshPath();
        mEnemy.agent.CalculatePath(targetPos, pathToUse);
        cornerQueue = new Queue<Vector3>();
        int e = 0;
        foreach (var corner in pathToUse.corners)
        {
            if (e != 0)
            {
                cornerQueue.Enqueue(corner);
            }
            Debug.Log(corner);
            e++;
        }
        
        GetNextCorner();
        lastUpdate = Time.time;
        hasPath = true;
        mEnemy.GetComponent<NavMeshAgent>().enabled = false;
        mEnemy.GetComponent<NavMeshObstacle>().enabled = true;
    }

    private void GetNextCorner()
    {
        if (cornerQueue.Count > 0)
        {
            
            currentDestination = cornerQueue.Dequeue();
            direction = currentDestination - mEnemy.transform.position;
            mEnemy.transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
           
            direction = new Vector3(direction.x, 0, direction.z);
            
            hasPath = true;
        }
        else
        {
            hasPath = false;
        }
    }


    public override void Move()
    {
        //base.Move();
        //Locations l = mEnemy.mLocations;
        //float minDist = Mathf.Infinity;
        //for(int i = 0; i < 8; i++)
        //{
        //    if(l.lSpotsValid[i] && l.lSpotsTaken[i] == false)
        //    {
        //        float dist = Vector3.Distance(l.lSpots[i], mEnemy.transform.position);
        //        if ( dist < minDist)
        //        {
        //            minDist = dist;
        //            mEnemy.agent.SetDestination(l.lSpots[i]);
        //        }
        //    }
        //}
        if (hasPath && mEnemy.hasTarget && mEnemy.spot != 9)
        {
            currentDistance = Vector3.Distance(mEnemy.transform.position, currentDestination);
            if (currentDistance > 1)
                mEnemy.transform.position += direction * 0.4f * Time.deltaTime;
            else
            {
                if (Vector3.Distance(mEnemy.transform.position, targetPos) >= 1)
                {
                    GetNextCorner();
                }
                else
                {
                    //calculate();
                }
            }
        }

    }
}
