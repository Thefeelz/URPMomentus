using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sendForce : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private int bouncePower;

    private Rigidbody playerBody;

    private void Start()
    {
        playerBody = player.GetComponentInChildren<Rigidbody>();
    }

    void OnCollisionEnter()
    {
       playerBody.AddForce(Vector3.up * bouncePower, ForceMode.VelocityChange);
    }
}
