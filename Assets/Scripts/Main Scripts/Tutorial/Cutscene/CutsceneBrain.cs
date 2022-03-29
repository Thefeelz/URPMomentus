using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneBrain : MonoBehaviour
{
    [SerializeField] Animator canvasAnim;
    [SerializeField] GameObject skipButton;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SkipCutscene()
    {
        canvasAnim.SetBool("skipCutscene", true);
        skipButton.SetActive(false);
        StartCoroutine(SwitchScene()); 
    }

    IEnumerator SwitchScene()
    {
        yield return new WaitForSeconds(6f);
        GetComponent<SceneController>().GoToNextScene();
    }
}
