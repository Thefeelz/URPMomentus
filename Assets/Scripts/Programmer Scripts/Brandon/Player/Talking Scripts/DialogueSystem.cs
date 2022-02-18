using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] DialogueMessage[] messagesToDisplay;
    [SerializeField] TMP_Text textDisplay;
    [SerializeField] Image canvasImageForDialoge;
    [SerializeField] Animator dialogueAnimator;
    int i = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (messagesToDisplay.Length > 0)
            StartDisplayMessage();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddMessageToDisplay(DialogueMessage[] messages)
    {
        i = 0;
        Array.Clear(messagesToDisplay, 0, messagesToDisplay.Length);
        StopAllCoroutines();
        messagesToDisplay = messages;
        if (messages.Length > 0 && dialogueAnimator.GetBool("dialogue"))
        {
            canvasImageForDialoge.sprite = messages[i].imageToDisplay;
            StartDisplayMessage();
        }
        else
        {
            canvasImageForDialoge.sprite = null;
            StartDisplayMessage();
        }
    }

    void StartDisplayMessage()
    {
        if(canvasImageForDialoge.sprite == null)
        {
            canvasImageForDialoge.sprite = messagesToDisplay[i].imageToDisplay;
            dialogueAnimator.SetBool("dialogue", true);
        }
        if (i < messagesToDisplay.Length)
        {
            StartCoroutine(DisplayMessage(messagesToDisplay[i].timeToDisplay, messagesToDisplay[i].message));
            i++;
        }
        else
        {
            dialogueAnimator.SetBool("dialogue", false);
            StartCoroutine(TurnOffImage());
            textDisplay.text = "";
            i = 0;
        }
    }
    IEnumerator DisplayMessage(float displayTime, string currentMessage)
    {
        textDisplay.text = currentMessage;
        yield return new WaitForSeconds(displayTime);
        StartDisplayMessage();
    }

    IEnumerator TurnOffImage()
    {
        yield return new WaitForSeconds(2f);
        canvasImageForDialoge.sprite = null;
    }
}