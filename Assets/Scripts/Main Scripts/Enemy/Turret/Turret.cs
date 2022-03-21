using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] GameObject headToRotate;
    [SerializeField] float detectionRange;
    [SerializeField] int damageToDeal;
    [SerializeField] float rotateSpeed;
    [SerializeField] float sleepTimer;
    [SerializeField] float chargeUpTime;
    [SerializeField] float rateOfFire;
    [SerializeField] float fireBurstTime;
    [SerializeField] Light firingLight;
    [SerializeField] Transform firePosition;
    [SerializeField] ParticleSystem smokeEffect;
    [SerializeField] GameObject bulletsToFire;
    [SerializeField] LayerMask mask;

    enum TurretState  {Waiting, Attacking, Asleep, Dead, Charging, PlayerDead, SpecialInUse};
    [SerializeField]TurretState currentState;
    TurretState previousState;

    P_Input myPlayer;
    Animator anim;

    float elapsedChargeTime = 0f;
    float elapsedFireTime = 0f;
    bool charged = false;
    bool firing = false;
    // Start is called before the first frame update
    void Start()
    {
        currentState = TurretState.Asleep;
        myPlayer = FindObjectOfType<P_Input>();
        anim = GetComponent<Animator>();
        if (myPlayer)
            anim.SetBool("awake", false);
        else
            anim.SetBool("awake", true);
        rateOfFire = 1 / rateOfFire;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentState == TurretState.Asleep)
        {
            CheckPlayerInRange();
        }
        else if (currentState == TurretState.Attacking)
        {
            CheckPlayerInRangeAttacking();
            RotateTowardsPlayer();
        }
        else if (currentState == TurretState.Charging)
        {
            StartCharging();
        }
        else if (currentState == TurretState.PlayerDead)
        {

        }
        else if (currentState==TurretState.Dead)
        {

        }
        else if (currentState == TurretState.SpecialInUse)
        {

        }
    }

    void CheckPlayerInRange()
    {
        if (!myPlayer) { return; }
        if (Vector3.Distance(transform.position, myPlayer.transform.position) < detectionRange && CheckInLoS())
        {
                currentState = TurretState.Charging;
        }
        else if (currentState == TurretState.Attacking)
        {
            currentState = TurretState.Asleep;
            ResetLife();
            anim.SetBool("awake", false);
        }
    }

    void CheckPlayerInRangeAttacking()
    {
        if (!myPlayer) { return; }
        if (!(Vector3.Distance(transform.position, myPlayer.transform.position) < detectionRange))
        {
            currentState = TurretState.Asleep;
            ResetLife();
            anim.SetBool("awake", false);
        }
    }

    void RotateTowardsPlayer()
    {
        headToRotate.transform.rotation = Quaternion.RotateTowards(headToRotate.transform.rotation, 
                                                                   Quaternion.LookRotation(myPlayer.transform.position + myPlayer.transform.up - headToRotate.transform.position),
                                                                   Time.deltaTime * rotateSpeed);
    }

    void StartCharging()
    {
        elapsedChargeTime += Time.deltaTime;
        firingLight.intensity = Mathf.Lerp(0, 2, elapsedChargeTime / chargeUpTime);
        if (elapsedChargeTime >= chargeUpTime)
        {
            elapsedChargeTime = 0;
            charged = true;
            currentState = TurretState.Attacking;
            anim.SetBool("awake", true);
            StartCoroutine(StartFiring());
        }
    }

    IEnumerator StartFiring()
    {
        firing = true;
        while(currentState == TurretState.Attacking)
        {
            elapsedFireTime+=Time.deltaTime;
            yield return  new WaitForSeconds(rateOfFire);
            GameObject newBullet = Instantiate(bulletsToFire, firePosition.position, Quaternion.identity);
            newBullet.GetComponent<EnemyBullet>().SetVelocityToPlayer(15f, myPlayer.GetComponent<CharacterStats>(), headToRotate.transform, damageToDeal);
        }
        currentState = TurretState.Asleep;
        ResetLife();
    }

    bool CheckInLoS()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position + new Vector3(0, 1, 0), myPlayer.transform.position - transform.position, out hit, mask);
        Debug.DrawRay(transform.position + new Vector3(0, 1, 0), myPlayer.transform.position - transform.position);
        if (hit.collider != null && hit.collider.GetComponentInParent<P_Input>())
        {
            return true;
        }
        return false;
    }

    public void TurnOnTurretFire()
    {
        currentState = TurretState.Dead;
        smokeEffect.gameObject.SetActive(true);
        smokeEffect.Play();
        StopAllCoroutines();
    }

    void ResetLife()
    {
        charged = false;
        firing = false;
        firingLight.intensity = 0;
        elapsedFireTime = 0;
    }

    public void SetStateToPlayerDead() { currentState = TurretState.PlayerDead; }

    public void SetStateToSpecialInUse(bool value)
    {
        if (value)
        {
            previousState = currentState;
            currentState = TurretState.SpecialInUse;
            anim.speed = 0;
        }
        else
        {
            if (currentState != TurretState.Dead)
                currentState = previousState;
            anim.speed = 1;
        }
    }
}
