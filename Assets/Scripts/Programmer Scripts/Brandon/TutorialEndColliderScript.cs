using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEndColliderScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<CharacterStats>())
        {
            other.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}
