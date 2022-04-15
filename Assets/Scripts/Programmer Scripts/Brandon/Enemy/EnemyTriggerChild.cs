using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTriggerChild : MonoBehaviour
{
    public void AlertParentGroupOfDeath()
    {
        GetComponentInParent<EnemyTriggerGroup>().UpdateTriggerGroup(GetComponent<EnemyStats>());
    }
}
