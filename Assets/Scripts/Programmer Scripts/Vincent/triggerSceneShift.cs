using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class triggerSceneShift : MonoBehaviour
{
    
    [SerializeField]
    private float transitionTime;
    [SerializeField]
    private string nextSceneName;

    private Animator transition;

    private bool changingLevel=false;

    void Start()
    {
        transition = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        /*if (changingLevel)
        {
            Debug.LogWarning("next level jump");
            nextLevel();
        }*/
    }

    private void startMyLevelTransition() 
    {

        SceneManager.LoadScene(0);
        // StartCoroutine(nextLevel());
    }

    IEnumerator nextLevel()
    {
        if (transition != null)
        {
            transition.SetTrigger("startFade");
        }
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(nextSceneName);
    }
}
