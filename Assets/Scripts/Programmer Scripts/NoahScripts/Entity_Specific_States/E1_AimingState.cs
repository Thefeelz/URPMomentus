using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class E1_AimingState : AimingState
{
    Enemy1 mEnemy;
    private Transform shotPoint;
    public GameObject bullet;
    public E1_AimingState(Entity mEntity, FiniteStateMachine mStateMachine, D_Aiming aimData, D_Entity entityData, Enemy1 mEnemy) : base(mEntity, mStateMachine, aimData, entityData)
    {
        this.mEnemy = mEnemy;
    }

    public override void Aim()
    {
        //mEnemy.canon.transform.LookAt(mEnemy.myTarget.transform);
        //mEnemy.transform.LookAt(mEnemy.myTarget.transform);
        base.Aim();
        mEntity.agent.SetDestination(mEntity.myTarget.transform.position);
        //Debug.Log(mEnemy.myTarget.transform.position);
    }

    public override void StateEnter()
    {
        base.StateEnter();
        mEnemy.agent.speed = 3;
       
        //mEnemy.gameObject.GetComponent<NavMeshAgent>().enabled = false;
        //mEnemy.gameObject.GetComponent<NavMeshObstacle>().enabled = true;
    }

    public override void StateExit()
    {
        base.StateExit();
        mEnemy.gameObject.GetComponent<NavMeshObstacle>().enabled = false;
        mEnemy.gameObject.GetComponent<NavMeshAgent>().enabled = true;
        
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        // debug
        Vector3 path = mEnemy.transform.TransformDirection(Vector3.forward * -1) * 10;
        Debug.DrawRay(mEnemy.transform.position, path, Color.green);
        // end debug
        Aim();
        if(mEnemy.canShoot)
        {
            mEnemy.canShoot = false;
            Shoot();
        }
        // switch states based on player distance if needed
        if (mEntity.DistanceToPlayer() <= entityData.evadeDistance && mEnemy.canEvade == true)
        {
            mEnemy.canEvade = false;
            mStateMachine.ChangeState(mEnemy.evadeState);
        }
        else if (mEntity.DistanceToPlayer() >= entityData.rapidDistance)
        {
            mStateMachine.ChangeState(mEnemy.moveState);
        }
    }

   

    public override void PhysicsUpdate()
    {
        
        //mEntity.agent.updateRotation = true;
        //mEntity.agent.SetDestination(mEntity.myTarget.transform.position);
    }

    private void Shoot()
    {
        //deques the front bullet in the ammo pool and resets its location to the canon of the enemy, and adds force to fire it
        Quaternion enemyRotation = mEnemy.transform.rotation;
        bullet = mEnemy.ammo.dequeBullet();
        bullet.transform.position = mEnemy.canon.transform.position + (mEnemy.transform.forward * 1.2f);
        bullet.transform.rotation = enemyRotation;
        bullet.SetActive(true);
        //GameObject bullet = GameObject.Instantiate(mEnemy.bulletObj, mEnemy.canon.transform.position + (mEnemy.transform.forward * 1.2f), enemyRotation);
        bullet.GetComponent<Rigidbody>().AddForce(mEnemy.canon.transform.forward * dAimData.bulletSpeed);
        mEnemy.StartCool(bullet);
    }

    
}
