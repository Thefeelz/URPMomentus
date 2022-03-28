using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] List<DialogueMessage> messagesToDisplay = new List<DialogueMessage>();
    [SerializeField] List<DialogueMessageInteractive> interactiveMessagesToDisplay = new List<DialogueMessageInteractive>();
    [SerializeField] TMP_Text textDisplay;
    [SerializeField] Image canvasImageForDialoge;
    [SerializeField] Animator dialogueAnimator;
    int i = 0;
    GameObject callingObject;
    // Start is called before the first frame update
    void Start()
    {
        /*
        if (messagesToDisplay.Count > 0)
            StartDisplayMessage();
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddMessageToDisplay(List<DialogueMessage> messages)
    {
        if (messages[0].overrideCurrentMessage && dialogueAnimator.GetBool("dialogue"))
        {
            messagesToDisplay.Clear();
            StopAllCoroutines();
        }
        messagesToDisplay = messages;
        if (messages.Count > 0 && dialogueAnimator.GetBool("dialogue"))
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

    public void AddMessageToDisplay(List<DialogueMessageInteractive> messages)
    {
        if (messages[0].overrideCurrentMessage && dialogueAnimator.GetBool("dialogue"))
        {
            messagesToDisplay.Clear();
            StopAllCoroutines();
        }
        interactiveMessagesToDisplay = messages;
        if (messages.Count > 0 && dialogueAnimator.GetBool("dialogue"))
        {
            canvasImageForDialoge.sprite = messages[i].imageToDisplay;
            StartDisplayMessageInteractive();
        }
        else
        {
            canvasImageForDialoge.sprite = null;
            StartDisplayMessageInteractive();
        }
    }

    public void AddMessageToDisplay(List<DialogueMessage> messages, GameObject _callingObject)
    {
        callingObject = _callingObject;
        if (messages[0].overrideCurrentMessage && dialogueAnimator.GetBool("dialogue"))
        {
            messagesToDisplay.Clear();
            StopAllCoroutines();
        }
        messagesToDisplay = messages;
        if (messages.Count > 0 && dialogueAnimator.GetBool("dialogue"))
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

    public void AddMessageToDisplay(List<DialogueMessageInteractive> messages, GameObject _callingObject)
    {
        callingObject = _callingObject;
        if (messages[0].overrideCurrentMessage && dialogueAnimator.GetBool("dialogue"))
        {
            messagesToDisplay.Clear();
            StopAllCoroutines();
        }
        interactiveMessagesToDisplay = messages;
        if (messages.Count > 0 && dialogueAnimator.GetBool("dialogue"))
        {
            canvasImageForDialoge.sprite = messages[i].imageToDisplay;
            StartDisplayMessageInteractive();
        }
        else
        {
            canvasImageForDialoge.sprite = null;
            StartDisplayMessageInteractive();
        }
    }

    void HandleAbilityToggles(List<AbilityToManipulateObject> daList)
    {
        foreach (AbilityToManipulateObject thang in daList)
        {
            if (thang.turnOnAbility)
                callingObject.GetComponent<TurnOnAndOffScripts>().TurnOnScripts(thang.abilitiesToManipulate);
            else
                callingObject.GetComponent<TurnOnAndOffScripts>().TurnOffScripts(thang.abilitiesToManipulate);
        }
    }

    void HandleAbilityToggles(List<AbilityToManipulateObjectInteractive> daList)
    {
        Debug.LogError("Should not reach here");
        foreach (AbilityToManipulateObjectInteractive thang in daList)
        {
            if (thang.turnOnAbility)
                callingObject.GetComponent<TurnOnAndOffScripts>().TurnOnScripts(thang.abilitiesToManipulate);
            else
                callingObject.GetComponent<TurnOnAndOffScripts>().TurnOffScripts(thang.abilitiesToManipulate);
        }
    }
    void HandleGameObjectToggle(DialogueToggleGameObject[] daList)
    {
        foreach (DialogueToggleGameObject peter in daList)
        {
            if (peter.turnOnObject)
                peter.objectToManipulate.SetActive(true);
            else
                peter.objectToManipulate.SetActive(false);
        }
    }
    void StartDisplayMessage()
    {
        if(canvasImageForDialoge.sprite == null)
        {
            canvasImageForDialoge.sprite = messagesToDisplay[i].imageToDisplay;
            dialogueAnimator.SetBool("dialogue", true);
        }
        if (i < messagesToDisplay.Count)
        {
            if(messagesToDisplay[i].abilities.Count > 0)
                HandleAbilityToggles(messagesToDisplay[i].abilities);
            if (messagesToDisplay[i].objectsToTurnOn.Length > 0)
                HandleGameObjectToggle(messagesToDisplay[i].objectsToTurnOn);
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

    void StartDisplayMessageInteractive()
    {
        if (canvasImageForDialoge.sprite == null)
        {
            canvasImageForDialoge.sprite = interactiveMessagesToDisplay[i].imageToDisplay;
            dialogueAnimator.SetBool("dialogue", true);
        }
        if (i < interactiveMessagesToDisplay.Count)
        {
            // Debug.Log("Ability Count: " + interactiveMessagesToDisplay[i].abilities.Count + " Object Count: " + interactiveMessagesToDisplay[i].objectsToTurnOn.Length);
            if (interactiveMessagesToDisplay[i].abilities.Count > 0)
                HandleAbilityToggles(interactiveMessagesToDisplay[i].abilities);
            if (interactiveMessagesToDisplay[i].objectsToTurnOn.Length > 0)
                HandleGameObjectToggle(interactiveMessagesToDisplay[i].objectsToTurnOn);
            StartCoroutine(DisplayMessageInteractive(interactiveMessagesToDisplay[i].timeToDisplay, interactiveMessagesToDisplay[i].message));
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

    IEnumerator DisplayMessageInteractive(float displayTime, string currentMessage)
    {
        textDisplay.text = currentMessage;
        yield return new WaitForSeconds(displayTime);
        StartDisplayMessageInteractive();
    }

    IEnumerator TurnOffImage()
    {
        yield return new WaitForSeconds(2f);
        canvasImageForDialoge.sprite = null;
    }
}
