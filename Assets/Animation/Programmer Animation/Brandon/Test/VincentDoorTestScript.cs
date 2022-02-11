using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VincentDoorTestScript : MonoBehaviour
{
    public bool isOpen = false;
    [SerializeField] Animator myAnim;
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponentInParent<P_Input>())
        {
            
            if(isOpen)
            {
                isOpen = false;
                myAnim.SetBool("closeDoor", true);
            }
            else
            {
                isOpen = true;
                myAnim.SetBool("openDoor", true);
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<P_Input>())
        {
            
            if (isOpen)
            {
                isOpen = false;
                myAnim.SetBool("closeDoor", true);
            }
            else
            {
                isOpen = true;
                myAnim.SetBool("openDoor", true);
            }

        }
    }

    
}
