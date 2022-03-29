using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasAnimatorBrain : MonoBehaviour
{
    [SerializeField] Animator playerAnimator;
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
        playerAnimator.Play("playerDeathV3");
        FindObjectOfType<mouseLook>().enabled = false;
    }
    public void GoToDeathScene()
    {
        FindObjectOfType<SceneController>().GoToDeathScene();
    }
}
