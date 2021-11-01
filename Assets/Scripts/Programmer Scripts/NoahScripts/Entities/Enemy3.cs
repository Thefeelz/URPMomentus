using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : Entity
{
    public E3_Patrol patrolState { get; private set; }
    public GameObject anchor;
    [SerializeField]
    private D_Patrol patrolData;

    public override void Awake()
    {
        base.Awake();

        patrolState = new E3_Patrol(this, stateMachine, patrolData, entityData, this);

        stateMachine.InitializeStateMachine(patrolState);
    }

    public override void Damage(float amountDamage)
    {
        base.Damage(amountDamage);
    }

    public override float DistanceToPlayer()
    {
        return base.DistanceToPlayer();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();
    }
}
