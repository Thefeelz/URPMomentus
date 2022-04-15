using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionControlController : MonoBehaviour
{

    [SerializeField]
    private GameObject missionControlObject;


    // Start is called before the first frame update
    void Start()
    {

        //missionControlObject.GetComponent<DialogueTrigger>().DTriggerDialogue();
        CallDialogueTrigger();
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnTriggerEnter(Collider collision)
    {
        //gets the current dialogue trigger on the object it's attached to and plays the dialogue.
        CallDialogueTrigger();
    }


    private void OnTriggerExit(Collider collision)
    {
        missionControlObject.GetComponent<BoxCollider>().enabled = false;
        missionControlObject.GetComponent<MissionControlController>().enabled = false;
    }

    public void CallDialogueTrigger()
    {
        missionControlObject.GetComponent<DialogueTrigger>().DTriggerDialogue();
    }
}

