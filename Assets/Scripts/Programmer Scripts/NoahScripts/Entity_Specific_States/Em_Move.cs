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
    LineRenderer line;
    public Em_Move(Entity mEntity, FiniteStateMachine mStateMachine, D_moveState stateData, D_Entity entityData, Enemy_Melee mEnemy) : base(mEntity, mStateMachine, stateData, entityData)
    {
        this.mEnemy = mEnemy;
        line = mEnemy.GetComponent<LineRenderer>();
    }


    public override void StateEnter()
    {
        //base.StateEnter();
        mEntity.mAnimator.SetBool("chasing", true);
        mEntity.mAnimator.SetBool("stationary", false);
        Calculate();

    }

    public override void StateExit()
    {
        base.StateExit();
    }

    public override void LogicUpdate()
    {
        if (Vector3.Distance(targetPos, mEnemy.mLocations.lSpots[mEnemy.spot]) > 1) // if the target position moved then calculate again
        {
            Calculate();
        }
        if (Vector3.Distance(mEnemy.transform.position, targetPos) < 1) // switch to melee if close enough
        {
            mEnemy.stateMachine.ChangeState(mEnemy.meleeState);
        }
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
        //if ((Vector3.Distance(mEnemy.mLocations.lSpots[mEnemy.spot], targetPos) > 1))
        //{
        //    Debug.Log("Update!!");
        //    calculate();
        //}

    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        RaycastHit hit;
        Debug.DrawRay(mEnemy.transform.position + Vector3.up + (mEnemy.transform.forward), mEnemy.transform.forward * 2, Color.green); //debug line to show raycast
        Physics.Raycast(mEntity.transform.position + Vector3.up + (mEntity.transform.forward * 0.5f), mEntity.transform.forward, out hit, 2f); // ray cast to see if it will collide with enemy
        if (hit.collider != null && hit.collider.GetComponentInParent<EnemyStats>()) // if enemy is about to collide with another it will wait until the path is clear
        {
            Debug.LogWarning("Get out of the way!");
            Calculate();
        }
        else // if not it moves
        {
            Move();
        }
        Debug.Log(currentDestination);
        //mEnemy.targetPos = targetPos;
    }



    public void Calculate()
    {
        if (mEnemy.hasTarget && mEnemy.spot != 9) // if the enemy has a valid position it will go there
        {
            mEnemy.GetComponent<NavMeshObstacle>().enabled = false; // disable obstacle
            mEnemy.GetComponent<NavMeshAgent>().enabled = true; //enable agent
            NavMeshPath p = new NavMeshPath(); //make new path
            mEnemy.agent.CalculatePath(mEnemy.mLocations.lSpots[mEnemy.spot], p); // calculate how to get to target
            targetPos = mEnemy.mLocations.lSpots[mEnemy.spot]; // set overall target pos to the set location around player
            if (mEnemy.GetComponent<NavMeshAgent>().enabled == true)
            {
                mEnemy.line.positionCount = mEnemy.agent.path.corners.Length;
                mEnemy.line.SetPositions(mEnemy.agent.path.corners);
            }
            cornerQueue = new Queue<Vector3>(); // make a queue to hold corners
            foreach (var corner in p.corners) // for each corner in our path we will put it in the queue
            {
                cornerQueue.Enqueue(corner);
            }
            NextCorner();
            mEnemy.GetComponent<NavMeshAgent>().enabled = false;
            mEnemy.GetComponent<NavMeshObstacle>().enabled = true;
        }
    }

    void NextCorner()
    {
        if (cornerQueue.Count > 0) // if there is a next corner
        {
            currentDestination = cornerQueue.Dequeue(); // deque a corner
            direction = currentDestination - mEnemy.transform.position; // direciton to destination
            direction = new Vector3(direction.x, mEnemy.transform.position.y, direction.z); // ignore the y of that direction, no need to fly
            if (Vector3.Distance(currentDestination, mEnemy.transform.position) > 1) //rotate to face if its not gonna be a short distance
                mEnemy.transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            hasPath = true; // there is a path

        }
        else // if not, calculate it again
        {

            hasPath = false; // there is not a path
                             //Calculate(); // time to calculate

        }
    }

    public override void Move()
    {
        if (hasPath == true) // only move if we have a path
        {
            if (Vector3.Distance(mEnemy.transform.position, currentDestination) < 0.5f)
            {
                NextCorner();
            }
            else
            {
                mEnemy.transform.position = Vector3.MoveTowards(mEnemy.transform.position, currentDestination, 3f * Time.deltaTime); // move towards
            }
        }
    }
}









// DEAD CODE THAT MAY HAVE INFO TO HELP


//void DrawPath(NavMeshPath p)
//{
//    line.positionCount = p.corners.Length;
//    line.SetPositions(p.corners);
//}

//public void calculate()
//{
//    hasPath = false;
//    mEnemy.GetComponent<NavMeshObstacle>().enabled = false;
//    mEnemy.GetComponent<NavMeshAgent>().enabled = true;
//    targetPos = mEnemy.mLocations.lSpots[mEnemy.spot];
//    pathToUse = new NavMeshPath();
//    mEnemy.agent.CalculatePath(targetPos, pathToUse);
//    cornerQueue = new Queue<Vector3>();
//    int e = 0;
//    foreach (var corner in pathToUse.corners)
//    {


//        cornerQueue.Enqueue(corner);

//        Debug.Log(corner);
//        e++;
//    }

//    GetNextCorner();
//    lastUpdate = Time.time;
//    hasPath = true;
//    mEnemy.GetComponent<NavMeshAgent>().enabled = false;
//    mEnemy.GetComponent<NavMeshObstacle>().enabled = true;
//}

//private void GetNextCorner()
//{
//    if (cornerQueue.Count > 0) // gets the location of the next corner and rotates to face it
//    {

//        currentDestination = cornerQueue.Dequeue();
//        direction = currentDestination - mEnemy.transform.position;
//        if (Vector3.Distance(currentDestination, mEnemy.transform.position) > 1)
//            mEnemy.transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
//        direction = new Vector3(direction.x, 0, direction.z);

//        hasPath = true;
//    }
//    else
//    {
//        hasPath = false;
//    }
//    //Debug.Log(Vector3.Distance(new Vector3(currentDestination.x, 0f, currentDestination.z), new Vector3(mEnemy.transform.position.x, 0f, mEnemy.transform.position.z)));
//    //if(hasPath == true && Vector3.Distance(new Vector3(currentDestination.x, 0f, currentDestination.z), new Vector3(mEnemy.transform.position.x, 0f, mEnemy.transform.position.z)) < 1) // if the length to the next corner is less than one unit, it is skipped
//    //{
//    //    GetNextCorner();
//    //}
//}


//public override void Move()
//{
//    ////        //base.Move();
//    //Locations l = mEnemy.mLocations;
//    //float minDist = Mathf.Infinity;
//    //for(int i = 0; i < 8; i++)
//    //{
//    //    if(l.lSpotsValid[i] && l.lSpotsTaken[i] == false)
//    //    {
//    //        float dist = Vector3.Distance(l.lSpots[i], mEnemy.transform.position);
//    //        if ( dist < minDist)
//    //        {
//    //            minDist = dist;
//    //            mEnemy.agent.SetDestination(l.lSpots[i]);
//    //        }
//    //    }
//    //}
//    if (hasPath && mEnemy.hasTarget && mEnemy.spot != 9) // if the enemy has a valid position it will go there
//    {
//        currentDistance = Vector3.Distance(mEnemy.transform.position, currentDestination);
//        if (currentDistance > 1)
//            mEnemy.transform.position += direction * 1 * Time.deltaTime;
//        else
//        {
//            if (Vector3.Distance(mEnemy.transform.position, targetPos) >= 1)
//            {
//                GetNextCorner();
//            }
//            else
//            {
//                mEnemy.stateMachine.ChangeState(mEnemy.meleeState);
//            }
//        }
//    }


//}