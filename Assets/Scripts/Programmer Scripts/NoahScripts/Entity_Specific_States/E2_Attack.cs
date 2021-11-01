using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E2_Attack : AttackState
{
    private Enemy2 mEnemy;
    private bool attackStart;
    private bool attackEnd;
    public E2_Attack(Entity mEntity, FiniteStateMachine mStateMachine, D_Attack attackData, D_Entity entityData, Enemy2 mEnemy) : base(mEntity, mStateMachine, attackData, entityData)
    {
        this.mEnemy = mEnemy;
    }

    public override void StateEnter()
    {
        base.StateEnter();
        mEntity.agent.speed = 0;
        mEntity.agent.SetDestination(mEntity.myTarget.transform.position);
        Attack();
    }

    public override void StateExit()
    {
        Debug.Log("bye bye");
        base.StateExit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
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
        mEntity.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * 8f, ForceMode.Impulse);
        Vector3 dir = (mEntity.myTarget.transform.position - mEntity.transform.position).normalized;
        mEntity.gameObject.GetComponent<Rigidbody>().AddForce(dir * 20f, ForceMode.Impulse);
        attackStart = true;

    }
}
