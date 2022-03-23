using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseLook : MonoBehaviour
{
    [SerializeField]
    private float mouseSens = 100f;
    [SerializeField] float lookRotation = 30f; 
    private float mouseMultiplier = 10f;
    private float horizontalRotation = 0f;
    private float verticalRotation = 0f;

    float cameraRollAmount;
    float cameraTransitionTime;
    float cameraElapsedTime;

    P_WallRun wallrun;
   
    // Start is called before the first frame update
    void Start()
    {
        mouseSens = FindObjectOfType<GameManager>().GetMouseSensitivity();
        //stops mouse from showing up
        Cursor.lockState = CursorLockMode.Locked;
        wallrun = GetComponent<P_WallRun>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //if (wallrun.isWallRunning) { return; }
            //get mouse position
        float mouseX = Input.GetAxis("Mouse X") * mouseSens * mouseMultiplier * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSens * mouseMultiplier * Time.deltaTime;
        
        //rotate left/right
        horizontalRotation += mouseX;
        

        //rotate up/down
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -lookRotation, lookRotation);
        
        
       // transform.localRotation = Quaternion.Euler(, 0f, 0f);
        transform.rotation = Quaternion.Euler(0, horizontalRotation, 0f);
        if(!wallrun.isWallRunning)
            Camera.main.transform.rotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0f);
        else
        {
            Camera.main.transform.rotation = Quaternion.Euler(verticalRotation, horizontalRotation, Camera.main.transform.rotation.z);
            Camera.main.transform.Rotate(Vector3.forward, Mathf.Lerp(0, cameraRollAmount, cameraElapsedTime / cameraTransitionTime));
        }
    }

    public void UpdateMouseSensitivity(float newSensitivity)
    {
        mouseSens = newSensitivity;
    }

    public void SetCameraWallRunRotate(float _cameraRollAmount, float _cameraElapsedTime, float _cameraTransitionTime)
    {
        cameraRollAmount = _cameraRollAmount;
        cameraElapsedTime = _cameraElapsedTime;
        cameraTransitionTime = _cameraTransitionTime;
    }
}
