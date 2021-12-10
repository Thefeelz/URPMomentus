using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Entity : MonoBehaviour
{
    public Pooler myPool; // object pooler object
    public NavMeshAgent agent;// navmesh agent
    public GameObject myTarget; // navMesh target. Normally the player is assigned to this
    public State defaultState;

    public FiniteStateMachine stateMachine { get; private set; } // statemachine used by this entity
    public float health { get; private set; } // how much health entity has

    [SerializeField]
    protected D_Entity entityData; // data file for entity variables

    private SkinnedMeshRenderer mMesh; // objects mesh
    private Color mColor; // original color of the mesh
    public bool grounded;
    public GameObject testFire; // a test fire object to detect collision
    public SpecialUseState specialUseState; // special use state

    public EnemyStats mEnemyStats; // brandons script that keeps track of certain aspects of the enemy

    public virtual void Awake()
    {
        //variables are assigned when object awakes
        myTarget = GameObject.FindWithTag("Player"); 
        stateMachine = new FiniteStateMachine(); 
        health = entityData.health; 
        mMesh = gameObject.GetComponentInChildren<SkinnedMeshRenderer>(); 
        mColor = mMesh.material.color;
        specialUseState = new SpecialUseState(this, this.stateMachine);
        
        
    }
    // update is called once per frame
    public virtual void Update()
    {
        // performs a logic update in the current state
        stateMachine.currentState.LogicUpdate();
        // a debug command to test damage
        if (Input.GetKeyDown(KeyCode.K))
        {
            Damage(1f);
        }

        
    }
    // physics update
    public virtual void FixedUpdate()
    {
        //performs a fixed update in the current state
        stateMachine.currentState.PhysicsUpdate();
        
    }
    // gets the distance to the player
    public virtual float DistanceToPlayer()
    {
        return Vector3.Distance(transform.position, myTarget.transform.position);
    }
    // gets the distance to a specific position
    public virtual float DistanceToPosition(Vector3 mPosition)
    {
        return Vector3.Distance(transform.position, mPosition);
    }

    //the entitys health decreases and its mesh flashes red
    public virtual void Damage(float amountDamage)
    {
        health -= amountDamage;
        if(mMesh)
            mMesh.material.color = Color.red;
        Invoke("ResetColor", .5f);
    }

   //resets mesh color
    private void ResetColor()
    {
        mMesh.material.color = mColor;
    }
    //called when health reaches 0 all functionality is done within children
    public virtual void Die()
    {
        agent.speed = 0;
       
    }

    public void OnCollisionEnter(Collision collision)
    {
        // IMPORTANT! TEST NAME ONLY! WILL NOT ALWAYS BE NAMED FLOOR! USE TAG INSTEAD!!!
        if (collision.gameObject.tag == "floor")
        {
            grounded = true;
        }
    }

    // makes the enemy rotate to face the player if it is needed
    public void facePlayer()
    {
       
        var lookPos = myTarget.transform.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = rotation;
    }



}
