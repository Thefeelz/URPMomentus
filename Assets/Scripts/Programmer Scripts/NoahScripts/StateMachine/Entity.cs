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
    public bool specialUseBool;
    public bool grounded;
    public GameObject testFire; // a test fire object to detect collision
    public SpecialUseState specialUseState; // special use state
    public Animator mAnimator;
    public float elapsedMaterializeTime = 0f;
    public List<Material> mesh = new List<Material>();
    public bool materializing = true;
    [SerializeField] float materializeTime;

    public float health { get; private set; } // how much health entity has
    public FiniteStateMachine stateMachine { get; private set; } // statemachine used by this entity

    [SerializeField]
    protected D_Entity entityData; // data file for entity variables
    private SkinnedMeshRenderer mMesh; // objects mesh
    private Color mColor; // original color of the mesh
    public GameObject bipedBody;


    //public EnemyStats mEnemyStats; // brandons script that keeps track of certain aspects of the enemy
    public string queueName; // the string that is used to enque


    public virtual void Awake()
    {
        //variables are assigned when object awakes
        myTarget = GameObject.FindWithTag("Player"); 
        stateMachine = new FiniteStateMachine(); 
        health = entityData.health; 
        mMesh = gameObject.GetComponentInChildren<SkinnedMeshRenderer>(); 
        mAnimator = gameObject.GetComponentInChildren<Animator>(); 
        mColor = mMesh.material.color;
        specialUseState = new SpecialUseState(this, this.stateMachine);
        //mEnemyStats = this.gameObject.GetComponent<EnemyStats>();
        foreach (Transform item in bipedBody.transform)
        {
            mesh.Add(item.GetComponent<SkinnedMeshRenderer>().material);
        }

    }
    // update is called once per frame
    public virtual void Update()
    {
        // performs a logic update in the current state
        stateMachine.currentState.LogicUpdate();
        // Enemy generates
        if (materializing == true)
        {
            MaterializeIn();
        }   
    }
    // physics update in current state
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
        if(health <= 0)
        {
            StartCoroutine(DeathAnim());
        }
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
        if (collision.gameObject.tag == "floor")
        {
            grounded = true;
        }
    }

    // makes the enemy rotate to face the player if it is needed
    public void facePlayer()
    {

        //var lookPos = myTarget.transform.position - transform.position;
        //lookPos.y = 0;
        //var rotation = Quaternion.LookRotation(lookPos);
        //transform.rotation = rotation;
        transform.LookAt(myTarget.transform.position);
        var rotVec = transform.rotation.eulerAngles;
        rotVec.z = 0;
        rotVec.x = 0;
        transform.rotation = Quaternion.Euler(rotVec);
    }


    // kills the enemy after a delay to allow an animation
    IEnumerator DeathAnim()
    {
        yield return new WaitForSeconds(3f);
        myPool.queueObject(queueName, this.gameObject);
    }


    public void resetAll()
    {
        health = entityData.health;
        stateMachine.ChangeState(defaultState);
        elapsedMaterializeTime = 0f;
        MaterializeIn();
    }

    public void MaterializeIn()
    {
        elapsedMaterializeTime += Time.deltaTime;
        Debug.Log("something");
        foreach (Material mat in mesh)
        {
            Debug.Log("something");
            mat.SetFloat("Vector1_25be2060a07040ad90d1716c35083360", Mathf.Lerp(1.2f, -0.2f, elapsedMaterializeTime / materializeTime));
        }
        if (elapsedMaterializeTime >= materializeTime)
        {
            GetComponent<EnemyStats>().SetAbleToBeAttacked(true);
            elapsedMaterializeTime = 0f;
            materializing = false;
        }
    }

    public void sphereCheck()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 0.5f, 9);
        for(int i = 0; i < hitColliders.Length; i++)
        {
            Debug.Log(hitColliders[i]);
        }
    }

}
