using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VinSpawnScript : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyToBeSpawned;

    [SerializeField]
    private int amountInScene=1;

    private string enemyClone;

    private void Start()
    {
        enemyClone = enemyToBeSpawned.name + "(Clone)";
    }
    // Update is called once per frame
    void Update()
    {
        var found=GameObject.Find(enemyClone);
        if (found == null)
        {
            GameObject.Instantiate(enemyToBeSpawned, this.transform.position, this.transform.rotation);
        }
    }
}
