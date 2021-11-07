using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Momentus Dialogue Handler
 * 
 * Purpose: To handle the dialogue queue of the game.
 * 
 * created by Divyansh Malik / 11/02/2021
 * 
 * Last Modded by: Divyansh Malik / 11/03/2021
 * 
 */


public class DialogueHandlerScript : MonoBehaviour
{

    [SerializeField]
    private Text dialogueText;

    [SerializeField]
    Animator closeAnimation;

    private Queue<string> sentences; // this variable holds all the sentences within it, you can access them in a FIFO order.

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        
    }

    public void StartDialogue(Dialogue dialogue)
    {
        //check if the conversation started
        Debug.Log("Starting conversation with " + dialogue.name);

        //clear all previous dialogues in the queue
        sentences.Clear();

        //loop, for each sentence in dialogue, queue them up within the handler's queue.
        foreach (string sentence in dialogue.scentences) 
        {
            sentences.Enqueue(sentence);

        }

        //incase one is already animating
        StopAllCoroutines();

        //start displaying sentences
        StartCoroutine(DelayTextSequence());
        

    }

    public void DisplayNextSentence()
    {
        
        //if there are no sentences left, then end the dialogue and return
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;

        }

        //otherwise dequeue the next sentence and display it
        string sent = sentences.Dequeue();
        dialogueText.text = sent;
        
       

    }

    public void EndDialogue()
    {
        dialogueText.text = "";
        closeAnimation.SetBool("isOpen", false);

    }

    //delays the sequence of the text and displays them
    IEnumerator DelayTextSequence()
    {
        for(int i=sentences.Count; i >= 0;--i)
        {
            DisplayNextSentence();
            yield return new WaitForSeconds(5);
        }
        
    }
}
