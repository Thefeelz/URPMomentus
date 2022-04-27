using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] spawners1;
    public GameObject[] spawners2;
    public GameObject[] spawners3;
    public Locations locations;//change name later
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spawn(GameObject enemy, int type)
    {
        int spawnPoint = Random.Range(0, spawners1.Length);


      
        int layermask = 9;
        bool yes = false;
        for(int i = 0; i < spawners1.Length; i++)
        {
        
            enemy.transform.position = spawners1[i].transform.position;
            if (Physics.CheckSphere(enemy.transform.position, 0.5f, layermask))
            {
                
            }
            else
            {
                break;
            }
        }




        enemy.SetActive(true);
        enemy.GetComponent<Entity>().materializing = true;
        enemy.GetComponent<EnemyStats>().NoahAIAddToActiveList();
        if (locations.spawnStart == true)
        {
            locations.restartSpots();
        }
    }
}
