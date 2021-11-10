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

    EnemyStats bossStats;

    /// proto variables
    bool attacked = false;


    // Start is called before the first frame update
    void Start()
    {
        nextLocation = spot[0].transform.position;
        totalLocations = spot.Length;
        localIndex = 1;
        bossStats = gameObject.GetComponent<EnemyStats>();
        
    }

    //proto sim-attack
    private void Update()
    {
        attacked = false;
        if (Input.GetMouseButtonDown(0))
        {
            attacked = true;
        }
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

        if (attacked)
        {
            if (bossStats.getCurrentHealth() > 100)
            {
                Debug.LogWarning("Health is out-of-bounds (greater than 100). Prepare for unforseen consequences");

            }
            else if (bossStats.getCurrentHealth() < 1)
            {
                Debug.LogWarning("Boss health has gone out-of-bounds (less than 1). Adjustments made");
                bossStats.setCurrentHealth(1);
            }
            else
            {
                bossStats.TakeDamage(25);
                Debug.Log("Health is now at " + bossStats.getCurrentHealth());
            }
        }
        //health check & Speed set
        
        if (bossStats.getCurrentHealth() < 75)
        {
            //speed = 10;
            ///fake speed
            speed = 15;
            myDelay = 3;
        }
        else if (bossStats.getCurrentHealth() < 50)
        {
            //speed = 15;
            ///fake speed
            speed = 25;
            myDelay = 2;
        }
        else if (bossStats.getCurrentHealth() < 25)
        {
            //speed = 20;
            ///fake speed
            speed = 50;
            myDelay = 0.25f;
        }
        //should be active for if health is above 75 (AKA: full health)
        else
        {
            speed = 5;
            myDelay = 4;
        }

        /// math version of getting speed based on health
        //speed = 20f / health;
        //Debug.Log("Speed: " + speed);

        float temp = 0;
        temp = temp + speed * Time.deltaTime;
        this.transform.position = Vector3.MoveTowards(transform.position, nextLocation, temp);
        //this.transform.position=Vector3.Lerp(transform.position, nextLocation, temp);
        
    }
    IEnumerator bossDelay()
    {
        
        inDelay = true;
        yield return new WaitForSeconds(myDelay);
        
        localIndex++;
        if (localIndex > totalLocations-1)
        {
            localIndex = 1;
        }
        
        nextLocation = spot[localIndex].transform.position;
        
        inDelay = false;
    }
}
