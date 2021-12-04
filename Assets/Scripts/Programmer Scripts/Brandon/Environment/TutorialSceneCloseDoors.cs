using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSceneCloseDoors : MonoBehaviour
{
    [SerializeField] float delayToStartCloseDoorsAnimation;
    [SerializeField] Animator closeDoorsAnimator, fogWindowsAnimator;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CloseDoors());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator CloseDoors()
    {
        yield return new WaitForSeconds(delayToStartCloseDoorsAnimation);
        Debug.Log("Called");
        closeDoorsAnimator.SetBool("openShutters", false);
        closeDoorsAnimator.SetBool("closeShutters", true);
    }
    public void OpenShuttersFalse() { closeDoorsAnimator.SetBool("openShutters", false); }
}
