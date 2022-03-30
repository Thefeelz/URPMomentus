using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrandonDialogueTrigger : MonoBehaviour
{
    bool triggered = false;
    [SerializeField] List<DialogueMessageInteractive> messages = new List<DialogueMessageInteractive>();
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponentInParent<DialogueSystem>() && !triggered)
        {
            other.GetComponentInParent<DialogueSystem>().AddMessageToDisplay(messages);
            triggered = true;
        }
        // Destroy(gameObject);
    }
}
