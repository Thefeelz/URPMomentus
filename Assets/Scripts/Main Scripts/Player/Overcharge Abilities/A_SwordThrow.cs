using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_SwordThrow : A_OverchargeAbilities
{
    [SerializeField] Transform endingPosition, rightHandEndPos;
    [SerializeField] LayerMask layerMask;
    [SerializeField] GameObject weaponToThrow;
    // In meters per second;
    [SerializeField] float throwSpeed = 20f;
    [SerializeField] float rotationSpeed = 20f;
    [SerializeField] float minThrowDistance = 5f;
    [SerializeField] float maxThrowDistance = 20f;
    float travelTime = 0f;
    bool throwing = false;
    bool returning = false;
    bool travelToSword = false;
    public bool stuck = false;
    float elapsedTime = 0f;
    Vector3 targetDestination;
    Vector3 initialPosition;
    float rotationAmount;
    SwordThrow swordThrow;
    
    void Start()
    {
        swordThrow = GetComponentInChildren<SwordThrow>();
        swordThrow.enabled = false;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (throwing)
            Throw();
        if (returning)
            ReturnSword();
        if (travelToSword)
            TravelToSword();
    }

    public void ThrowSword()
    {
        if (!throwing && !stuck && !returning)
        {
            RaycastHit hit;
            Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit);
            if (hit.collider != null)
            {
                targetDestination = hit.point;
                float distance = Vector3.Distance(transform.position, targetDestination);
                if (distance > minThrowDistance && distance < maxThrowDistance)
                {
                    swordThrow.enabled = true;
                    throwing = true;
                    weaponToThrow.GetComponentInParent<Animator>().enabled = false;
                    weaponToThrow.transform.parent = null;
                    weaponToThrow.transform.LookAt(targetDestination);
                    weaponToThrow.transform.Rotate(transform.up, 90f);
                    travelTime = distance / throwSpeed;
                    DetermineRotations();
                }
            }
        }
    }

    public void FlyToSword()
    {
        StopAllCoroutines();
        initialPosition = transform.position;
        travelToSword = true;
        playerMovement.enabled = false;
        mouseLook.enabled = false;
    }

    void Throw()
    {
        elapsedTime += Time.deltaTime;
        weaponToThrow.transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));

        weaponToThrow.transform.position = Vector3.Lerp(endingPosition.position, targetDestination, elapsedTime / travelTime);
        if(elapsedTime >= travelTime)
        {
            stuck = true;
            throwing = false;
            elapsedTime = 0f;
            StartCoroutine(SwordStickDelay());
        }
    }
    public void SwordHitEnemy(EnemyStats enemyHit)
    {
        elapsedTime = 0f;
        weaponToThrow.transform.parent = enemyHit.transform;
        stuck = true;
        throwing = false;
        StartCoroutine(SwordStickDelay());
    }

    void DetermineRotations()
    {
        if(travelTime < 1)
        {
            rotationAmount = 520;
        }
        else if (travelTime < 1.5)
        {
            rotationAmount = 880;
        }
        else if (travelTime < 2)
        {
            rotationAmount = 1240;
        }
        else if (travelTime < 2.5)
        {
            rotationAmount = 1600;
        }
        else
        {
            rotationAmount = 1960;
        }
        rotationSpeed = rotationAmount / travelTime;
    }
    void ReturnSword()
    {
        elapsedTime += Time.deltaTime;

        weaponToThrow.transform.Rotate(new Vector3(0, 0, -rotationSpeed * Time.deltaTime));

        weaponToThrow.transform.position = Vector3.Lerp(targetDestination, endingPosition.position, elapsedTime / travelTime);
        if(elapsedTime >= travelTime)
        {
            returning = false;
            weaponToThrow.transform.parent = endingPosition.transform.parent;
            weaponToThrow.transform.position = endingPosition.position;
            weaponToThrow.transform.rotation = endingPosition.rotation;
            elapsedTime = 0f;
            weaponToThrow.GetComponentInParent<Animator>().enabled = true;
        }
    }
    void TravelToSword()
    {
        elapsedTime += Time.deltaTime;
        transform.position = Vector3.Lerp(initialPosition, targetDestination - (transform.forward * 2f), elapsedTime / (travelTime * 0.5f));
        if(elapsedTime >= travelTime)
        {
            stuck = false;
            travelToSword = false;
            weaponToThrow.transform.parent = rightHandEndPos;
            weaponToThrow.transform.position = endingPosition.position;
            weaponToThrow.transform.rotation = endingPosition.rotation;
            elapsedTime = 0;
            playerMovement.enabled = true;
            mouseLook.enabled = true;
            weaponToThrow.GetComponentInParent<Animator>().enabled = true;
        }
    }

    IEnumerator SwordStickDelay()
    {
        yield return new WaitForSeconds(0.5f);
        weaponToThrow.transform.parent = null;
        swordThrow.HitEnemyToFalse();
        swordThrow.enabled = false;
        stuck = false;
        returning = true;
    }

    public bool SwordThrowComplete()
    {
        return (!returning && !throwing);
    }
}
