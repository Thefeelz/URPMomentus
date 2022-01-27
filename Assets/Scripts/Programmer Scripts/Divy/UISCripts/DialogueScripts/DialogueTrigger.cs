using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Momentus Dialogue Trigger
 * 
 * Dependancies: MissionControlTrigger must me avaliable and ready.
 * 
 * Purpose: To be able to trigger the dialogue at the time which the animation of the mission control finishes opening, and then tell mission control to close when finished. if you want to add
 * a dialogue to your level then you must make sure that you place triggers for the mission control UI since it is dependant on that.
 * temporary solution for sgx, will get a better version later on.
 * 
 * created by Divyansh Malik / 11/02/2021
 * 
 * Modded by: Divyansh Malik / 12/07/2021
 * 
 */

public class DialogueTrigger : MonoBehaviour
{
    //public Dialogue dialogue;
    [SerializeField]
    private Dialogue[] DialogueArray;

 
    /// <summary>
    /// Purpose: To take a refrence of a dialogue object for the dialogue handler send it to the handler sot hat it then print out the entire dialogue sequence to the user
    /// </summary>
    /// <param name="element">
    /// this is the element of the array that the user wants to display.
    /// </param>
    /// <param name="isConversation">
    /// this is a boolean variable that tells the trigger that when true it should go through the entire array
    /// </param>
    /// Input: the element of the array that the user wants to trigger and if the user wants to trigger an entire conversation instead
    /// Output:none
    public void DTriggerDialogue()
    {

        if (DialogueArray.Length > 0)
        {

            StartCoroutine(waitForCharacterToFinish()); // starts the dialogue
         

        }

    }

    IEnumerator waitForCharacterToFinish()
    {
        for (int i = 0; i < DialogueArray.Length; ++i)
        {
            Debug.Log(DialogueArray[i].name + "before");

            FindObjectOfType<DialogueHandlerScript>().StartDialogue(DialogueArray[i],DialogueArray[i].HasImage);
            yield return new WaitWhile(() => FindObjectOfType<DialogueHandlerScript>().isPlaying);
            yield return new WaitForSeconds(2);
        }

        
    }

}

