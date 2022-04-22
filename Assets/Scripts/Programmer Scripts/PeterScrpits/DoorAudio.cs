using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAudio : MonoBehaviour
{
    public FMODUnity.EventReference DoorNoiseRef;

    FMOD.Studio.EventInstance doorState;

    // Start is called before the first frame update
    void Start()
    {
        doorState = FMODUnity.RuntimeManager.CreateInstance(DoorNoiseRef);
        
    }

  
    public void PlayDoorOpen()
    {
        FMODUnity.RuntimeManager.PlayOneShot(DoorNoiseRef, transform.position);
    }
}
