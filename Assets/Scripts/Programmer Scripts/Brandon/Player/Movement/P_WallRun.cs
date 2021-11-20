using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class P_WallRun : MonoBehaviour
{
    public float minimumHeightForWallRun;
    [SerializeField] Transform wallRunCamPos;
    [SerializeField] float wallHugForce = 2f;
    [SerializeField] int wallRunLayer = 7;
    [SerializeField] float wallDetectionDistanceMultiplier = 2f;
    [SerializeField] float wallRunHeightOffset = 1f;
    Vector3[] directions;
    RaycastHit[] hits;

    Rigidbody playerBody;
    P_Movement playerMovement;
    public bool isWallRunning;
    public bool wallLeft;
    Vector3 lastWallPosition;
    Vector3 lastWallNormal;
    public float normalizedAngleThreshold = 0.1f;
    bool IsPlayerGrounded() => playerMovement.isGrounded;
    mouseLook cameraLook;
    public bool camRotated;

    [SerializeField] float cameraRollAmount = 20f;
    [SerializeField] float cameraTransitionTime = 0.5f;
    float cameraElapsedTime = 0f;

    bool VerticalCheck()
    {
        //return !Physics.Raycast(transform.position, Vector3.down, minimumHeightForWallRun);
        return true;
    }
    void Start()
    {
        playerBody = GetComponent<Rigidbody>();
        playerMovement = GetComponent<P_Movement>();
        cameraLook = GetComponent<mouseLook>();

        // Initialize Raycast Directions
        directions = new Vector3[]
        {
            Vector3.right,
            Vector3.right + Vector3.forward,
            Vector3.forward,
            Vector3.left + Vector3.forward,
            Vector3.left
        };
        for(int i = 0; i < directions.Length; i++)
        {
            directions[i] *= 1;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        isWallRunning = false;
        if (VerticalCheck())
        {
            hits = new RaycastHit[directions.Length];
            for (int i = 0; i < directions.Length; i++)
            {
                Vector3 dir = transform.TransformDirection(directions[i]);
                Physics.Raycast(transform.position + Vector3.up * wallRunHeightOffset, dir, out hits[i], wallDetectionDistanceMultiplier);
                /*if (hits[i].collider != null && hits[i].collider.gameObject.layer == wallRunLayer)
                {
                    Debug.DrawRay(transform.position + Vector3.up * wallRunHeightOffset, dir * hits[i].distance, Color.green);
                }
                else
                {
                    Debug.DrawRay(transform.position + Vector3.up * wallRunHeightOffset, dir * wallDetectionDistanceMultiplier, Color.red);
                }*/
            }
            hits = hits.ToList().Where(h => h.collider != null &&  h.collider.gameObject.layer == wallRunLayer).OrderBy(h => h.distance).ToArray();
            // If we enter this, we are wall running
            if (hits.Length > 0)
            {
                isWallRunning = true;
                lastWallPosition = hits[0].point;
                lastWallNormal = hits[0].normal;
                Vector3 cross = Vector3.Cross(transform.forward, hits[0].normal);
                float dir = Vector3.Dot(cross, transform.up);
                
                if (dir > 0)
                {
                    wallLeft = true;
                }
                else if (dir < 0)
                {
                    wallLeft = false;
                }
                else
                {
                    
                }
                OnWall(hits[0]);
            }
        }
        if (isWallRunning) 
        {
            WallRunningCamera();
        }
        else
        {

            cameraElapsedTime = 0f;
        }
    }

    void OnWall(RaycastHit hit)
    {
        float d = Vector3.Dot(hit.normal, Vector3.up);
        if (d >= -normalizedAngleThreshold && d <= normalizedAngleThreshold)
        {
            Vector3 alongWall = Vector3.Cross(hit.normal, transform.up);
            float vertical = Input.GetAxisRaw("Vertical");

            if (wallLeft)
            {
                playerBody.AddForce(-transform.right * wallHugForce *Time.deltaTime);
                playerBody.velocity = alongWall * vertical * 10f;
            }
            else
            {
                playerBody.AddForce(transform.right * wallHugForce * Time.deltaTime);
                playerBody.velocity = -alongWall *  vertical * 10f;
            }
            isWallRunning = true;
        }
    }
    void WallRunningCamera()
    {
        cameraElapsedTime += Time.deltaTime;
        playerMovement.isGrounded = true;
        if(!wallLeft)
        {
            Camera.main.transform.position = wallRunCamPos.position;
            Camera.main.transform.Rotate(Vector3.forward, Mathf.Lerp(0, cameraRollAmount, cameraElapsedTime / cameraTransitionTime));
        }
        else if (wallLeft)
        {
            Camera.main.transform.position = wallRunCamPos.position;
            Camera.main.transform.Rotate(Vector3.forward, Mathf.Lerp(0, -cameraRollAmount, cameraElapsedTime / cameraTransitionTime));
        }
    }
}
