using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialElevatorBrain : MonoBehaviour
{
    [SerializeField] List<GameObject> elevatorDoor = new List<GameObject>();
    [SerializeField] Material startingMat;
    int i = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TurnOnElevatorDoor()
    {
        if(i >= elevatorDoor.Count) { return; }
        elevatorDoor[i].SetActive(true);
        elevatorDoor[i].GetComponent<TutorialElevatorDoor>().TurnOnDoorFromBrain(3f, startingMat);
        //elevatorDoor[i].GetComponent<TutorialElevatorDoor>().TurnOnDoorFromBrainLame(endingMat);
        i++;
    }
}
