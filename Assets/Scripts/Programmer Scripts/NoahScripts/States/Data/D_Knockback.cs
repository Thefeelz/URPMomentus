using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "knockbackData", menuName = "Data/State Data/Knockback Data")]
public class D_Knockback : ScriptableObject
{
    public float distance;
    public float height;
    public float speed;
}
