using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnCheckpointManager : MonoBehaviour
{
    [SerializeField] List<Transform> checkPointLocations = new List<Transform>();
    [SerializeField] Transform startingTransform;
    [SerializeField] List<GameObject> checkPointOneGameObjects = new List<GameObject>();
    [SerializeField] List<GameObject> checkPointTwoGameObjects = new List<GameObject>();

    // Serialized for deBug purposes
    [SerializeField] int currentCheckpoint = 0;

    
    
    CharacterStats player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<CharacterStats>();
        currentCheckpoint = GameManager.Instance.GetRespawnCheckpointIndex();
        SetUpLevelStart();
    }
    /// <summary>
    /// Call this function when the player interacts with a collider to update the current checkpoint in the level
    /// </summary>
    public void ProgressToNextCheckpointInList()
    {
        currentCheckpoint++;
        if (currentCheckpoint < checkPointLocations.Count)
        {
            GameManager.Instance.SetNewRespawnLocation(checkPointLocations[currentCheckpoint]);
            GameManager.Instance.SetRespawnCheckpointIndex(currentCheckpoint);
        }
        else if (currentCheckpoint >= checkPointLocations.Count && checkPointLocations.Count != 0)
        {
            Debug.LogError("currentCheckpoint count is exceeding the list size for checkpointLocations, make sure you are destroying whatever is incrimenting the count after it incriments it.");
            GameManager.Instance.SetNewRespawnLocation(checkPointLocations[checkPointLocations.Count - 1]);
        }
        else if (checkPointLocations.Count == 0)
        {
            Debug.LogError("There are currently no checkPointLocations set in the RespawnCheckpointManager.cs, make sure to add some before continuing");
        }
    }

    

    public void SetUpLevelStart()
    {
        if(GameManager.Instance.GetRespawnAtCheckpoint() && player)
        {
            DeactivatePassedCheckpoints();
            player.transform.position = checkPointLocations[currentCheckpoint].position;
            player.transform.rotation = checkPointLocations[currentCheckpoint].rotation;
        }
        else if (player)
        {
            player.transform.position = checkPointLocations[0].position;
            player.transform.rotation = checkPointLocations[0].rotation;
        }
        else
        {
            Debug.LogError("No Player Found");
        }
    }
    public Transform GetCurrentIndexTransform() 
    {
        if (currentCheckpoint >= checkPointLocations.Count)
            return checkPointLocations[checkPointLocations.Count - 1];
        return checkPointLocations[currentCheckpoint]; 
    }
    public int GetCurrentIndexNumber()
    {
        if (currentCheckpoint >= checkPointLocations.Count)
            return checkPointLocations.Count - 1;
        return currentCheckpoint;
    }
    void DeactivatePassedCheckpoints()
    {
        if(currentCheckpoint > 0)
        {
            foreach (GameObject item in checkPointOneGameObjects)
            {
                item.SetActive(false);
            }
        }
        if (currentCheckpoint > 1)
        {
            foreach (GameObject item in checkPointTwoGameObjects)
            {
                item.SetActive(false);
            }
        }
    }
}
