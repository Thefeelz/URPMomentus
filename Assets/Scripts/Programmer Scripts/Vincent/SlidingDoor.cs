using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    GameObject leftDoor, rightDoor;
    bool isDoorOpen;
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
        rightDoor = transform.GetChild(1).gameObject;

        //If player 
        if (target == null)
        {
            target = GameObject.Find("Body").GetComponent<Collider>();
            Debug.LogWarning("Door was not given target DEFAULT:" + target);
        }
       

        closeDoorGap = Mathf.Abs(leftDoor.transform.position.z - rightDoor.transform.position.z);
        openDoorGap = Mathf.Abs(leftDoor.transform.position.z - rightDoor.transform.position.z) + doorGap;

        doorSpeed /= 100;
        if (reverseDoor)
        {
            openDoors();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentDoorGap = Mathf.Abs(leftDoor.transform.position.z - rightDoor.transform.position.z);
        if (isDoorOpen && currentDoorGap < openDoorGap)
        {
            leftDoor.transform.position = Vector3.Lerp(leftDoor.transform.position, leftDoor.transform.position - new Vector3(0, 0, doorGap), doorSpeed);
            rightDoor.transform.position = Vector3.Lerp(rightDoor.transform.position, rightDoor.transform.position + new Vector3(0, 0, doorGap), doorSpeed);
            Debug.Log(openDoorGap + " = " + currentDoorGap);
        }
        else if (!isDoorOpen && currentDoorGap > closeDoorGap)
        {
            leftDoor.transform.position = Vector3.Lerp(leftDoor.transform.position, leftDoor.transform.position + new Vector3(0, 0, doorGap), doorSpeed);
            rightDoor.transform.position = Vector3.Lerp(rightDoor.transform.position, rightDoor.transform.position - new Vector3(0, 0, doorGap), doorSpeed);
            Debug.Log(closeDoorGap + " = " + currentDoorGap);
        } 
    }
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

    void openDoors()
    {
        isDoorOpen = true;
        Debug.Log("The door is open");
    }
    void closeDoors()
    {
        isDoorOpen = false;
        Debug.Log("The door is closed");
    }
}
