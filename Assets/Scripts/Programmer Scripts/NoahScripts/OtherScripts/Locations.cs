using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Locations : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject lPlayer;
    public Vector3 lPlayerPos;
    public Vector3[] lSpots = new Vector3[4];
    public Enemy_Melee[] lSpotsTaken = new Enemy_Melee[4];
    public bool[] lSpotsValid = new bool[4];
    public GameObject prefab;
    public GameObject[] objects = new GameObject[4];
    public Enemy_Melee[] enemies;
    public Vector3 direction;
    private int count = 0;
    public List<Enemy_Melee> currentEnemies = new List<Enemy_Melee>();
    public bool spawnStart = false;


    void Start()
    {
        StartCoroutine(setStuff());

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = lPlayer.transform.position;
        spotUpdate();
        for (int i = 0; i < enemies.Length; i++)
        {
            for (int j = 0; j < enemies.Length; j++)
            {
                if (i != j)
                {
                    if (enemies[i].spot == enemies[j].spot)
                    {
                        currentEnemies.Clear();
                        restartSpots();
                    }
                }
            }
        }
    }
    void spotUpdate()
    {
        int l = 0;
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (j != i && j != i && j != -i)
                {
                    lSpots[l] = new Vector3(lPlayer.transform.position.x + i * 2, lPlayer.transform.position.y, lPlayer.transform.position.z + j * 2); // moves the spot locatiosn aroudn player
                    objects[l].transform.position = lSpots[l];
                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(lSpots[l], out hit, 0.1f, NavMesh.AllAreas)) // checks if each spot is on navmesh
                    {
                        lSpotsValid[l] = true;
                    }
                    else
                    {
                        lSpotsValid[l] = false;
                    }
                    if (lSpotsTaken[l] == null) // MAKE SURE THAT SPOTS VALID IS CHECKED IN FUTURE!!!
                    {
                        Enemy_Melee closest = null;
                        float minDist = Mathf.Infinity;
                        foreach (Enemy_Melee em in enemies)
                        {
                            if (em.hasTarget == false && lSpotsTaken[l] == null) // only run if the enemy has no assigned spot
                            {
                                float dist = Vector3.Distance(lSpots[l], em.transform.position); //for finds which enemy is closest provided the enemy is not assigned a spot
                                if (dist < minDist)
                                {
                                    minDist = dist;
                                    closest = em;
                                }
                                if (closest)
                                {
                                    closest.hasTarget = true;
                                    closest.spot = l;
                                    lSpotsTaken[l] = closest;
                                }
                            }

                        }
                    }
                    l += 1;
                }
            }
        }

    }





    void corners(NavMeshPath p)
    {

    }

    IEnumerator setStuff()
    {
        Debug.LogWarning("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaAWAHAHAHAHAHHAHG!!!!! NOTICE ME!");
        int l = 0;

        for (int i = -1; i < 2; i++)
        {

            for (int j = -1; j < 2; j++)
            {
                if (j != i && j != i && j != -i)
                {

                    lSpots[l] = new Vector3(lPlayer.transform.position.x + i, lPlayer.transform.position.y, lPlayer.transform.position.z + j);
                    GameObject testy = Instantiate(prefab, this.transform);
                    testy.transform.position = lSpots[l];
                    objects[l] = testy;
                    l += 1;
                }
            }
        }
        yield return new WaitForSeconds(2);
        enemies = FindObjectsOfType(typeof(Enemy_Melee)) as Enemy_Melee[]; // gets all melee enemies
        for (int i = 0; i < enemies.Length; i++) // The list is just made for efficient sorting
        {
            currentEnemies.Add(enemies[i]);
        }
        currentEnemies.Sort((e1, e2) => Vector3.Distance(e1.transform.position, lPlayer.transform.position).CompareTo(Vector3.Distance(e2.transform.position, lPlayer.transform.position))); // sorting algorithm
        Enemy_Melee[] closeEnemies = new Enemy_Melee[4]; // the 4 closest
        currentEnemies.CopyTo(0, closeEnemies, 0, 4); // copies the the closet for the array
        foreach (Enemy_Melee en in enemies)
        {
            if (en == closeEnemies[0])
            {
                en.hasTarget = true;
                en.spot = 0;
                lSpotsTaken[0] = en;
            }
            else if (en == closeEnemies[1])
            {
                en.hasTarget = true;
                en.spot = 1;
                lSpotsTaken[1] = en;
            }
            else if (en == closeEnemies[2])
            {
                en.hasTarget = true;
                en.spot = 2;
                lSpotsTaken[2] = en;
            }
            else if (en == closeEnemies[3])
            {
                en.hasTarget = true;
                en.spot = 3;
                lSpotsTaken[3] = en;
            }
            else
            {
                en.hasTarget = false;
                en.spot = 9;
            }
        }
        
        yield return new WaitForSeconds(2);
        spawnStart = true;
    }

    public void DeathRelocate(int spot)
    {

        enemies = FindObjectsOfType(typeof(Enemy_Melee)) as Enemy_Melee[]; // gets all melee enemies
        if (spot != 9)
        {
            currentEnemies.Clear();

            for (int i = 0; i < enemies.Length; i++) // The list is just made for efficient sorting
            {
                if (enemies[i].spot == 9)
                {
                    currentEnemies.Add(enemies[i]);
                }
            }
            currentEnemies.Sort((e1, e2) => Vector3.Distance(e1.transform.position, lPlayer.transform.position).CompareTo(Vector3.Distance(e2.transform.position, lPlayer.transform.position)));
            if (currentEnemies[0])
            {
                Enemy_Melee tempE = currentEnemies[0];
                tempE.hasTarget = true;
                tempE.spot = spot;
            }

        }
        
    }

    public void restartSpots()
    {
        enemies = FindObjectsOfType(typeof(Enemy_Melee)) as Enemy_Melee[]; //gets all enemies in scene that are melee type
        currentEnemies.Clear(); // clears current enemy list
        for (int i = 0; i < enemies.Length; i++) // The list is just made for efficient sorting
        {
            if (enemies[i].hasTarget == false) // only add if enemy does not have a target
            {
                currentEnemies.Add(enemies[i]);
            }
        }
        int freeSpot = 0; // free spot code should be able to be deleted
        for(int i = 0; i < 4; i++) // this will get us a number of how many spots are currently free
        {
            if(!lSpotsTaken[i])
            {
                freeSpot += 1;
            }
        }
        currentEnemies.Sort((e1, e2) => Vector3.Distance(e1.transform.position, lPlayer.transform.position).CompareTo(Vector3.Distance(e2.transform.position, lPlayer.transform.position))); // sorting algorithm
        //Enemy_Melee[] closeEnemies = new Enemy_Melee[freeSpot]; // the x closest enemies, x being the amount of free spots
        //currentEnemies.CopyTo(0, closeEnemies, 0, freeSpot); // copies the the closest to the array
        foreach (Enemy_Melee en in currentEnemies) // assigns enemies by how close they are to available spots
        {
            if (lSpotsTaken[0] == null)
            {
                en.hasTarget = true;
                en.spot = 0;
                lSpotsTaken[0] = en;
            }
            else if (lSpotsTaken[1] == null)
            {
                en.hasTarget = true;
                en.spot = 1;
                lSpotsTaken[1] = en;
            }
            else if (lSpotsTaken[2] == null)
            {
                en.hasTarget = true;
                en.spot = 2;
                lSpotsTaken[2] = en;
            }
            else if (lSpotsTaken[3] == null)
            {
                en.hasTarget = true;
                en.spot = 3;
                lSpotsTaken[3] = en;
            }
            else
            {
                en.hasTarget = false;
                en.spot = 9;
            }
        }
    }
}
