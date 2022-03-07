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
    public NavMeshAgent agent;
    public NavMeshPath navMeshPath;
    public Entity[] enemies;
    public Vector3 direction;
    private int count = 0;


    void Start()
    {
        enemies = FindObjectsOfType(typeof(Enemy_Melee)) as Enemy_Melee[]; // gets all melee enemies
        navMeshPath = new NavMeshPath();
        agent = GetComponent<NavMeshAgent>();
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
        enemiesCalc();
        InvokeRepeating("calculate", 2.0f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = lPlayer.transform.position;
        spotUpdate();
        if (Input.GetKeyDown(KeyCode.U))
        {
            foreach (Enemy_Melee em in enemies)
            {
                em.moveState.Calculate();
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
                    if (lSpotsValid[l] && lSpotsTaken[l] == null)
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




    // DEAD CODE THAT MAY HAVE USE

    void enemiesCalc()
    {

        foreach (Enemy_Melee em in enemies)
        {
            //em.GetComponent<NavMeshObstacle>().enabled = false;
            //em.GetComponent<NavMeshAgent>().enabled = true;
            ////if ((Vector3.Distance(em.mLocations.lSpots[em.spot], em.targetPos) > 2.5))
            ////    em.moveState.calculate();
            ////else
            ////    em.moveState.Move();
            ////if (em.GetComponent<NavMeshAgent>().enabled == true)
            ////{
            ////    em.line.positionCount = agent.path.corners.Length;
            ////    em.line.SetPositions(agent.path.corners);
            ////}
            //NavMeshPath p = new NavMeshPath();
            //em.agent.CalculatePath(em.mLocations.lSpots[em.spot], p);
            //LineRenderer line = em.GetComponent<LineRenderer>();
            //line.positionCount = p.corners.Length;
            //line.SetPositions(p.corners);
            //em.targetPos = new Vector3(p.corners[0].x, em.transform.position.y, p.corners[0].z);

            //em.GetComponent<NavMeshAgent>().enabled = false;
            //em.GetComponent<NavMeshObstacle>().enabled = true;
        }
    }

    void calculate()
    {
        //    Debug.Log("ayo");
        //    Debug.LogWarning("CALCULATING");
        //    foreach (Enemy_Melee em in enemies)
        //    {
        //        em.GetComponent<NavMeshObstacle>().enabled = false;
        //        em.GetComponent<NavMeshAgent>().enabled = true;
        //        em.moveState.calculate();
        //        em.GetComponent<NavMeshAgent>().enabled = false;
        //        em.GetComponent<NavMeshObstacle>().enabled = true;
        //    }
    }

    void corners(NavMeshPath p)
    {

    }
}
