using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Locations : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject lPlayer;
    public Vector3 lPlayerPos;
    public Vector3[] lSpots = new Vector3[8];
    public Enemy_Melee[] lSpotsTaken = new Enemy_Melee[8];
    public bool[] lSpotsValid = new bool[8];
    public GameObject prefab;
    public GameObject[] objects = new GameObject[8];
    public NavMeshAgent agent;
    public NavMeshPath navMeshPath;
    public Entity[] enemies;

    void Start()
    {
        enemies = FindObjectsOfType(typeof(Enemy_Melee)) as Enemy_Melee[]; // gets all melee enemies
        navMeshPath = new NavMeshPath();
        agent = GetComponent<NavMeshAgent>();
        int l = 0;
        for(int i = -1; i<2; i++)
        {
            for(int j = -1; j<2; j++)
            {
                if(!(j ==0 && i == 0))
                {
                    
                    lSpots[l] = new Vector3(lPlayer.transform.position.x + i, lPlayer.transform.position.y, lPlayer.transform.position.z + j);
                    GameObject testy = Instantiate(prefab, this.transform);
                    testy.transform.position = lSpots[l];
                    objects[l] = testy;
                    l += 1;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = lPlayer.transform.position;
        int l = 0;
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (!(j == 0 && i == 0))
                {
                    lSpots[l] = new Vector3(lPlayer.transform.position.x + i * 2, lPlayer.transform.position.y, lPlayer.transform.position.z + j * 2);
                    objects[l].transform.position = lSpots[l];
                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(lSpots[l], out hit, 0.1f, NavMesh.AllAreas))
                    {
                        lSpotsValid[l] = true;
                    }
                    else
                    {
                        lSpotsValid[l] = false;
                    }
                    if(lSpotsValid[l] && lSpotsTaken[l] == null)
                    {
                        Enemy_Melee closest = null;
                        float minDist = Mathf.Infinity;
                        foreach(Enemy_Melee em in enemies)
                        {
                            if (em.hasTarget == false && lSpotsTaken[l] == null) // only run if the enemy has no assigned spot
                            {
                                float dist = Vector3.Distance(lSpots[l], em.transform.position);
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
}
