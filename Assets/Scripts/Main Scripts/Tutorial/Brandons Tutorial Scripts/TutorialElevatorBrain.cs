using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialElevatorBrain : MonoBehaviour
{
    [SerializeField] List<GameObject> elevatorDoor = new List<GameObject>();
    [SerializeField] Material startingMat;
    [SerializeField] Collider thang;
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
        if(i >= elevatorDoor.Count) 
        {
            TurnOnCollider();
            return; 
        }
        elevatorDoor[i].SetActive(true);
        elevatorDoor[i].GetComponent<TutorialElevatorDoor>().TurnOnDoorFromBrain(3f, startingMat);
        //elevatorDoor[i].GetComponent<TutorialElevatorDoor>().TurnOnDoorFromBrainLame(endingMat);
        i++;
    }
    private void TurnOnCollider()
    {
        if (thang)
            thang.gameObject.SetActive(true);
    }
}
