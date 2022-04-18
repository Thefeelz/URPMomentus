using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName ="Speaker", menuName ="Dialogue/Speaker", order =1)]
public class DialogueSpeaker : ScriptableObject
{
    public Sprite speakerPortait;
    public TMP_FontAsset fontToUse;
    public string speakerName;
    public Color colorForText;
}
