using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] List<DialogueMessageInteractive> interactiveMessagesToDisplay = new List<DialogueMessageInteractive>();
    [SerializeField] TMP_Text textDisplay;
    [SerializeField] Image canvasImageForDialoge;
    [SerializeField] Animator dialogueAnimator;
    [SerializeField] GameObject skipMessage;
    Image skipMessageImage;
    int i = 0;
    GameObject callingObject;
    private void Start()
    {
        skipMessageImage = skipMessage.GetComponent<Image>();
    }

    /// <summary>
    /// Add a list of DialogueMessageInteractive to be displayed to the screen for player instruction.
    /// </summary>
    /// <param name="messages"></param>
    public void AddMessageToDisplay(List<DialogueMessageInteractive> messages)
    {
        if (messages.Count == 0) { return; }
        if (messages[0].overrideCurrentMessage && interactiveMessagesToDisplay.Count > 0)
        {
            interactiveMessagesToDisplay.Clear();
            StopAllCoroutines();
            interactiveMessagesToDisplay = messages;
            i = 0;
            canvasImageForDialoge.sprite = null;
        }
        else if(!messages[0].overrideCurrentMessage && interactiveMessagesToDisplay.Count > 0)
        {
            foreach (DialogueMessageInteractive newMessage in messages)
            {
                interactiveMessagesToDisplay.Add(newMessage);
            }
        }
        else
            interactiveMessagesToDisplay = messages;
        StartDisplayMessageInteractive();
    }

    public void AddMessageToDisplay(List<DialogueMessageInteractive> messages, GameObject _callingObject)
    {
        if(messages.Count == 0) { return; }
        callingObject = _callingObject;
        if (messages[0].overrideCurrentMessage && dialogueAnimator.GetBool("dialogue"))
        {
            FastForwardAbilityToggle(interactiveMessagesToDisplay);
            interactiveMessagesToDisplay.Clear();
            StopAllCoroutines();
            interactiveMessagesToDisplay = messages;
            i = 0;
            canvasImageForDialoge.sprite = null;
            StartDisplayMessageInteractive();
            return;
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

    void HandleAbilityTogglesInteractive(List<AbilityToManipulateObjectInteractive> daList)
    {
        foreach (AbilityToManipulateObjectInteractive thang in daList)
        {
            if (thang.turnOnAbility && callingObject.GetComponent<TurnOnAndOffScripts>())
                callingObject.GetComponent<TurnOnAndOffScripts>().TurnOnScripts(thang.abilitiesToManipulate);
            else if (!thang.turnOnAbility && callingObject.GetComponent<TurnOnAndOffScripts>())
                callingObject.GetComponent<TurnOnAndOffScripts>().TurnOffScripts(thang.abilitiesToManipulate);
        }
    }
    void HandleParticleSystemInteractive(List<DialogueToggleParticleSystem> camille)
    {
        foreach (DialogueToggleParticleSystem brandon in camille)
        {
            if (brandon.stopPlay && brandon.particle)
                brandon.particle.Stop();
            if (brandon.startPlay && brandon.particle)
                brandon.particle.Play();
        }
    }
    void HandleAnimatorSystemInteractive(List<DialogueToggleAnimation> rose)
    {
        foreach (DialogueToggleAnimation jon in rose)
        {
            if(jon.animator)
                jon.animator.SetBool(jon.boolName, jon.boolValue);
        }
    }
    void HandleGameObjectToggle(DialogueToggleGameObject[] daList)
    {
        foreach (DialogueToggleGameObject peter in daList)
        {
            if (peter.turnOnObject && peter.objectToManipulate)
                peter.objectToManipulate.SetActive(true);
            else if (peter.objectToManipulate && !peter.turnOnObject)
                peter.objectToManipulate.SetActive(false);
            
        }
    }

    void HandleObjectives(List<DialogueToggleObjective> objectives)
    {
        if(FindObjectOfType<ObjectiveHelper>())
        {
            ObjectiveHelper helper = FindObjectOfType<ObjectiveHelper>();
            foreach (DialogueToggleObjective holly in objectives)
            {
                if (holly.startObjective)
                    helper.AddObjectivesToDisplay(holly.objective);
                else
                    helper.RemoveObjectiveByID(holly.objective.objectiveID);
            }
        }
    }
    

    void StartDisplayMessageInteractive()
    {   
        if (i < interactiveMessagesToDisplay.Count)
        {
            canvasImageForDialoge.sprite = interactiveMessagesToDisplay[i].imageToDisplay;
            if (!interactiveMessagesToDisplay[i].ableToSkipMessage)
            {
                skipMessage.SetActive(false);
            }
            else
            {
                skipMessage.SetActive(true);
            }
            if (interactiveMessagesToDisplay[i].abilities.Count > 0)
                HandleAbilityTogglesInteractive(interactiveMessagesToDisplay[i].abilities);
            if (interactiveMessagesToDisplay[i].objectsToTurnOn.Length > 0)
                HandleGameObjectToggle(interactiveMessagesToDisplay[i].objectsToTurnOn);
            if (interactiveMessagesToDisplay[i].particleSystem.Count > 0)
                HandleParticleSystemInteractive(interactiveMessagesToDisplay[i].particleSystem);
            if (interactiveMessagesToDisplay[i].animations.Count > 0)
                HandleAnimatorSystemInteractive(interactiveMessagesToDisplay[i].animations);
            if (interactiveMessagesToDisplay[i].objectives.Count > 0)
                HandleObjectives(interactiveMessagesToDisplay[i].objectives);
            StartCoroutine(DisplayMessageInteractive(interactiveMessagesToDisplay[i]));
            i++;
        }
        else
        {
            StartCoroutine(TurnOffImage());
            textDisplay.text = "";
            i = 0;
        }
    }
    

    IEnumerator DisplayMessageInteractive(DialogueMessageInteractive message)
    {
        dialogueAnimator.SetBool("dialogue", true);
        canvasImageForDialoge.sprite = message.imageToDisplay;
        textDisplay.text = message.message;
        if (message.fontSize == 0)
            textDisplay.fontSize = 28;
        else
            textDisplay.fontSize = message.fontSize;
        textDisplay.color = message.fontColor;
        yield return new WaitForSeconds(message.timeToDisplay);
        StartDisplayMessageInteractive();
    }

    IEnumerator TurnOffImage()
    {
        dialogueAnimator.SetBool("dialogue", false);
        interactiveMessagesToDisplay.Clear();
        yield return new WaitForSeconds(2f);
        if (!dialogueAnimator.GetBool("dialogue"))
        {
            canvasImageForDialoge.sprite = null;
            skipMessage.SetActive(false);
        }
    }

    void FastForwardAbilityToggle(List<DialogueMessageInteractive> remainingMessages)
    {
        foreach (DialogueMessageInteractive item in remainingMessages)
        {
            HandleAbilityTogglesInteractive(item.abilities);
        }
    }

    public void SkipCurrentMessage()
    {
        /*if (!dialogueAnimator.GetBool("dialogue") || (i < interactiveMessagesToDisplay.Count && !interactiveMessagesToDisplay[i].ableToSkipMessage)) { return; }
        Debug.Log(i);
        if(i >= interactiveMessagesToDisplay.Count)
        {
            StartCoroutine(TurnOffImage());
            textDisplay.text = "";
            i = 0;
        }
        else if (interactiveMessagesToDisplay.Count > 0 && i < interactiveMessagesToDisplay.Count)
        {
            StopAllCoroutines();
            StartDisplayMessageInteractive();
        }*/
        if(skipMessage.activeSelf)
        {
            StopAllCoroutines();
            StartDisplayMessageInteractive();
        }
    }
}
