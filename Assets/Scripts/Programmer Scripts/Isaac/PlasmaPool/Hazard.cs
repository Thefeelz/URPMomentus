using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour


{
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private int bouncePower;

    private Rigidbody playerBody;

    // Start is called before the first frame update
    void Start()
    {
        playerBody = player.GetComponentInChildren<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<CharacterStats>())
        {
            Debug.Log("collision detected fucking hell");
            other.GetComponentInParent<CharacterStats>().RemoveHealthMelee(other.GetComponentInParent<CharacterStats>().GetPlayerMaxHealth() / 3);
            playerBody.AddForce(Vector3.up * bouncePower, ForceMode.VelocityChange);
        }
    }
}
