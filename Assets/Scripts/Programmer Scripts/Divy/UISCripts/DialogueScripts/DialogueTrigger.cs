using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Momentus Dialogue Trigger
 * 
 * Dependancies: MissionControlTrigger must me avaliable and ready.
 * 
 * Purpose: To be able to trigger the dialogue at the time which the animation of the mission control finishes opening, and then tell mission control to close when finished. if you want to add
 * a dialogue to your level then you must make sure that you place triggers for the mission control UI since it is dependant on that
 * 
 * created by Divyansh Malik / 11/02/2021
 * 
 * Modded by: Divyansh Malik / --/--/----
 * 
 */

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueHandlerScript>().StartDialogue(dialogue);
    }
}
