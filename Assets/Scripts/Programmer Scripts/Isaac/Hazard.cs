using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
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
        if (other.GetComponentInParent<CharacterStats>())
        {
            Debug.Log("collision detected fucking hell");
            other.GetComponentInParent<CharacterStats>().RemoveHealth(other.GetComponentInParent<CharacterStats>().GetPlayerMaxHealth());
        }
    }
}
