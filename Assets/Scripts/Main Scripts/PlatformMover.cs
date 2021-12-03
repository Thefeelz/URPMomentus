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




    int i = 0;

    void Start()
    {
        start = this.transform;

        targetPosition = listOfPoints[1];
        //listOfPoints = new Transform[numberOfPositions];
        //platform = this.gameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float gap = Vector3.Distance(this.transform.position, targetPosition.position);
        if (gap < .001)
        {
            i++;
            if (i > listOfPoints.Length)
            {
                i = 0;
            }
            targetPosition = listOfPoints[i];

        }


        float speed = 0;
        speed = speed + elapsedTime * Time.deltaTime;
        this.transform.position = Vector3.MoveTowards(this.transform.position, targetPosition.position, speed);
      

    }
    //parents objects when on elevator
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject==player)
        {   
            player.GetComponent<Collider>().transform.SetParent(this.transform);
            Debug.Log("Parented");
        }
    }
    //de-parents objects when leave elevator
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            player.GetComponent<Collider>().transform.SetParent(null);
            Debug.Log("Moved out: ");
        }
    }
}
