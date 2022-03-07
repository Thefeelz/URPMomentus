using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pitfall : MonoBehaviour
{
    //where the player goes after being falling into pit
    [SerializeField]
    GameObject resetLocation;

    [SerializeField]
    GameObject Player;


    //if the player enters the trigger box
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        Debug.Log(other.gameObject.name);
        if(other.gameObject.name == "Body")
        {
            Player.transform.position = resetLocation.transform.position;
            
            Debug.Log("Player hit the pit");
        }
        else
        {
            Debug.Log("we Didn't get it");
        }
        Debug.Log("Collider Hit");

        
    }
   
}
