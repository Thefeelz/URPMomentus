using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    Collider colliderz;
    // Start is called before the first frame update
    void Start()
    {
        colliderz = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<EnemyBullet>())
        {
            //other.GetComponent<Rigidbody>().velocity = -other.GetComponent<Rigidbody>().velocity;
            other.GetComponent<Rigidbody>().velocity = returnRandomVectorReverse(other.GetComponent<Rigidbody>().velocity);
        }
    }
    public void ActivateCollider() { colliderz.enabled = true; }
    public void DeactivateCollider() { colliderz.enabled = false; }

    private Vector3 returnRandomVectorReverse(Vector3 initialVelocity)
    {
        Vector3 vel = -initialVelocity;
        vel.x += Random.Range(-2, 2);
        vel.y += Random.Range(-2, 2);
        vel.z += Random.Range(-2, 2);
        return vel;

    }
}
