using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StupidFuckingAnimatorBrain : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StupidFuckingCaller()
    {
        GetComponentInParent<Enemy1>().shootNowDaddy = true;
    }
    public void StupidFuckingEnder()
    {
        GetComponentInParent<Enemy1>().returnToRun = true;
    }

    public void StupidFuckingFreezer()
    {
        GetComponentInParent<Enemy1>().canJump = true;
    }
}
