using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newEntityData", menuName = "Data/State Data/Slow Data")]
public class D_SlowApproach : ScriptableObject
{
    public float speed;
    public bool circleStart; // bool to check if the enemy has started to circle
}
