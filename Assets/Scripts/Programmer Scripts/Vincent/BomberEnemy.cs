using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberEnemy : MonoBehaviour
{
    [SerializeField]
    private float damage = 1f, attackPlayerSpeed, timeToLock, range, targetOffset = 1f;

    private GameObject player, flyerBody;
    CharacterStats playerStats;
    private Collider playerCollider;

    private Quaternion lastBodyRota;

    RaycastHit seeObject; // Sight raycast
    private bool isLockedOn = false;
    private Vector3 targetSpot;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Body");

        playerCollider = player.GetComponent<Collider>();
        playerStats = FindObjectOfType<CharacterStats>();

        flyerBody = this.transform.GetChild(0).gameObject;
        attackPlayerSpeed = attackPlayerSpeed / 20;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //look at player while not locked on
        if (isLockedOn)
        {
            flyerBody.transform.LookAt(targetSpot);
        }
        else
        {
            Physics.Raycast(this.transform.position, player.transform.position, out seeObject, range);
        }

        if (seeObject.collider == playerCollider)
        {
            targetSpot = player.transform.position + new Vector3(0, targetOffset, 0);
            flyerBody.transform.LookAt(player.transform);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(LockOn());
    }
    private void OnTriggerExit(Collider other)
    {
        StopCoroutine(LockOn());
    }

    IEnumerator LockOn()
    {
        yield return new WaitForSeconds(timeToLock);
        isLockedOn = true;
    }
}
