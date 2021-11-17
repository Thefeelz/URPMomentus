using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnGameObject : MonoBehaviour
{
    [SerializeField] GameObject objectToActivate;
    [SerializeField] bool onEnter, onExit;
    private void OnTriggerEnter(Collider other)
    {
        if(onEnter && other.GetComponentInParent<P_Input>())
        {
            objectToActivate.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (onExit && other.GetComponentInParent<P_Input>())
        {
            if (objectToActivate)
            {
                objectToActivate.SetActive(true);
            }
        }
    }
}
