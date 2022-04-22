using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueMessageInteractive
{
    public DialogueSpeaker dialogueSpeaker;
    public int fontSize = 28;
    [TextArea(3, 10)]
    public string message;
    public float timeToDisplay;
    public bool overrideCurrentMessage;
    public bool ableToSkipMessage = true;
    public List<AbilityToManipulateObjectInteractive> abilities = new List<AbilityToManipulateObjectInteractive>();
    public DialogueToggleGameObject[] objectsToTurnOn;
    public List<DialogueToggleAnimation> animations = new List<DialogueToggleAnimation>();
    public List<DialogueToggleParticleSystem> particleSystem = new List<DialogueToggleParticleSystem>();
    public List<DialogueToggleObjective> objectives = new List<DialogueToggleObjective>();
    public float healthToAdd = 0f;
    public float overchargeToAdd = 0f;
}
