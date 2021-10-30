using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_WallRun : MonoBehaviour
{
    [SerializeField] float minimumHeightForWallRun = 1.5f;
    Vector3[] directions;
    RaycastHit[] hits;

    Rigidbody playerBody;

    bool VerticalCheck()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minimumHeightForWallRun);
    }

    // Start is called before the first frame update
    void Start()
    {
        playerBody = GetComponent<Rigidbody>();

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
    void Update()
    {
        if(VerticalCheck())
        {
            hits = new RaycastHit[directions.Length];

            for (int i = 0; i < directions.Length; i++)
            {
                Vector3 dir = transform.TransformDirection(directions[i]);
                Physics.Raycast(transform.position, dir, out hits[i], 1);
                if (hits[i].collider != null)
                {
                    Debug.DrawRay(transform.position, dir * hits[i].distance, Color.green);
                }
                else
                {
                    Debug.DrawRay(transform.position, dir * 1, Color.red);
                }
            }
        }
    }
}
