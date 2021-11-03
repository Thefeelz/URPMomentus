using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class P_WallRun : MonoBehaviour
{
    public float minimumHeightForWallRun;
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

    bool VerticalCheck()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minimumHeightForWallRun);
    }
    void Start()
    {
        playerBody = GetComponent<Rigidbody>();
        playerMovement = GetComponent<P_Movement>();

        // Initialize Raycast Directions
        directions = new Vector3[]
        {
            Vector3.right,
            Vector3.right + Vector3.forward,
            Vector3.forward,
            Vector3.left + Vector3.forward,
            Vector3.left
        };
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
                Physics.Raycast(transform.position, dir, out hits[i], 1);
                if (hits[i].collider != null && hits[i].transform.GetComponent<WallRunnable>())
                {
                    Debug.DrawRay(transform.position, dir * hits[i].distance, Color.green);
                }
                else
                {
                    Debug.DrawRay(transform.position, dir * 1, Color.red);
                }
            }
            hits = hits.ToList().Where(h => h.collider != null && h.transform.GetComponent<WallRunnable>()).OrderBy(h => h.distance).ToArray();
            // If we enter this, we are wall running
            if (hits.Length > 0)
            {
                isWallRunning = true;
                OnWall(hits[0]);
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
                    Debug.Log("Yah Fucked Up I guess");
                }
            }
        }
        if (isWallRunning) { playerMovement.isGrounded = true; }
    }

    void OnWall(RaycastHit hit)
    {
        float d = Vector3.Dot(hit.normal, Vector3.up);
        if (d >= -normalizedAngleThreshold && d <= normalizedAngleThreshold)
        {
            // Vector3 alongWall = Vector3.Cross(hit.normal, transform.up);
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 alongWall = transform.TransformDirection(Vector3.forward);

            Debug.DrawRay(transform.position, alongWall.normalized * 10, Color.green);
            Debug.DrawRay(transform.position, lastWallNormal * 10, Color.magenta);

            playerBody.velocity = alongWall * vertical * 10f;
            isWallRunning = true;
        }
    }
}
