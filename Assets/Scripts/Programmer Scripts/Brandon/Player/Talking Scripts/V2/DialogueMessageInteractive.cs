using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueMessageInteractive
{
    public Sprite imageToDisplay;
    [TextArea(3, 10)]
    public string message;
    public float timeToDisplay;
    public bool overrideCurrentMessage;
    public List<AbilityToManipulateObjectInteractive> abilities = new List<AbilityToManipulateObjectInteractive>();
    public DialogueToggleGameObject[] objectsToTurnOn;
    public List<DialogueToggleAnimation> animations = new List<DialogueToggleAnimation>();
    public List<DialogueToggleParticleSystem> particleSystem = new List<DialogueToggleParticleSystem>();
    public List<DialogueToggleObjective> objectives = new List<DialogueToggleObjective>();
}
