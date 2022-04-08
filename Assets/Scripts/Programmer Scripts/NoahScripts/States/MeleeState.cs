using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeState : State
{
    protected D_Entity entityData;
    protected bool start;
    protected float strikeStart;
    public MeleeState(Entity mEntity, FiniteStateMachine mStateMachine, D_Entity entityData) : base(mEntity, mStateMachine)
    {
        this.entityData = entityData;
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
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public virtual void Strike()
    {
        strikeStart = Time.time;
        start = true;
        RaycastHit send;
        Physics.Raycast(mEntity.transform.position + Vector3.up + (mEntity.transform.forward * 0.5f), mEntity.transform.forward, out send, 1.5f);
        if (send.collider != null && send.collider.GetComponentInParent<CharacterStats>())
        {
            send.transform.GetComponentInParent<CharacterStats>().RemoveHealthMelee(2f);

        }
    }
}
