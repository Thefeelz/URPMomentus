using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberEnemy : MonoBehaviour
{
    //Flyer states and stats
    enum FlyerState { Scanning, Targeting, LockOn, Explode, Asleep, Dead, SpecialInUse };
    [SerializeField]
    FlyerState currentState, previousState;

    //Flyer specfic stats
    [SerializeField]
    private float damage = 1f, explosionRange, attackPlayerSpeed, timeToLock, scanRange, patrolSpeed, rotateSpeed;
    [SerializeField]
    private bool patrolCounterClockwise = false;
    Coroutine LO;

    //So drone doesn't fly towards player's toes
    private float targetOffset = 1.0f;

    //[DEBUGGING] Raycast color
    Color sightColor;

    //Other Flyer pieces
    [SerializeField]
    ParticleSystem ExplosionEffect;
    private int point = 0;
    private Quaternion lastBodyRota;
    RaycastHit seeObject; // Sight raycast
    private Vector3 targetSpot;

    //The animations attached to flyer
    Animator anim;

    //Pivot parent
    Transform pivPar;

    //Enemy stats
    EnemyStats flyStat;

    //Player
    CharacterStats player;
    private Collider playerCollider;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<CharacterStats>();
        playerCollider = player.GetComponentInChildren<CapsuleCollider>();
        anim = GetComponentInChildren<Animator>();
        flyStat = GetComponent<EnemyStats>();

        attackPlayerSpeed /= 20;
        patrolSpeed /= 10;

        pivPar = transform.parent;

        if (patrolCounterClockwise)
        {
            patrolSpeed *= -1;
        }
    }

    private void Update()
    {
        if (flyStat.getCurrentHealth() <= 0)
        {
            currentState = FlyerState.Dead;
        }
    }

    private void FixedUpdate()
    {
        Physics.Raycast(this.transform.position, player.transform.position - transform.position, out seeObject, scanRange);
        Debug.DrawRay(this.transform.position, this.transform.forward * 10, sightColor);
        //Debug.Log(" Drone sees: " + seeObject.collider.name);

        if (currentState == FlyerState.Scanning)
        {
            if (seeObject.collider == playerCollider)
            {
                LO = StartCoroutine(LockOn());
                lastBodyRota = transform.rotation;
                currentState = FlyerState.Targeting;
            }

            pivPar.rotation *= Quaternion.Euler(0, patrolSpeed, 0);
        }
        else if (currentState == FlyerState.Targeting)
        {
            sightColor = Color.blue;
            anim.SetBool("shake", true);

            this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.LookRotation(player.transform.position + player.transform.up - this.transform.position), Time.deltaTime * rotateSpeed);


            if (seeObject.collider != playerCollider)
            {
                transform.rotation = lastBodyRota;
                StopCoroutine(LO);
                currentState = FlyerState.Scanning;
                anim.SetBool("shake", false);
            }
            targetSpot = player.transform.position + new Vector3(0, targetOffset, 0);

        }
        else if (currentState == FlyerState.LockOn)
        {
            sightColor = Color.red;
            this.transform.position = Vector3.MoveTowards(this.transform.position, targetSpot, attackPlayerSpeed);
            RaycastHit hit;
            Debug.DrawRay(transform.position, transform.forward, Color.red, 1f);
            if(Physics.Raycast(transform.position, transform.forward, out hit, 2f))
            {
                Debug.Log(hit.transform.name);
                currentState = FlyerState.Explode;
            }
            anim.SetBool("spin", true);
            if (Vector3.Distance(this.transform.position, targetSpot) < 0.5f)
            {
                currentState = FlyerState.Explode;
            }

        }
        else if (currentState == FlyerState.Explode)
        {
            if (Vector3.Distance(this.transform.position, player.transform.position + new Vector3(0, targetOffset, 0)) < explosionRange)
            {
                player.A_RemoveHealth(damage);
            }
            currentState = FlyerState.Dead;
        }
        else if (currentState == FlyerState.Dead)
        {
            Instantiate(ExplosionEffect, this.transform.position, this.transform.rotation);
            Destroy(this.gameObject, .01f); //wipes the object from existence

        }
        else if (currentState == FlyerState.Asleep)
        {


        }
        else if (currentState == FlyerState.SpecialInUse)
        {


        }
        else
        {
            Debug.LogError("Error: Flyer-state");
        }
    }
    public void SpecialInUse(bool value)
    {
        if (value)
        {
            previousState = currentState;
            currentState = FlyerState.SpecialInUse;
            StopCoroutine(LO);
            anim.SetBool("shake", false);
            anim.speed = 0;
        }
        else
        {
            if (currentState != FlyerState.Dead)
                currentState = FlyerState.Scanning;
            anim.speed = 1;
        }
    }
    IEnumerator LockOn()
    {
        yield return new WaitForSeconds(timeToLock);
        currentState = FlyerState.LockOn;
    }

    public void SetStateToAsleep()
    {
        StopCoroutine(LO);
        currentState = FlyerState.Asleep; 
    }


    private void OnCollisionEnter(Collision collision)
    {
        currentState = FlyerState.Explode;
    }
}