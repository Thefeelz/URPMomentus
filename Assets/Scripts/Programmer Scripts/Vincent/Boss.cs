using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField]
    private Transform[] spot;
    [SerializeField]
    private Vector3 nextLocation;

    private int totalLocations, localIndex;
    // Start is called before the first frame update
    void Start()
    {
        nextLocation = spot[0].transform.position;
        totalLocations = spot.Length;
        localIndex = 1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (Vector3.Distance(this.transform.position, nextLocation)<1)
        {
            localIndex++;
            if (localIndex > totalLocations)
            {
                localIndex = 0;
            }
            nextLocation = spot[localIndex].transform.position;
        }

        float temp=0;
        temp = temp + .5f * Time.deltaTime;
        this.transform.position=Vector3.Lerp(transform.position, nextLocation, temp );
        
    }
}
