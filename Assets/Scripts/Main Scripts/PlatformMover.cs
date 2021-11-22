using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMover : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private int numberOfPositions; // arraysize

    [SerializeField]
    private Vector3[]  listOfPoints;

    [SerializeField]
    private GameObject platform;

    private float midpoint = 0.5f;

    private Transform startPosition;

    private Transform endPosition;


    void Start()
    {
         listOfPoints = new Vector3[numberOfPositions];
        
    }

    // Update is called once per frame
    void Update()
    {

        for (int i = 0; i <  listOfPoints.Length; ++i)
        {
            int firstPosition = 0;
            startPosition.position = listOfPoints[i];

            if ((i + 1) <  listOfPoints.Length)
            {
                endPosition.position = listOfPoints[firstPosition];
                transform.position = Vector3.Lerp(startPosition.position, endPosition.position, midpoint);

            }

            endPosition.position = listOfPoints[i + 1];
            transform.position = Vector3.Lerp( listOfPoints[i],  listOfPoints[0], midpoint);

        }

    }
}
