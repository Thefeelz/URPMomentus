using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
 * Momentus Dialogue Handler
 * 
 * Purpose: To handle the dialogue queue of the game.
 * 
 * created by Divyansh Malik / 11/02/2021
 * 
 * Last Modded by: Divyansh Malik / 12/08/2021
 * 
 */


public class DialogueHandlerScript : MonoBehaviour
{

    [SerializeField]
    private TMP_Text dialogueText; //textbox to display the text

    [SerializeField]
    Animator missionControlAnimator; // refrence to close the animator

    [SerializeField]
    private Image CharacterImg;

    

    private Queue<string> sentences; // this variable holds all the sentences from the dialogue within it, you can access them in a FIFO order.

    //private Queue<string> printQueue;

    public bool isPlaying;


    //private string[] sentences2; // replacing the queue

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        isPlaying = true;
        
    }

    /// <summary>
    /// This function is supposed to take in a dialogue object which is an arrat of strings and put them within a queue. then after that it starts a coroutine which tells the handler to display the scentences
    /// to the User at a set period of time
    /// </summary>
    /// <param name="dialogue"></param>
    /// Input: the dialogue object which contains the name of the speaker and their script
    /// Output: None
    public void StartDialogue(Dialogue dialogue,bool hasImage)
    {

        if (hasImage == true)
        {
            displayImage(dialogue);
        }

        //sentences2 = new string[arraySize];
        //Debug.Log(sentences2.Length);

        //missionControlAnimator.SetBool("isOpen", true);
        //check if the conversation started
        Debug.Log("Starting conversation with " + dialogue.name);

        //clear all previous dialogues in the queue
        //sentences.Clear();

        //int i = 0;
        //loop, for each sentence in dialogue, queue them up within the handler's queue.
        foreach (string sentence in dialogue.scentences)
        {
            sentences.Enqueue(dialogue.name + ": " + sentence);
            //printQueue.Enqueue(sentence);
            //sentences2[i] = sentence;
            // i++;

        }

        //incase one is already animating
        StopAllCoroutines();

        //start displaying sentences
        StartCoroutine(DelayTextSequence());

        //Debug.Log(printQueue.Count);
    }

    private void displayImage(Dialogue dialogue)
    {
        CharacterImg.sprite = dialogue.characterProfilePic;
        missionControlAnimator.SetBool("dialogue", true);

    }

    private void DisplayNextSentence()
    {

        //if there are no sentences left, then end the dialogue and return
        if (sentences.Count == 0)
        {
           
            return;

        }

        //otherwise dequeue the next sentence and display it

        string sent = sentences.Dequeue();
        dialogueText.text = sent;
        Debug.Log(sent);




    }

    public void EndDialogue()
    {
        missionControlAnimator.SetBool("dialogue", false);
        dialogueText.text = "";
        CharacterImg.sprite = null;
        isPlaying = false;


    }

    //delays the sequence of the text and displays them
    IEnumerator DelayTextSequence()
    {
        for (int i = sentences.Count-1; i >= 0; --i)
        {
            //Debug.Log("conversation with " + i);

            DisplayNextSentence();
            yield return new WaitForSeconds(3);
        }
        EndDialogue();
    }

}

    
