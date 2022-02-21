using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ej_Melee : MeleeState
{
    Enemy_Jumper mEnemy;

    public Ej_Melee(Entity mEntity, FiniteStateMachine mStateMachine, D_Entity entityData, Enemy_Jumper mEnemy) : base(mEntity, mStateMachine, entityData)
    {
        this.mEnemy = mEnemy;
    }

    public override void StateEnter()
    {
        base.StateEnter();
        mEnemy.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
        mEntity.facePlayer();
        mEntity.mAnimator.SetBool("chasing", false);
        mEntity.mAnimator.SetBool("meleeAttack", true);
        start = false;
        Strike();

    }

    public override void StateExit()
    {
        base.StateExit();
        mEnemy.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
        mEntity.mAnimator.SetBool("meleeAttack", false);
        mEntity.mAnimator.SetBool("chasing", false);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (start == true && Time.time - strikeStart >= 2)
        {
            mEnemy.stateMachine.ChangeState(mEnemy.moveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

    }

    public override void Strike()
    {
        strikeStart = Time.time;
        start = true;
        RaycastHit send;
        Physics.Raycast(mEntity.transform.position + Vector3.up + (mEntity.transform.forward * 0.5f), mEntity.transform.forward, out send, 1f);
        if (send.collider != null && send.collider.GetComponentInParent<CharacterStats>())
        {
            send.transform.GetComponentInParent<P_Movement>().enabled = false;
            send.transform.GetComponentInParent<P_Input>().enabled = false;
        }
    }


}
