using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedSpawner : MonoBehaviour
{
    public GameObject rangedEn;
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject newSpawn = Instantiate(rangedEn, transform);
        newSpawn.GetComponent<Enemy1>().spawner = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
