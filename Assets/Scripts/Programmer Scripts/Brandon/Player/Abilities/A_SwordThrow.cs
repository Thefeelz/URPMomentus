using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_SwordThrow : MonoBehaviour
{
    [SerializeField] Transform endingPosition;
    [SerializeField] LayerMask layerMask;
    [SerializeField] GameObject weaponToThrow;
    // In meters per second;
    [SerializeField] float throwSpeed = 20f;
    [SerializeField] float rotationSpeed = 20f;
    float travelTime = 0f;
    bool throwing = false;
    bool returning = false;
    float elapsedTime = 0f;
    Vector3 targetDestination;
    float rotationAmount;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (throwing)
            Throw();
        if (returning)
            ReturnSword();
    }

    public void ThrowSword()
    {
        if (!throwing)
        {
            Debug.Log("Throw Sword was Called");
            RaycastHit hit;
            Physics.Raycast(transform.position, transform.forward * 20f, out hit);
            Debug.DrawRay(transform.position + transform.up, transform.forward * 20f, Color.red, 1f) ;
            if (hit.collider != null)
            {
                throwing = true;
                targetDestination = hit.point;
                Debug.Log(targetDestination + hit.collider.name);
                weaponToThrow.GetComponentInParent<Animator>().enabled = false;
                weaponToThrow.transform.parent = null;
                float distance = Vector3.Distance(transform.position, targetDestination);
                travelTime = distance / throwSpeed;
                DetermineRotations();
            }
        }
    }

    public void Throw()
    {
        elapsedTime += Time.deltaTime;
        weaponToThrow.transform.Rotate(new Vector3(0, 0, -rotationSpeed * Time.deltaTime));
        
        weaponToThrow.transform.position = Vector3.Lerp(endingPosition.position, targetDestination, elapsedTime / travelTime);
        if(elapsedTime >= travelTime)
        {
            throwing = false;
            elapsedTime = 0f;
            StartCoroutine(SwordStickDelay());
        }
    }

    void DetermineRotations()
    {
        if(travelTime < 1)
        {
            rotationAmount = 460;
        }
        else if (travelTime < 2)
        {
            rotationAmount = 820;
        }
        else
        {
            rotationAmount = 1180;
        }
        rotationSpeed = rotationAmount / travelTime;
    }
    void ReturnSword()
    {
        elapsedTime += Time.deltaTime;

        weaponToThrow.transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));

        weaponToThrow.transform.position = Vector3.Lerp(targetDestination, endingPosition.position, elapsedTime / travelTime);
        if(elapsedTime >= travelTime)
        {
            returning = false;
            weaponToThrow.transform.parent = transform;
            weaponToThrow.transform.position = endingPosition.position;
            weaponToThrow.transform.rotation = endingPosition.rotation;
            elapsedTime = 0f;
            weaponToThrow.GetComponentInParent<Animator>().enabled = true;
        }
    }

    IEnumerator SwordStickDelay()
    {
        yield return new WaitForSeconds(0.5f);
        returning = true;
    }
}
