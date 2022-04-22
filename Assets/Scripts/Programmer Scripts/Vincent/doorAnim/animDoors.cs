using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animDoors : MonoBehaviour
{
    public bool locked;
    GameObject leftDoor, rightDoor;
    Vector3 leftDoorStartPos, rightDoorStartPos;
    bool isDoorOpen;
    float currentDoorGap, closeDoorGap, openDoorGap;
    Vector3[] leftPos;
    Animator anim;



    //Who the door opens for
    [SerializeField]
    Collider target = null;

    //Can make the door close when player gets close
    [SerializeField]
    private bool reverseDoor = false;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

    

        //Get player as default
        if (target == null)
        {
            target = GameObject.Find("Body").GetComponent<Collider>();
            Debug.LogWarning("Door was not given target DEFAULT:" + target);
        }

        if (reverseDoor)
        {
            openDoors();
        }
    }

    //play sound when player enters the trigger
    private void OnTriggerEnter(Collider other)
    {

        if (other == target && reverseDoor && !locked)
        {
            closeDoors();
        }
        else if (other == target && !locked)
        {
            openDoors();
        }

    }

 

    //when player leaves range
    private void OnTriggerExit(Collider other)
    {

        if (other == target && reverseDoor && !locked)
        {
            openDoors();
        }
        else if (other == target && !locked)
        {
            closeDoors();
        }

    }

    //Changes bool based on if door should open or close
    public void openDoors()
    {
        anim.SetBool("isDoorOpen", true);
        //Debug.Log("A door is open");

    }
    public void closeDoors()
    {
        anim.SetBool("isDoorOpen", false);
        //Debug.Log("A door is closed");
    }

    //will lock the doors
    public void shouldDoorsLock(bool key)
    {
        locked = key;
    }
}
