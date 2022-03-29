using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrandonDialogueTrigger : MonoBehaviour
{
    [SerializeField] List<DialogueMessage> messages = new List<DialogueMessage>();
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponentInParent<DialogueSystem>())
        {
            other.GetComponentInParent<DialogueSystem>().AddMessageToDisplay(messages);
        }
        Destroy(gameObject);
    }
}
