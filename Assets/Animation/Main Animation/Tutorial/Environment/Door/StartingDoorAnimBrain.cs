using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingDoorAnimBrain : MonoBehaviour
{
    [SerializeField] DialogueSystem dialogueSystem;
    [SerializeField] List<DialogueMessage> messageList = new List<DialogueMessage>();
    [SerializeField] List<DialogueMessageInteractive> messageInteractive = new List<DialogueMessageInteractive>();
    [SerializeField] List<DialogueMessageInteractive> messageInteractive2 = new List<DialogueMessageInteractive>();
    // Start is called before the first frame update
    void Start()
    {
        SendFirstMessage();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SendFirstMessage()
    {
        dialogueSystem.AddMessageToDisplay(messageInteractive, this.gameObject);
    }

    public void SendSecondMesage()
    {
        dialogueSystem.AddMessageToDisplay(messageInteractive2, this.gameObject);
    }
}
