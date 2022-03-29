using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberEnemy : MonoBehaviour
{
    //Flyer states and stats
    enum FlyerState { Scanning, Targeting, LockOn, Explode, Asleep, Dead };
    [SerializeField]
    FlyerState currentState;
    
    //Flyer specfic stats
    [SerializeField]
    private float damage = 1f, attackPlayerSpeed, timeToLock, scanRange, explosionRange;
    Coroutine LO;

    //So drone doesn't fly towards player's toes
    private float targetOffset=1.0f;

    Color sightColor;

    //Other Flyer pieces
    private Quaternion lastBodyRota;
    RaycastHit seeObject; // Sight raycast
    private Vector3 targetSpot;

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

        flyStat = GetComponent<EnemyStats>();
        attackPlayerSpeed = attackPlayerSpeed / 20;
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
                LO=StartCoroutine(LockOn());
                currentState = FlyerState.Targeting;
            }
        }
        else if (currentState == FlyerState.Targeting)
        {
            sightColor = Color.blue;
            transform.LookAt(player.transform);
            lastBodyRota = transform.rotation;
            if (seeObject.collider != playerCollider )
            {
                transform.rotation = lastBodyRota;
                StopCoroutine(LO);
                currentState = FlyerState.Scanning;
            }
            targetSpot = player.transform.position + new Vector3(0, targetOffset, 0);

        }
        else if (currentState == FlyerState.LockOn)
        {
            sightColor = Color.red;
            this.transform.position = Vector3.MoveTowards(this.transform.position, targetSpot, attackPlayerSpeed);
            if(Vector3.Distance(this.transform.position, targetSpot) < 1)
            {
                currentState = FlyerState.Explode;
            }

        }
        else if (currentState == FlyerState.Explode)
        {
            if(Vector3.Distance(this.transform.position, player.transform.position + new Vector3(0, targetOffset, 0)) < explosionRange)
            {
                player.A_RemoveHealth(damage);
            }
            currentState = FlyerState.Dead;
        }
        else if (currentState == FlyerState.Dead)
        {
            Destroy(this.gameObject); //wipes the object from existence

        }
        else if (currentState == FlyerState.Asleep)
        {


        }
        else
        {
            Debug.LogError("Error: Flyer-state");
        }
    }

    IEnumerator LockOn()
    {
        yield return new WaitForSeconds(timeToLock);
        currentState = FlyerState.LockOn;
    }

    public void SetStateToAsleep() { currentState = FlyerState.Asleep; }
}