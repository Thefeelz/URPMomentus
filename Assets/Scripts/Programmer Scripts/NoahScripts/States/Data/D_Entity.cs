using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "newEntityData", menuName = "Data/Entity Data/Base Data")]
public class D_Entity : ScriptableObject
{
    public float rapidDistance;
    public float slowDistance;
    public float aimDistance;
    public float evadeDistance;
    public float health;
}
