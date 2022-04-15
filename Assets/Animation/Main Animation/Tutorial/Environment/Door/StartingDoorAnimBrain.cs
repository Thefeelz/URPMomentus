using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingDoorAnimBrain : MonoBehaviour
{
    [SerializeField] DialogueSystem dialogueSystem;
    [SerializeField] List<DialogueMessage> messageList = new List<DialogueMessage>();
    [SerializeField] List<DialogueMessageInteractive> messageInteractive = new List<DialogueMessageInteractive>();
    [SerializeField] List<DialogueMessageInteractive> messageInteractive2 = new List<DialogueMessageInteractive>();

    float elapsedTime = 0f;
    float delayTime = 0.5f;
    bool messageSent = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (!messageSent && elapsedTime >= delayTime)
        {
            SendFirstMessage();
            messageSent = true;
        }
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
