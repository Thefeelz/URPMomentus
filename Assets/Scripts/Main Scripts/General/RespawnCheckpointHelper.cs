using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnCheckpointHelper : MonoBehaviour
{
    bool trigger = false;
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponentInParent<CharacterStats>())
        {
            if(FindObjectOfType<RespawnCheckpointManager>() && !trigger)
            {
                FindObjectOfType<RespawnCheckpointManager>().ProgressToNextCheckpointInList();
                trigger = true;
            }
            else
            {
                Debug.LogError("There is no RespawnCheckpointManager in the scene. Make sure to add one");
            }
            // Destroy(this);
        }
    }
}
