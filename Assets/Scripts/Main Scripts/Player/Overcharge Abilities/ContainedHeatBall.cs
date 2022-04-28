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
            other.GetComponentInParent<Rigidbody>().AddForce(knockbackDirection * 15f + (Vector3.up * 7), ForceMode.Impulse);
            return;
        }
        if(other.GetComponent<EnemyBullet>())
        {
            other.GetComponent<Rigidbody>().velocity = returnRandomVectorReverse(other.GetComponent<Rigidbody>().velocity);
            return;
        }

        if(other.GetComponentInParent<Entity>())
        {
            Vector3 knockbackDirection = other.transform.position - transform.position;
            knockbackDirection = knockbackDirection.normalized;
            other.GetComponentInParent<Rigidbody>().AddForce(knockbackDirection * 15f + (Vector3.up * 7), ForceMode.Impulse);
            return;
        }
    }

    private Vector3 returnRandomVectorReverse(Vector3 initialVelocity)
    {
        Vector3 vel = -initialVelocity;
        vel.x += Random.Range(-2, 2);
        vel.y += Random.Range(-2, 2);
        vel.z += Random.Range(-2, 2);
        return vel;

    }
}
