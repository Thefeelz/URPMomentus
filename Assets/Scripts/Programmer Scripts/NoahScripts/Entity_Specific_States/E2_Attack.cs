using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E2_Attack : AttackState
{
    private Enemy2 mEnemy;
    private bool attackStart;
    private bool attackEnd;
    private bool topArc;
    public E2_Attack(Entity mEntity, FiniteStateMachine mStateMachine, D_Attack attackData, D_Entity entityData, Enemy2 mEnemy) : base(mEntity, mStateMachine, attackData, entityData)
    {
        this.mEnemy = mEnemy;
    }

    public override void StateEnter()
    {
        base.StateEnter();
        topArc = false;
        attackStart = false;
        mEntity.agent.speed = 0;
        mEntity.agent.SetDestination(mEntity.myTarget.transform.position);
        Attack();
    }

    public override void StateExit()
    {
        mEnemy.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        mEnemy.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        Debug.Log("bye bye");
        base.StateExit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(attackStart == true)
        {
            attackStart = false;
            mEnemy.callCoroutine("moveState", 3f);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    protected override void Attack()
    {
        // STILL TESTING
        base.Attack();
        mEntity.agent.enabled = false;
        mEntity.GetComponent<Rigidbody>().velocity = Vector3.zero;
        mEntity.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * 3f, ForceMode.Impulse);
        Vector3 dir = (mEntity.myTarget.transform.position - mEntity.transform.position).normalized;
        mEntity.gameObject.GetComponent<Rigidbody>().AddForce(dir * 10f, ForceMode.Impulse);
        attackStart = true;

    }
}
