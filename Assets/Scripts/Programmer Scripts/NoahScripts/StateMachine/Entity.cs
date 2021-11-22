using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Entity : MonoBehaviour
{
    public Pooler myPool; // object pooler object
    public NavMeshAgent agent;// navmesh agent
    public GameObject myTarget; // navMesh target. Normally the player is assigned to this

    public FiniteStateMachine stateMachine { get; private set; } // statemachine used by this entity
    public float health { get; private set; } // how much health entity has

    [SerializeField]
    protected D_Entity entityData; // data file for entity variables

    private SkinnedMeshRenderer mMesh; // objects mesh
    private Color mColor; // original color of the mesh

    public virtual void Awake()
    {
        //variables are assigned when object awakes
        myTarget = GameObject.FindWithTag("Player"); 
        stateMachine = new FiniteStateMachine(); 
        health = entityData.health; 
        mMesh = gameObject.GetComponentInChildren<SkinnedMeshRenderer>(); 
        mColor = mMesh.material.color;
        
        
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
        
       
    }



}
