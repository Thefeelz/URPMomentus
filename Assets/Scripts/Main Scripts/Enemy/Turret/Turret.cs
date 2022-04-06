using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Turret : MonoBehaviour
{
    [SerializeField] GameObject headToRotate;
    [SerializeField] float detectionRange;
    [SerializeField] int damageToDeal;
    [SerializeField] float rotateSpeed;
    [SerializeField] float chargeUpTime;
    [SerializeField] float rateOfFire;
    [SerializeField] float bulletVelocity;
    [SerializeField] int bulletCapacity, currentBulletCount;
    [SerializeField] float ammoToAddPerSecond;
    [SerializeField] float reloadTime = 0f;
    [SerializeField] Light firingLight;
    [SerializeField] Transform firePosition;
    [SerializeField] ParticleSystem smokeEffect;
    [SerializeField] GameObject bulletsToFire;
    [SerializeField] LayerMask mask;
    [SerializeField] Image reloadSprite;

    enum TurretState  {Waiting, Attacking, Asleep, Dead, Charging, PlayerDead, SpecialInUse, Reload};
    [SerializeField]TurretState currentState;
    TurretState previousState;

    P_Input myPlayer;
    Animator anim;
    TurretRatatatatatata ratatatatatata;

    float elapsedChargeTime = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        ratatatatatata = GetComponent<TurretRatatatatatata>();
        currentState = TurretState.Asleep;
        currentBulletCount = bulletCapacity;
        myPlayer = FindObjectOfType<P_Input>();
        anim = GetComponent<Animator>();
        if (myPlayer)
            anim.SetBool("awake", false);
        else
            anim.SetBool("awake", true);
        rateOfFire = 1 / rateOfFire;
        if (ammoToAddPerSecond > 0)
            ammoToAddPerSecond = 1 / ammoToAddPerSecond;
        else
            ammoToAddPerSecond = 1;
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
        else if (currentState == TurretState.Reload)
        {
            Reload();
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
        else if (currentState == TurretState.Reload && Vector3.Distance(transform.position, myPlayer.transform.position) < detectionRange)
        {
            currentState = TurretState.Attacking;
            StartCoroutine(StartFiring());
        }
    }

    void Reload()
    {
        reloadTime += Time.deltaTime;
        if(reloadTime >= ammoToAddPerSecond)
        {
            currentBulletCount++;
            reloadSprite.fillAmount = (float)currentBulletCount / bulletCapacity;
            reloadTime = 0f;
            if(currentBulletCount == bulletCapacity)
            {
                ratatatatatata.EndedFiring();
                reloadSprite.fillAmount = 0;
                reloadSprite.gameObject.SetActive(false);
                CheckPlayerInRangeAttacking();
            }
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
            currentState = TurretState.Attacking;
            anim.SetBool("awake", true);
            StartCoroutine(StartFiring());
        }
    }

    IEnumerator StartFiring()
    {
        bool reload = false;
        while(currentState == TurretState.Attacking)
        {
            yield return  new WaitForSeconds(rateOfFire);
            GameObject newBullet = Instantiate(bulletsToFire, firePosition.position, Quaternion.identity);
            newBullet.GetComponent<EnemyBullet>().SetVelocityToPlayer(bulletVelocity, myPlayer.GetComponent<CharacterStats>(), headToRotate.transform, damageToDeal);
            ratatatatatata.FireRaTaTa(this.gameObject);
            currentBulletCount--;
            if(currentBulletCount == 0)
            {
                currentState = TurretState.Reload;
                reloadSprite.gameObject.SetActive(true);
                reload = true;
                break;
            }
        }
        if (!reload)
        {
            currentState = TurretState.Asleep;
            ResetLife();
        }
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
        firingLight.intensity = 0;
        ratatatatatata.EndedFiring();
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
