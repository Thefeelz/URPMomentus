using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sendForce : MonoBehaviour
{
    public GameObject player;
    public Rigidbody rigid;
    public int bounce;
    
    // Start is called before the first frame update
    void Start()
    {
        rigid = player.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    void OnCollisionEnter()
    {
       rigid.AddForce(Vector3.up * bounce, ForceMode.Impulse);
    }

}
