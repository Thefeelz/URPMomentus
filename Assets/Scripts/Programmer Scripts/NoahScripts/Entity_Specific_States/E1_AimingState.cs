using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        mEnemy.canon.transform.LookAt(mEntity.myTarget.transform);
        base.Aim();
    }

    public override void StateEnter()
    {
        base.StateEnter();
    }

    public override void StateExit()
    {
        base.StateExit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        Aim();
        if(mEnemy.canShoot)
        {
            mEnemy.canShoot = false;
            Shoot();
        }
        // switch states based on player distance if needed
        if (mEntity.DistanceToPlayer() <= entityData.evadeDistance)
        {
            mStateMachine.ChangeState(mEnemy.evadeState);
        }
        if (mEntity.DistanceToPlayer() >= entityData.rapidDistance)
        {
            mStateMachine.ChangeState(mEnemy.moveState);
        }
    }

   

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
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
        bullet.GetComponent<Rigidbody>().AddForce(mEnemy.canon.transform.forward * 2000);
        mEnemy.StartCool(bullet);
    }

    
}
