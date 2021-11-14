using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : MonoBehaviour
{
    P_Movement player;
    [Tooltip("Allows the enemy to be 'broken' so they will never attack the player")]
    [SerializeField] bool deactive;

    [Header("Detection Ranges")]
    [Tooltip("The max detection range until an enemy chases a player")]
    [SerializeField] float maxDetectionRange;
    [Tooltip("The Range at which an enemy will begin shooting the player")]
    [SerializeField] float chaseStopRangeAttack;
    [Tooltip("The Range at which an enemy will continue to chase the player if the enemy has ammo")]
    [SerializeField] float chaseStartRangeAttack;
    [Tooltip("The Range at which an enemy will begin to melee the player")]
    [SerializeField] float chaseStopMeleeAttack;
    [Tooltip("The Range at which an enemy will continue to chase the player if the enemy has no ammo")]
    [SerializeField] float chaseStartMeleeAttack;
    [Tooltip("The Animator attached to this enemy")]
    [SerializeField] Animator animController;
    [SerializeField] float enemyRunSpeed;
    [Range(0, 100)][SerializeField] int ammoCount;

    enum State {Chasing, Attacking, Dead, Inactive}
    [SerializeField ]State currentState;
    bool dead = false;

    public bool specialInUse = false;

    Rigidbody enemyRigidbody;
    // Start is called before the first frame update
    void Start()
    {
        currentState = State.Inactive;
        player = FindObjectOfType<P_Movement>();
        enemyRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!deactive)
        {
            if (!specialInUse)
            {
                if (currentState == State.Inactive)
                {
                    CheckPlayerInRange();
                }
                else if (currentState == State.Chasing)
                {
                    // I dont know what to put heeyah yet
                }
                else if (currentState == State.Attacking)
                {
                    AttackPlayer();
                }
                else if (currentState == State.Dead && !dead)
                {
                    Die();
                }
            }
            else
            {
                animController.speed = 0;
            }
        }
        else
            animController.SetBool("deactive", true);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(!specialInUse)
        {
            if(currentState == State.Chasing)
            {
                ChasePlayer();
            }
        }
    }

    void CheckPlayerInRange()
    {
        if(DistanceFromEnemyToPlayer() < maxDetectionRange)
        {
            currentState = State.Chasing;
            animController.SetBool("chasing", true);
        }
    }

    void ChasePlayer()
    {
        float distance = DistanceFromEnemyToPlayer();
        transform.LookAt(player.transform);
        if((ammoCount > 0 && distance > chaseStopRangeAttack) || (ammoCount == 0 && distance > chaseStopMeleeAttack))
        {
            enemyRigidbody.velocity = transform.forward * enemyRunSpeed;
        }
        else
        {
            currentState = State.Attacking;
            animController.SetBool("chasing", false);
        }
    }
    void AttackPlayer()
    {
        transform.LookAt(player.transform);
        float distance = DistanceFromEnemyToPlayer();
        bool startChase = false;
        if (ammoCount > 0)
        {
            animController.SetBool("rangeAttack", true);
            if(distance > chaseStartRangeAttack)
            {
                startChase = true;
            }
        }
        else
        {
            animController.SetBool("meleeAttack", true);
            if(distance > chaseStartMeleeAttack)
            {
                startChase = true;
            }
        }
        if(startChase)
        {
            animController.SetBool("rangeAttack", false);
            animController.SetBool("meleeAttack", false);
            animController.SetBool("chasing", true);
            currentState = State.Chasing;
        }
    }

    void Die()
    {
        dead = true;
        animController.SetBool("dead", true);
    }
    float DistanceFromEnemyToPlayer() { return Vector3.Distance(transform.position, player.transform.position); }

    public void SetStateDead() 
    {
        deactive = false;
        currentState = State.Dead; 
    }

    public void ShootAtPlayer()
    {
        Debug.Log("Pew");
        ammoCount--;
    }
}
