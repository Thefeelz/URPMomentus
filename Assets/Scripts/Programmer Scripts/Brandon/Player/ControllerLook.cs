using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controllerLook : MonoBehaviour
{
    [SerializeField]
    private float mouseSens = 1f;
    private float mouseMultiplier = 1f;
    private float horizontalRotation = 0f;
    private float verticalRotation = 0f;
   
    // Start is called before the first frame update
    void Start()
    {
        //stops mouse from showing up
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //get the right thumbstick axis
        float mouseX = Input.GetAxis("cntrlLookLeft") * mouseSens * mouseMultiplier; // Gets the right stick X axis
        float mouseY = Input.GetAxis("cntrlLookDown") * mouseSens * mouseMultiplier; // Gets the right stick Y axis

        //rotate left/right
        horizontalRotation += mouseX; // @Isaac 
        

        //rotate up/down
        verticalRotation -= mouseY; 
        verticalRotation = Mathf.Clamp(verticalRotation, -30f, 30f); // @Isaac this keeps the camera from spinning infinitely


        // transform.localRotation = Quaternion.Euler(, 0f, 0f);
        transform.rotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0f);
    }
}
