using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAudio : MonoBehaviour
{
    public FMODUnity.EventReference DoorOpenNoiseRef;
    public FMODUnity.EventReference DoorCloseNoiseRef;


    // Start is called before the first frame update
    void Start()
    {
        
    }

  
    public void PlayDoorOpen()
    {
        FMODUnity.RuntimeManager.PlayOneShot(DoorOpenNoiseRef, transform.position);
    }


    public void PlayDoorClose()
    {
        FMODUnity.RuntimeManager.PlayOneShot(DoorCloseNoiseRef, transform.position);
    }
}
