using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigFan : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private int fanPower;

    //applies a constant upwards force while under the fan's influence (inside the collider)
    [SerializeField]
    private bool strongFan;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent == player.transform) /// if top level parent of the object colliding with elvator trigger is the given player 
        {
            Rigidbody playerRB = player.GetComponent<Rigidbody>();
            playerRB.AddForce(Vector3.up*fanPower, ForceMode.Acceleration);
        }
    }
    public void OnTriggerStay(Collider other)
    {
        if (strongFan)
        {
            if (other.transform.parent == player.transform) /// if top level parent of the object colliding with elvator trigger is the given player 
            {
                Rigidbody playerRB = player.GetComponent<Rigidbody>();
                playerRB.AddForce(Vector3.up * fanPower, ForceMode.Acceleration);
            }
        }
    }
    //de-parents player when leaving elevator
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.parent == player.transform) /// if top level parent of the object colliding with elvator trigger is the given player 
        {
            Rigidbody playerRB = player.GetComponent<Rigidbody>();
            playerRB.useGravity = true;
        }
    }
}
