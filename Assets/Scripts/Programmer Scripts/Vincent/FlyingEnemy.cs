using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    [SerializeField]
    private float damage=1f, attackPlayerSpeed, distToDestruct, selfDestructTimer, targetOffset = 1f;

    private GameObject player, flyerBody;
    CharacterStats playerStats;
    private Collider playerCollider;
    private SphereCollider RoI;

    private float raycastLength;
    private bool goingBoom = false;
    private string currentState;

    private Quaternion holdBodyRot;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Body");
        playerStats = FindObjectOfType<CharacterStats>();
        flyerBody = this.transform.GetChild(0).gameObject;

        playerCollider = player.GetComponent<Collider>();
        attackPlayerSpeed = attackPlayerSpeed / 20;
        RoI = GetComponentInChildren<SphereCollider>();
        raycastLength = 50;
    }

    private void FixedUpdate()
    {

        if (!goingBoom)
        {
            flyerBody.transform.LookAt(player.transform);
            holdBodyRot = flyerBody.transform.rotation;
        }
        else
        {
            flyerBody.transform.rotation = holdBodyRot;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("stay: "+other+" playerCollider:"+playerCollider);
        if (other == playerCollider)  // Checks if player
        {
            Vector3 playerHead = other.transform.position + new Vector3(0, targetOffset, 0); // Offsets target so it doesn't target player's feet
            RaycastHit hitObject; // Sight raycast 

            transform.LookAt(playerHead); // Set's rotation to look at the offset of the player
            Physics.Raycast(this.transform.position, this.transform.forward, out hitObject, raycastLength); // sends a raycast forward equal to the radius of it's influence and puts results in "hitObject"

            // if player is in range, start detonation 
            if (Vector3.Distance(this.transform.position, playerHead) <= distToDestruct && !goingBoom)
            {
                Debug.Log("Conditions met");
                goingBoom = true;
                StartCoroutine(Explode());
            }

            // if raycast hits player and is not exploding, move towards player
            else if (hitObject.collider == playerCollider && !goingBoom)
            {
                Debug.Log("enemy spotted");
                Debug.DrawRay(this.transform.position, this.transform.forward * 10, Color.red);
                this.transform.position = Vector3.MoveTowards(this.transform.position, playerHead, attackPlayerSpeed);
            }
        }
    }
    /*
     *check area of influence
    if player enters and not supporting enemy
        move towards player
        if player near
            blow up
    if no player and not supporing
        find support target
    if no player or enemy
        roam

    ROAM 
            idk...roam? 

    
     */
    IEnumerator Explode()
    {
        Debug.Log(selfDestructTimer + " seconds till self-destruct");//outputs countdown start
        yield return new WaitForSeconds(selfDestructTimer); //starts delay
        Debug.Log("damaging");
        playerStats.RemoveHealthMelee(damage);
        Debug.Log("should be damaged");
        Destroy(this.gameObject); //wipes the object from existence
    }
}
