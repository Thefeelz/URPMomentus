using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFireTrigger : MonoBehaviour
{
    public Enemy1 parent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "obstacle")
        {
            Debug.LogWarning("patricide");
            parent.stateMachine.ChangeState(parent.moveState);
        }
    }
}
