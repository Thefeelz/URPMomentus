using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSpawner : MonoBehaviour
{
    [SerializeField] EnemyStats[] spawnArray;
    [SerializeField] float spawnDelay;
    [SerializeField] int spawnAmount;
    [SerializeField] int amountRemaining;
    [SerializeField] bool chaseOnAwake;
    [SerializeField] Transform spawnLocation;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemy());
        amountRemaining = spawnAmount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator SpawnEnemy()
    {
        int i = 0;
        int currentlySpawned = 0;
        do
        {
            yield return new WaitForSeconds(spawnDelay);
            EnemyStats newEnemy = Instantiate(spawnArray[i % spawnArray.Length],spawnLocation.position, Quaternion.identity, this.transform);
            currentlySpawned++;
            amountRemaining--;
            if (chaseOnAwake)
                newEnemy.GetComponent<EnemyChaseState>().SetStateToChase();
            i++;
            Debug.Log("Spawned Enemy");
        } while (spawnAmount > currentlySpawned);
    }
}
