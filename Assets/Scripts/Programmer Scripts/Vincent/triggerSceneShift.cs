using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class triggerSceneShift : MonoBehaviour
{
    
    [SerializeField]
    private float transitionTime;

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
    private void OnTriggerEnter()
    {
        changingLevel = true;
        StartCoroutine(nextLevel());
        
    }
    IEnumerator nextLevel()
    {
        Debug.LogWarning("next level jump");
        transition.SetTrigger("startFade");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene("end screen");
    }
}
