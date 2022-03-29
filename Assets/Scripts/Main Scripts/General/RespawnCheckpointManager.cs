using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnCheckpointManager : MonoBehaviour
{
    [SerializeField] List<Transform> checkPointLocations = new List<Transform>();

    // Serialized for deBug purposes
    [SerializeField] int currentCheckpoint = 0;

    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        if(gameManager)
        {
            
            if (checkPointLocations.Count > 0)
            {
                gameManager.SetNewRespawnLocation(checkPointLocations[0]);
            }
            else
            {
                Debug.LogError("Checkpoint Locations count is 0, make sure to at least set up one for a default level start location to respawn");
            }
        }
        else
        {
            Debug.LogError("No Game Manager in the Scene, please add one.");
        }      
    }
    /// <summary>
    /// Call this function when the player interacts with a collider to update the current checkpoint in the level
    /// </summary>
    public void ProgressToNextCheckpointInList()
    {
        currentCheckpoint++;
        if (currentCheckpoint < checkPointLocations.Count)
        {
            gameManager.SetNewRespawnLocation(checkPointLocations[currentCheckpoint]);
        }
        else if (currentCheckpoint >= checkPointLocations.Count && checkPointLocations.Count != 0)
        {
            Debug.LogError("currentCheckpoint count is exceeding the list size for checkpointLocations, make sure you are destroying whatever is incrimenting the count after it incriments it.");
            gameManager.SetNewRespawnLocation(checkPointLocations[checkPointLocations.Count - 1]);
        }
        else if (checkPointLocations.Count == 0)
        {
            Debug.LogError("There are currently no checkPointLocations set in the RespawnCheckpointManager.cs, make sure to add some before continuing");
        }
    }
}
