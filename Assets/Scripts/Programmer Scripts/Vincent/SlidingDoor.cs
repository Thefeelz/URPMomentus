using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    GameObject leftDoor, rightDoor;
    Vector3 leftDoorStartPos, rightDoorStartPos;
    bool isDoorOpen, firstClose=true;
    float currentDoorGap, closeDoorGap, openDoorGap;
    Vector3[] leftPos;
    float temp;

    //Who the door opens for
    [SerializeField]
    Collider target=null;
    
    //How wide the door opens
    [SerializeField]
    private float doorGap=1;
    
    //How quickly the door opens
    [SerializeField]
    private float doorSpeed =1;
    
    //Can make the door close when player gets close
    [SerializeField]
    private bool reverseDoor=false;

    

    // Start is called before the first frame update
    void Start()
    {
        //Gets the doors from the children
        leftDoor = transform.GetChild(0).gameObject;
        leftDoorStartPos = leftDoor.transform.localPosition;
        rightDoor = transform.GetChild(1).gameObject;
        rightDoorStartPos = rightDoor.transform.localPosition;

        //If player 
        if (target == null)
        {
            target = GameObject.Find("Body").GetComponent<Collider>();
            Debug.LogWarning("Door was not given target DEFAULT:" + target);
        }
       
        //closeDoorGap = Mathf.Abs(leftDoor.transform.localPosition.z - rightDoor.transform.localPosition.z);
        openDoorGap = Mathf.Abs(leftDoor.transform.localPosition.z - rightDoor.transform.localPosition.z) + doorGap;

        doorSpeed /= 100;
        if (reverseDoor)
        {
            openDoors();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //finds the current space between the doors
        currentDoorGap = Mathf.Abs(leftDoor.transform.localPosition.z - rightDoor.transform.localPosition.z);

        //open door motion
        ///every frame, the door's shutters lerp apart until they reach the set distance  
        if (isDoorOpen && currentDoorGap < openDoorGap)
        {
            leftDoor.transform.localPosition = Vector3.Lerp(leftDoor.transform.localPosition, leftDoor.transform.localPosition - new Vector3(0, 0, doorGap), doorSpeed);
            rightDoor.transform.localPosition = Vector3.Lerp(rightDoor.transform.localPosition, rightDoor.transform.localPosition + new Vector3(0, 0, doorGap), doorSpeed);
        }

        //close door motion
        ///if the door should be closed but is open. Every frame the doors will lerp towards each other until they are about .01 apart(which looks closed
        else if (!isDoorOpen && Vector3.Distance(leftDoor.transform.localPosition, leftDoorStartPos)>.01)
        {
            leftDoor.transform.localPosition = Vector3.Lerp(leftDoor.transform.localPosition, leftDoor.transform.localPosition + new Vector3(0, 0, doorGap), doorSpeed);
            rightDoor.transform.localPosition = Vector3.Lerp(rightDoor.transform.localPosition, rightDoor.transform.localPosition - new Vector3(0, 0, doorGap), doorSpeed);
        } 
    }

    //when player enters range
    private void OnTriggerEnter(Collider other)
    {
        if (other == target && reverseDoor)
        {
            closeDoors();
        }
        else if (other == target)
        {
            openDoors();
        }
    }

    //when player leaves range
    private void OnTriggerExit(Collider other)
    {
        if (other == target && reverseDoor)
        {
            openDoors();
        }
        else if (other == target)
        {
            closeDoors();
        }
    }

    //Changes bool based on if door should open or close
    void openDoors()
    {
        isDoorOpen = true;
    }
    void closeDoors()
    {
        isDoorOpen = false;
    }
}
