using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnCheckpointHelper : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponentInParent<CharacterStats>())
        {
            if(FindObjectOfType<RespawnCheckpointManager>())
            {
                FindObjectOfType<RespawnCheckpointManager>().ProgressToNextCheckpointInList();
            }
            else
            {
                Debug.LogError("There is no RespawnCheckpointManager in the scene. Make sure to add one");
            }
            Destroy(this);
        }
    }
}
