using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E2_Attack : AttackState
{
    private Enemy2 mEnemy;
    private bool attackStart;
    private bool attackEnd;
    private bool topArc;


    private float progress;
    private float step;
    private float speed;
    private float height;
    private float distance;
    private Vector3 startPos;
    private Vector3 tPos;
    public E2_Attack(Entity mEntity, FiniteStateMachine mStateMachine, D_Attack attackData, D_Entity entityData, Enemy2 mEnemy) : base(mEntity, mStateMachine, attackData, entityData)
    {
        this.mEnemy = mEnemy;
    }

    public override void StateEnter()
    {
        base.StateEnter();
        Debug.Log(mEnemy.targPos);
        topArc = false;
        attackStart = false;
        mEntity.agent.speed = 0;
        mEntity.agent.SetDestination(mEntity.myTarget.transform.position);
        progress = 0;
        //test shit
        speed = 3;
        height = 2;
        distance = mEntity.DistanceToPlayer();
        step = speed / distance;
        startPos = mEnemy.transform.position;
        tPos = mEnemy.targPos;


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
            //mEnemy.callCoroutine("moveState", 3f);
        }

        progress = Mathf.Min(progress + Time.deltaTime * step, 1.0f);
        float parabola = 1.0f - 4.0f * (progress - 0.5f) * (progress - 0.5f);
        Vector3 nextPos = Vector3.Lerp(startPos, tPos, progress);

        // Then add a vertical arc in excess of this.
        nextPos.y += parabola * height;

        // Continue as before.
        //mEnemy.transform.LookAt(nextPos, mEnemy.transform.forward);
        mEnemy.transform.position = nextPos;
        if(progress == 1.0)
        {
            mEnemy.stateMachine.ChangeState(mEnemy.moveState);
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
        //mEntity.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //mEntity.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * 3f, ForceMode.Impulse);
        //Vector3 dir = (mEntity.myTarget.transform.position - mEntity.transform.position).normalized;
        //mEntity.gameObject.GetComponent<Rigidbody>().AddForce(dir * 10f, ForceMode.Impulse);
        attackStart = true;

    }
}
