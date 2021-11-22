using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField]
    private Transform[] spot;
    
    private Vector3 nextLocation;
    
    private int totalLocations, localIndex;
    private float speed, myDelay;
    private bool inDelay = false;

    StatBoss bossStats;

    /// proto variables
    bool attacked = false;


    // Start is called before the first frame update
    void Start()
    {
        nextLocation = spot[0].transform.position;
        totalLocations = spot.Length;
        localIndex = 1;
        bossStats = gameObject.GetComponent<StatBoss>();
        
    }

    //proto sim-attack
    private void Update()
    {
        
       
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (Vector3.Distance(this.transform.position, nextLocation)<1)
        {
            
            if (!inDelay)
            {
                StartCoroutine(bossDelay());
            }
            
        }

        //Bounds
        if (Input.GetMouseButtonDown(0) && attacked == false)
        {
          
            bossStats.TakeDamage(25);
            Debug.Log("Health is now at " + bossStats.getCurrentHealth());
            
        }
        if (bossStats.getCurrentHealth() > 100)
        {
            Debug.LogWarning("Health is out-of-bounds (greater than 100). Prepare for unforseen consequences");

        }
        else if (bossStats.getCurrentHealth() < 1)
        {
            Debug.LogWarning("Boss health has gone out-of-bounds (less than 1). Adjustments made");
            bossStats.setCurrentHealth(1);
        }
        
        //health check & Speed set
        
        if (bossStats.getCurrentHealth() == 75)
        {
            //speed = 10;
            ///fake speed
            speed = 3;
            myDelay = 6;
        }
        else if (bossStats.getCurrentHealth() == 50)
        {
            //speed = 15;
            ///fake speed
            speed = 5;
            myDelay = 4;
        }
        else if (bossStats.getCurrentHealth() <= 25)
        {
            //speed = 20;
            ///fake speed
            speed = 10;
            myDelay = 2f;
        }
        //should be active for if health is above 75 (AKA: full health)
        else
        {
            speed = 1;
            myDelay = 8;
        }

        /// math version of getting speed based on health
        //speed = 20f / health;
        Debug.Log("Health: " + bossStats.getCurrentHealth());
        Debug.Log("Speed: " + speed);

        float temp = 0;
        temp = temp + speed * Time.deltaTime;
        this.transform.position = Vector3.MoveTowards(transform.position, nextLocation, temp);
    }

    //Boss pauses before setting new destination
    IEnumerator bossDelay()
    {
        //Stops the delay from being called again
        inDelay = true;
        yield return new WaitForSeconds(myDelay);
        
        //Increment position or go back to first position
        localIndex++;
        if (localIndex > totalLocations-1)
        {
            localIndex = 1;
        }
        
        //Set target for where to move based on transform in array
        nextLocation = spot[localIndex].transform.position;
        
        //let function be called again
        inDelay = false;
    }
}
