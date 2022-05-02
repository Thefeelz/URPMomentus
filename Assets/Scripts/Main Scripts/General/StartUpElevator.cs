using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUpElevator : MonoBehaviour
{
    [SerializeField] Animator elevator;
    [SerializeField] FMODUnity.EventReference elevatorNoiseRef;

    // Start is called before the first frame update
    void Start()
    {
        if (elevator)
        {
            elevator.SetBool("startElevator", true);
            FMODUnity.RuntimeManager.PlayOneShot(elevatorNoiseRef, this.transform.position);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
