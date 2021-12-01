using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainedHeatBall : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponentInParent<EnemyChaseState>())
        {
            other.GetComponentInParent<EnemyChaseState>().SetStateToKnockback();
            Vector3 knockbackDirection = other.transform.position - transform.position;
            knockbackDirection = knockbackDirection.normalized;
            other.GetComponentInParent<Rigidbody>().AddForce(knockbackDirection * 10f + (Vector3.up * 5), ForceMode.Impulse);
        }
    }
}
