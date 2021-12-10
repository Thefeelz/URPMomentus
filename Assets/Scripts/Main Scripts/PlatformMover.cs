using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Touched by Vincent on 11/21/2021
/// </summary>
public class PlatformMover : MonoBehaviour
{
    // Start is called before the first frame update

    /*[SerializeField]
    private int numberOfPositions; // arraysize*/
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private Transform[] listOfPoints;

    private Transform start;

    private Transform targetPosition;

    private float speed = 0.1f;

    private float elapsedTime = 3f;

    private float tripTime = 5f;

    private Collider playerBody;


    int i = 0;

    void Start()
    {
        start = this.transform;

        targetPosition = listOfPoints[1];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //distance to target
        float gap = Vector3.Distance(this.transform.position, targetPosition.position);

        //if platform reaches target position (could be 0 distance but felt like that could cause issues later)
        if (gap < .001)
        {
            //increment up one
            i++;

            //if "i" is greater than array set i back to 0
            if (i > listOfPoints.Length)
            {
                i = 0;
            }

            //set new position as the target position
            targetPosition = listOfPoints[i];

        }


        float speed = 0; ///idk why this is needed, just is
        speed = speed + elapsedTime * Time.deltaTime;
        
        // move platform towards target position at given speed every frame
        this.transform.position = Vector3.MoveTowards(this.transform.position, targetPosition.position, speed); /// moveTowards seemed gentler than Lerp or Slerp


    }
    //parents player when on elevator
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root == player.transform) /// if top level parent of the object colliding with elvator trigger is the given player 
        {
            player.transform.SetParent(this.transform); /// set the elvator as the parent of the given player (elvator move, player moves)
        }
    }

    //de-parents player when leaving elevator
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.parent.parent == player.transform) /// can't use root again since that would be the enviroment now
        {
            player.transform.SetParent(null); /// removes the parent so that the player is on its own again
        }
    }
}
