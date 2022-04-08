using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] spawners; 
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private float dis;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, player.transform.position) < dis)
        {
            for(int i = 0; i < spawners.Length; i++)
            {
                spawners[i].SetActive(true);
            }
        }
    }
}
