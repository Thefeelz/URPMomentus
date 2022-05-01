using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasAnimatorBrain : MonoBehaviour
{
    [SerializeField] Animator playerAnimator;
    Pitfall resetLocation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartPlayerDeathSceneV3()
    {
        // Update the Game Manager with the furthest checkpoint
        if (FindObjectOfType<RespawnCheckpointManager>())
        {
            GameManager.Instance.SetNewRespawnLocation(FindObjectOfType<RespawnCheckpointManager>().GetCurrentIndexTransform());
            GameManager.Instance.SetRespawnCheckpointIndex(FindObjectOfType<RespawnCheckpointManager>().GetCurrentIndexNumber());
            GameManager.Instance.ClearEnemyList();
        }
        playerAnimator.Play("playerDeathV3");
        FindObjectOfType<mouseLook>().enabled = false;
    }
    public void GoToDeathScene()
    {
        FindObjectOfType<SceneController>().GoToDeathScene();
    }

    public void FallThroughPit(Pitfall resetLocationObject)
    {
        resetLocation = resetLocationObject;
        playerAnimator.SetBool("gap", true);
        GetComponent<Animator>().SetBool("gap", true);
        StartCoroutine(ResetBool("gap", false));
    }

    public void CallResetPlayer()
    {
        resetLocation.SendPlayerToLocation();
    }

    IEnumerator ResetBool(string name, bool value)
    {
        yield return new WaitForSeconds(0.25f);
        GetComponent<Animator>().SetBool(name, value);
    }

    public void SetHealthBarGroupToNotActive()
    {
        foreach (Transform transform in transform)
        {
            if (transform.CompareTag("HealthBarGroup"))
                transform.gameObject.SetActive(false);
        }
    }
    public void BeatTutorialLevel()
    {
        if(FindObjectOfType<SceneController>())
        {
            FindObjectOfType<SceneController>().GoToNextScene();
        }
        else
        {
            Debug.LogError("There is no Scene Controller in this scene, add one");
        }
    }
}
