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

    enum TurretState  {Waiting, Attacking, Asleep, Dead, Charging};
    [SerializeField]TurretState state;

    P_Input myPlayer;
    Animator anim;

    float elapsedChargeTime = 0f;
    float elapsedFireTime = 0f;
    bool charged = false;
    bool firing = false;
    // Start is called before the first frame update
    void Start()
    {
        state = TurretState.Asleep;
        myPlayer = FindObjectOfType<P_Input>();
        anim = GetComponent<Animator>();
        anim.SetBool("awake", false);
        rateOfFire = 1 / rateOfFire;
    }

    // Update is called once per frame
    void Update()
    {
        if(state == TurretState.Asleep)
        {
            CheckPlayerInRange();
        }
        else if (state == TurretState.Attacking)
        {
            CheckPlayerInRangeAttacking();
            RotateTowardsPlayer();
        }
        else if (state == TurretState.Charging)
        {
            StartCharging();
        }
    }

    void CheckPlayerInRange()
    {
        if (!myPlayer) { return; }
        if (Vector3.Distance(transform.position, myPlayer.transform.position) < detectionRange && CheckInLoS())
        {
                state = TurretState.Charging;
        }
        else if (state == TurretState.Attacking)
        {
            state = TurretState.Asleep;
            ResetLife();
            anim.SetBool("awake", false);
        }
    }

    void CheckPlayerInRangeAttacking()
    {
        if (!myPlayer) { return; }
        if (!(Vector3.Distance(transform.position, myPlayer.transform.position) < detectionRange))
        {
            state = TurretState.Asleep;
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
            state = TurretState.Attacking;
            anim.SetBool("awake", true);
            StartCoroutine(StartFiring());
        }
    }

    IEnumerator StartFiring()
    {
        firing = true;
        while(state == TurretState.Attacking)
        {
            elapsedFireTime+=Time.deltaTime;
            yield return  new WaitForSeconds(rateOfFire);
            GameObject newBullet = Instantiate(bulletsToFire, firePosition.position, Quaternion.identity);
            newBullet.GetComponent<EnemyBullet>().SetVelocityToPlayer(15f, myPlayer.GetComponent<CharacterStats>(), headToRotate.transform, damageToDeal);
        }
        state = TurretState.Asleep;
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
        state = TurretState.Dead;
        smokeEffect.gameObject.SetActive(true);
        smokeEffect.Play();
    }

    void ResetLife()
    {
        charged = false;
        firing = false;
        firingLight.intensity = 0;
        elapsedFireTime = 0;
    }
}
