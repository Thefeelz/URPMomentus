using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJumpCollider : MonoBehaviour
{
    [SerializeField] Transform targetPosition;
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponentInParent<EnemyChaseState>())
        {
            
            other.GetComponentInParent<EnemyChaseState>().SetStateToJumpCollider(targetPosition.position);
        }
    }
}
