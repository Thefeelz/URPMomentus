using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour


{
    [SerializeField]
    private CharacterStats player;

    [SerializeField]
    private int bouncePower;

    private Rigidbody playerBody;
    

    public bool instakill;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<CharacterStats>();
        //playerBody = player.GetComponentInChildren<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<CharacterStats>())
        {
            if (instakill)
            {
                player.RemoveHealthMelee(player.GetPlayerMaxHealth());
            }
            else
            {
                other.GetComponentInParent<CharacterStats>().RemoveHealthMelee(other.GetComponentInParent<CharacterStats>().GetPlayerMaxHealth() / 3);
                //playerBody.AddForce(Vector3.up * bouncePower, ForceMode.VelocityChange);
            }
            
        }

    }
}
