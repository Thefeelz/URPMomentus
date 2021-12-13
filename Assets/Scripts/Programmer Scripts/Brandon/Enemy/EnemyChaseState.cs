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
    [SerializeField] bool startInChase;

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform bulletLaunch;

    public enum State {Chasing, Attacking, Dead, Inactive, SpecialInUse, CollideJump, Falling, Knockback}
    public State currentState;
    public State previousState;
    public bool isGrounded;
    bool dead = false;

    public bool specialInUse = false;

    Rigidbody enemyRigidbody;
    Vector3 startingPosition, targetPosition;
    float jumpTime;
    float elapsedTime = 0f;
    float distanceToGround;
    RaycastHit[] objectAvoidanceHits;
    Vector3[] directions;
    bool avoidObstacles = true;
    MasterLevel masterLevel;

    float knockBackCounter = 0f;
    // Start is called before the first frame update
    void Start()
    {
        masterLevel = FindObjectOfType<MasterLevel>();
        currentState = State.Inactive;
        player = FindObjectOfType<P_Movement>();
        enemyRigidbody = GetComponent<Rigidbody>();
        if (startInChase)
            currentState = State.Chasing;
        else if (deactive)
            animController.SetBool("deactive", true);
        distanceToGround = GetComponentInChildren<Collider>().bounds.extents.y;
        directions = new Vector3[]
        {
            Vector3.right,
            Vector3.zero,
            Vector3.left
        };
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
                else if (currentState == State.SpecialInUse)
                {
                    // I dont know what to put heeyah yet
                }
                else if (currentState == State.CollideJump)
                {
                    JumpToPosition();
                }
                else if (currentState == State.Knockback)
                {
                    knockBackCounter++;
                    if (knockBackCounter > 1)
                    {
                        currentState = State.Chasing;
                        knockBackCounter = 0;
                    }
                }
            }
            else
            {
                animController.speed = 0;
            }
        }
        CheckForGrounded();
        if (!isGrounded && AbleToEnterFallingState())
        {
            if (currentState != State.Falling)
            {
                
                previousState = currentState;
                animController.SetBool("falling", true);
                currentState = State.Falling;
            }
        }
        else if (currentState == State.Falling)
        {
            animController.SetBool("falling", false);
            if (previousState != State.Falling)
                currentState = previousState;
            else
                currentState = State.Inactive;
        }
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

    // INACTIVE STATE
    void CheckPlayerInRange()
    {
        if(DistanceFromEnemyToPlayer() < maxDetectionRange && PlayerInLineOfSight(maxDetectionRange))
        {
            currentState = State.Chasing;
            animController.SetBool("chasing", true);
        }
    }
    bool PlayerInLineOfSight(float detectionRange)
    {
        RaycastHit hit;
        Physics.Raycast(transform.position + Vector3.up, player.transform.position - transform.position, out hit, detectionRange);
        Debug.DrawRay(transform.position + Vector3.up, (player.transform.position - transform.position).normalized * detectionRange, Color.yellow, 0.1f);
        if(hit.collider != null && hit.transform.GetComponentInParent<P_Movement>())
        {
            return true;
        }
        return false;
    }

    // CHASING STATE
    void ChasePlayer()
    {
        float distance = DistanceFromEnemyToPlayer();
        transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
        // AvoidObstacles();
        if((ammoCount > 0 && distance > chaseStopRangeAttack) || (ammoCount == 0 && distance > chaseStopMeleeAttack))
        {
            enemyRigidbody.velocity = new Vector3(transform.forward.x * enemyRunSpeed, enemyRigidbody.velocity.y, transform.forward.z * enemyRunSpeed);
        }
        else
        {
            enemyRigidbody.velocity = Vector3.zero;
            currentState = State.Attacking;
            animController.SetBool("chasing", false);
        }
    }
    void AvoidObstacles()
    {
        objectAvoidanceHits = new RaycastHit[directions.Length];
        Vector3 directionVector = Vector3.zero;
        Physics.Raycast(transform.position + (Vector3.up * 0.25f), transform.forward, out objectAvoidanceHits[0], 2f);
        Physics.Raycast(transform.position + ((Vector3.up * 0.25f) + (transform.right)), transform.forward, out objectAvoidanceHits[1], 2f);
        Physics.Raycast(transform.position + ((Vector3.up * 0.25f) - (transform.right)), transform.forward, out objectAvoidanceHits[2], 2f);
        if ((objectAvoidanceHits[0].collider == null || objectAvoidanceHits[0].collider.gameObject.layer == 6) && PlayerInLineOfSight(100))
        {
            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));

        }
        else
            transform.Rotate(0, 5, 0);
    }

    // ATTACKING STATE
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
        masterLevel.AddToKillCount(1);
    }

    public void MeleeAttack()
    {
        RaycastHit hit;
        Physics.Raycast((transform.position + Vector3.up) + (transform.forward * 0.5f), transform.forward, out hit, 1f);
        if (hit.collider != null && hit.collider.GetComponentInParent<CharacterStats>())
        {
            hit.transform.GetComponentInParent<CharacterStats>().RemoveHealth(10f);
        }
    }

    void JumpToPosition()
    {
        elapsedTime += Time.deltaTime;
        float lerpPos = elapsedTime / jumpTime;
        float yStep = Time.deltaTime;
        if (lerpPos > 0.5)
            yStep *= -1;
        transform.position = Vector3.Lerp(startingPosition, targetPosition, lerpPos);
        transform.position = new Vector3(transform.position.x, transform.position.y + yStep, transform.position.z);

        if(lerpPos >= 1)
        {
            currentState = previousState;
            elapsedTime = 0;
            jumpTime = 0;
        }
    }

    public void SpecialInUse(bool value)
    {
        if (value)
        {
            previousState = currentState;
            currentState = State.SpecialInUse;
            animController.speed = 0;
        }
        else
        {
            if(currentState != State.Dead)
                currentState = previousState;
            animController.speed = 1;
        }
    }
    float DistanceFromEnemyToPlayer() { return Vector3.Distance(transform.position, player.transform.position); }

    public void SetStateToDead() 
    {
        deactive = false;
        currentState = State.Dead; 
    }
    public void SetStateToChase()
    {
        currentState = State.Chasing;
    }
    public void SetStateToKnockback()
    {
        enemyRigidbody.velocity = Vector3.zero;
        previousState = currentState;
        currentState = State.Knockback;
        StartCoroutine(KnockBackDelayOff());
    }
    public void SetStateToJumpCollider(Vector3 targetPos)
    {
        if(currentState == State.CollideJump) { return; }
        previousState = currentState;
        startingPosition = transform.position;
        targetPosition = targetPos;
        currentState = State.CollideJump;
        jumpTime = Vector3.Distance(startingPosition, targetPosition) / 5;
        animController.SetBool("falling", true);
    }

    public void ShootAtPlayer()
    {
        Instantiate(bulletPrefab, bulletLaunch.position, Quaternion.LookRotation(transform.forward));
        ammoCount--;
    }
    
    public State GetCurrentState() { return currentState; }
    void CheckForGrounded()
    {
        if (Physics.Raycast(transform.position + new Vector3(0, distanceToGround, 0), -Vector3.up, distanceToGround + .25f))
        {
            isGrounded = true;
        }
        else
            isGrounded = false;
    }
    IEnumerator KnockBackDelayOff()
    {
        yield return new WaitForSeconds(0.5f);
        CheckForGrounded();
        if (isGrounded)
        {
            if (previousState != State.Knockback)
                currentState = previousState;
            else
                currentState = State.Chasing;
        }
        else
            currentState = State.Falling;
    }
    bool AbleToEnterFallingState()
    {
        if (currentState != State.Knockback && currentState != State.CollideJump)
            return true;
        else
            return false;
    }
}
