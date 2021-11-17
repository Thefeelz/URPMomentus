using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainedHeatBall : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponentInParent<EnemyChaseState>())
        {
            Vector3 knockbackDirection = other.ClosestPoint(transform.position) - transform.position;
            knockbackDirection = knockbackDirection.normalized;
            other.GetComponentInParent<Rigidbody>().AddForce(knockbackDirection * 10f, ForceMode.Impulse);
        }
    }
}
