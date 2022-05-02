using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FlyingEnemy : MonoBehaviour
{
    //Primary Flyer Components\\

    //Flyer states
    enum FlyerState { Scanning, Targeting, LockOn, Explode, Asleep, Dead };
    [SerializeField]
    FlyerState currentState, returnToState;

    //Flyer settings
    [SerializeField]
    private float damage=50, explosionRange=6, attackPlayerSpeed=4, timeToLock=5, scanRange=20, patrolSpeed=5, rotateSpeed=100;
    
    //Flight destination
    private Vector3 targetSpot;
    private float targetOffset = 1.0f; ///So drone doesn't fly towards player's toes

    //Holds "Lock On" coroutine so that it can be stopped if done is no longer locking-on
    private Coroutine LO;

    //Player
    CharacterStats player;
    private Collider playerCollider;

    //Sound
    public FMODUnity.EventReference flyingIdleRef;
    FMOD.Studio.EventInstance flyingIdle;
    public FMODUnity.EventReference explosionRef;
    FMOD.Studio.EventInstance explosion;


    //Extra Flyer Components\\

    //Active or Inactive
    [SerializeField]
    private bool StartAsleep = false;

    //Patrol direction
    [SerializeField]
    private bool patrolCounterClockwise = false;

    //Rotation before player spotted
    private Quaternion lastBodyRota;

    // Sight raycast
    RaycastHit seeObject;

    //[DEBUGGING] Raycast color
    Color sightColor;
     
    //Explosion effect
    [SerializeField]
    ParticleSystem ExplosionEffect;
    
    //The animations attached to flyer
    Animator anim;

    //The Parent's transform for circular patrol
    Transform pivPar;

    //Enemy stats
    EnemyStats flyStat;

    

    // Start is called before the first frame update
    void Start()
    {

        flyingIdle.start();
        //Connects components from the scene to respective variables
        player = FindObjectOfType<CharacterStats>();
        if (player == null) Debug.LogError("Can't find player");

        playerCollider = player.GetComponentInChildren<CapsuleCollider>();
        if (playerCollider == null) Debug.LogError("Player collider is not in it's children");

        anim = GetComponentInChildren<Animator>();
        if (anim == null) Debug.LogError("Can't find flyer's animator");

        flyStat = GetComponent<EnemyStats>();
        if (flyStat == null) Debug.LogError("Missing EnemyStats script");

        pivPar = transform.parent;
        if (pivPar == null) Debug.LogWarning("Flyer has no parent to orbit, Patrol is disabled");

        //Cuts speed to realistic levels
        attackPlayerSpeed /= 20;
        patrolSpeed /= 10;

        //Multiplies rotation speed by -1 to flip direction
        if (patrolCounterClockwise)
        {
            patrolSpeed *= -1;
        }

        //Sound stuff
        flyingIdle = FMODUnity.RuntimeManager.CreateInstance(flyingIdleRef);
        flyingIdle.start();

        //Inactive on spawn
        if (StartAsleep)
            SetStateToAsleep();
    }

    private void Update()
    {
        //The moment health reaches zero, set state to dead
        if (flyStat.getCurrentHealth() <= 0)
        {
            currentState = FlyerState.Dead;
        }
    }

    private void FixedUpdate()
    {
        //Raycast towards player
        Physics.Raycast(this.transform.position, player.transform.position - transform.position, out seeObject, scanRange);

        //[DEBUGGING] Draws the raycast using current state's color
        Debug.DrawRay(this.transform.position, this.transform.forward * 10, sightColor);
        ///Debug.Log(" Drone sees: " + seeObject.collider.name);


// FLYING DRONE STATES \\
    ///Debug.Log(currentState);
    //SCANNING\\
        if (currentState == FlyerState.Scanning)
        {
            //If Raycast sees player: Start LockOn coroutine, record rotation, change state to TARGETING
            if (seeObject.collider == playerCollider)
            {
                LO = StartCoroutine(LockOn());
                lastBodyRota = transform.rotation;
                currentState = FlyerState.Targeting;
            }

            //Rotates parent to give illusion of flying in a circle
            if(pivPar!=null)
                pivPar.rotation *= Quaternion.Euler(0, patrolSpeed, 0);
        }

    //TARGETING\\
        else if (currentState == FlyerState.Targeting)
        {
            //Change drawn raycast line's color
            sightColor = Color.blue;

            //Plays animation that makes it shake
            anim.SetBool("shake", true);

            //Smoothly rotates to face player
            this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.LookRotation(player.transform.position + player.transform.up - this.transform.position), Time.deltaTime * rotateSpeed);

            //If raycast no longer shows player: revert to old rotation, stop the lock-on coroutine, stop shaking, change state to SCANNING
            if (seeObject.collider != playerCollider)
            {
                transform.rotation = lastBodyRota;
                StopCoroutine(LO);
                anim.SetBool("shake", false);
                currentState = FlyerState.Scanning;
            }

            //Keeps updating target destination with player's current position
            targetSpot = player.transform.position + new Vector3(0, targetOffset, 0);
        }

    //LOCKED ON\\
        else if (currentState == FlyerState.LockOn)
        {
            ///Change drawn raycast line's color
            sightColor = Color.red;
            
            //Flies towards a locked position
            this.transform.position = Vector3.MoveTowards(this.transform.position, targetSpot, attackPlayerSpeed);

            //Starts spinning 
            anim.SetBool("spin", true);

            //When close enough to destination, change state to EXPLODE
            if (Vector3.Distance(this.transform.position, targetSpot) < 0.5f)
            {

                currentState = FlyerState.Explode;
            }
        }

    //EXPLODE\\
        else if (currentState == FlyerState.Explode)
        {
            //If player is in given range, damage player
            if (Vector3.Distance(this.transform.position, player.transform.position + new Vector3(0, targetOffset, 0)) < explosionRange)
            {
                player.A_RemoveHealth(damage);
            }

            //Change state to DEAD
            currentState = FlyerState.Dead;
        }

    //DEAD\\
        else if (currentState == FlyerState.Dead)
        {
            //Creates an instance of the explosion effect at enemy's current position (Particle system will delete itself after finished playing)
            Instantiate(ExplosionEffect, this.transform.position, this.transform.rotation);

            FMODUnity.RuntimeManager.PlayOneShot(explosionRef, this.transform.position);

            //Wipe the enemy from existence
            Destroy(this.gameObject, .1f); 
        }

    //Empty state\\
        else if (currentState == FlyerState.Asleep)
        {
            ///Blank on purpose\\\
        }
    
    //Error case\\
        else
        {
            Debug.LogError("Error: Flyer-state");
        }
    }


    //Changes state after set duration
    IEnumerator LockOn()
    {
        yield return new WaitForSeconds(timeToLock);
        currentState = FlyerState.LockOn;
    }

    //Can be called to deactivate enemy without destruction
    public void SetStateToAsleep() 
    {
        StopCoroutine(LO);
        currentState = FlyerState.Asleep;
    }

    public void SpecialInUse(bool value)
    {
        if (value)
        {
            returnToState = currentState;
            currentState = FlyerState.Asleep;
            StopCoroutine(LO);
            anim.speed = 0;
        }
        else
        {
            if (currentState != FlyerState.Dead)
            {
                currentState = returnToState;
            }
                
            anim.speed = 1;
        }
    }

    //Will explode state if it collides with any object
    private void OnCollisionEnter(Collision collision)
    {
        currentState = FlyerState.Explode;
    }
}