using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject myParent;
    public AmmoPool myPool;
    public float activeLimit; // limit on how long object can be dequeued
    public float timeActive; // time bullet was dequed
    public bool hit; // did bullet hit something
    public float damage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - timeActive >= activeLimit && hit == false)
        {
            dequeue();
        }
    }

    void dequeue()
    {
        myPool.enqueBullet(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Body")
        {
            hit = true;
            other.gameObject.GetComponentInParent<CharacterStats>().RemoveHealth(damage);
            dequeue();
        }
    }
}
