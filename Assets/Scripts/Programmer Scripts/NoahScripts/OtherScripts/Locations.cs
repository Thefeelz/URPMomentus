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
    public bool[] lSpotsTaken = new bool[8];
    public bool[] lSpotsValid = new bool[8];
    public GameObject prefab;
    public GameObject[] objects = new GameObject[8];
    public NavMeshAgent agent;
    public NavMeshPath navMeshPath;

    void Start()
    {
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
                    lSpots[l] = new Vector3(lPlayer.transform.position.x + i, lPlayer.transform.position.y, lPlayer.transform.position.z + j);
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
                        l += 1;
                }
            }
        }



    }
}
