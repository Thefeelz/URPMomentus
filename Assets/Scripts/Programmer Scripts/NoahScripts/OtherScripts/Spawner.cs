using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] spawners1;
    public GameObject[] spawners2;
    public GameObject[] spawners3;
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
        enemy.transform.position = spawners1[spawnPoint].transform.position;
        enemy.SetActive(true);
    }
}
