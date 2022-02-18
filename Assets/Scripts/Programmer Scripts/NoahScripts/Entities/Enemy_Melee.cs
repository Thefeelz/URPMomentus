using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Melee : Entity
{
    public bool linking; // linking stuff
    public float origSpeed = 3; // linking stuff
    public float linkSpeed = 1; // linking stuff
    
    //states
    public DeathState deathState { get; private set; }
    public Em_Move moveState { get; private set; }
    public Em_Melee meleeState { get; private set; }

    //Datas
    [SerializeField]
    private D_moveState moveData;

    public override void Awake()
    {
        base.Awake();
        deathState = new DeathState(this, stateMachine);
        moveState = new Em_Move(this, stateMachine, moveData, entityData, this);
        meleeState = new Em_Melee(this, stateMachine, entityData, this);
        stateMachine.InitializeStateMachine(moveState);
        defaultState = moveState;
        queueName = "Melee";

    }
    public override void Update()
    {
        base.Update();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        // controls the enemies speed when jumping between links
        if (agent.isOnOffMeshLink && linking == false)
        {
            linking = true;
            agent.speed = agent.speed * linkSpeed;
        }
        else if (agent.isOnNavMesh && linking == true)
        {
            linking = false;
            agent.velocity = Vector3.zero;
            agent.speed = origSpeed;
        }
    }

   

    public override void Damage(float amountDamage)
    {
        base.Damage(amountDamage);

        if (health <= 0)
        {
            stateMachine.ChangeState(deathState);
        }
    }

    


}
