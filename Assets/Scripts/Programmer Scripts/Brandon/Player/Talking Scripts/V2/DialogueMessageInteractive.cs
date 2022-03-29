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
}