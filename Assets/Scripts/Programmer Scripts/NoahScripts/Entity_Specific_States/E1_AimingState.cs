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
    private bool shooting;
    private float shootTime = 2;
    private float shootStart;
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


    ///see about enabling disabled stuff in function, might be needed
    public override void StateEnter()
    {
        base.StateEnter();
        mEnemy.agent.speed = aimData.moveSpeed;
       
        
        //mEnemy.gameObject.GetComponent<NavMeshAgent>().enabled = false;
        //mEnemy.gameObject.GetComponent<NavMeshObstacle>().enabled = true;
    }

    public override void StateExit()
    {
        base.StateExit();
        
        mEnemy.gameObject.GetComponent<NavMeshObstacle>().enabled = false;
        mEnemy.gameObject.GetComponent<NavMeshAgent>().enabled = true;
        mEntity.mAnimator.SetBool("rangeAttack", false); // extra precaution in case rangeAttack is still true when leaving state


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
        // switch states based on player distance and cool down when needed
        if (mEntity.DistanceToPlayer() <= entityData.evadeDistance && mEnemy.canEvadeState == true)
        {
            //starts the evade cooldown in the enemy class if enemy can evade
            mEnemy.evadeTime = Time.time;
            mEnemy.canEvadeState = false;
            mStateMachine.ChangeState(mEnemy.evadeState);
        }
        else if (mEntity.DistanceToPlayer() >= entityData.rapidDistance)
        {
            mStateMachine.ChangeState(mEnemy.moveState);
        }
        //timer for shooting animation
        if(shooting == true)
        {
            if(Time.time >= shootStart + shootTime)
            {
                shooting = false;
                mEntity.mAnimator.SetBool("rangeAttack", false);
                mEntity.mAnimator.SetBool("chasing", true);
            }
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
        mEntity.mAnimator.SetBool("rangeAttack", true);
        mEntity.mAnimator.SetBool("chasing", false);
        shooting = true;
        shootStart = Time.time;
        Quaternion enemyRotation = mEnemy.transform.rotation; // enemy faces player
        bullet = mEnemy.ammo.dequeBullet(); //deques bullet
        bullet.transform.position = mEnemy.canon.transform.position + (mEnemy.transform.forward * 1.2f); // places bullet infront of cannon, not perfect yet
        bullet.transform.rotation = enemyRotation; // bullet faces player
        bullet.SetActive(true); //bullet is set to active
        //GameObject bullet = GameObject.Instantiate(mEnemy.bulletObj, mEnemy.canon.transform.position + (mEnemy.transform.forward * 1.2f), enemyRotation);
        bullet.GetComponent<Rigidbody>().AddForce((mEnemy.myTarget.transform.position - mEnemy.transform.position) * aimData.bulletSpeed); // bullet force
        mEnemy.StartCool(bullet); // cooldown for when it can shoot
    }

    
}
