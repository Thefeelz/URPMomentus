using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Message", menuName = "MessageCreator/CreateMessage", order = 1)]
public class DialogueMessage : ScriptableObject
{
    public Sprite imageToDisplay;
    public string message;
    public float timeToDisplay;
    public bool overrideCurrentMessage;
}
