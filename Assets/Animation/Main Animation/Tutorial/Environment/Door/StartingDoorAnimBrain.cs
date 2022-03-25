using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingDoorAnimBrain : MonoBehaviour
{
    [SerializeField] DialogueSystem dialogueSystem;
    [SerializeField] List<DialogueMessage> messageList = new List<DialogueMessage>();
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
        dialogueSystem.AddMessageToDisplay(messageList);
    }
}
