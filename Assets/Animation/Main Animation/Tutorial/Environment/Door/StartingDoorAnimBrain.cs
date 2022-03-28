using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingDoorAnimBrain : MonoBehaviour
{
    [SerializeField] DialogueSystem dialogueSystem;
    [SerializeField] List<DialogueMessage> messageList = new List<DialogueMessage>();
    [SerializeField] List<DialogueMessageInteractive> messageInteractive = new List<DialogueMessageInteractive>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SendFirstMessage()
    {
        Debug.Log(messageInteractive.Count + "dabnjodnabdl");
        dialogueSystem.AddMessageToDisplay(messageInteractive, this.gameObject);
    }
}
