using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnCheckpointHelper : MonoBehaviour
{
    [SerializeField] int respawnNumber;
    bool trigger = false;
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponentInParent<CharacterStats>())
        {
            if(FindObjectOfType<RespawnCheckpointManager>() && !trigger)
            {
                if (GameManager.Instance.GetRespawnCheckpointIndex() < respawnNumber)
                {
                    FindObjectOfType<RespawnCheckpointManager>().ProgressToNextCheckpointInList();
                    trigger = true;
                }
            }
            else
            {
                Debug.LogError("There is no RespawnCheckpointManager in the scene. Make sure to add one");
            }
            // Destroy(this);
        }
    }
}
