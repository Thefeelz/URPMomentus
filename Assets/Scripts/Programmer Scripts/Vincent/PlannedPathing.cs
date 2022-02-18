using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlannedPathing : MonoBehaviour
{
    [SerializeField]
    private Transform[] spot;

    [SerializeField]
    private bool pauseAtPoints;

    [SerializeField]
    private float speed, delayDuration;

    private Vector3 nextLocation;
    private int totalLocations, localIndex;    
    private bool inDelay = false;

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
        if (Vector3.Distance(this.transform.position, nextLocation) < 1)
        {
            if (!inDelay)
            {
                StartCoroutine(pointTransition());
            }
        }

        float temp = 0;
        temp = temp + speed * Time.deltaTime;
        this.transform.position = Vector3.MoveTowards(transform.position, nextLocation, temp);
    }

    //Boss pauses before setting new destination
    IEnumerator pointTransition()
    {
        //Stops an infinite call till new point is set
        inDelay = true; 
        
        //IfWill pause for set delayDuration
        if (pauseAtPoints)
        {
            yield return new WaitForSeconds(delayDuration);
        }
        //Increment position or go back to first position
        localIndex++;
        if (localIndex > totalLocations - 1)
        {
            localIndex = 1;
        }

        //Set target for where to move based on transform in array
        nextLocation = spot[localIndex].transform.position;

        //let function be called again
        inDelay = false;
    }
}
