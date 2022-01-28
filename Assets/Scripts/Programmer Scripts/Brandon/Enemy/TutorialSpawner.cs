using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSpawner : MonoBehaviour
{
    [SerializeField] EnemyStats[] spawnArray;
    [SerializeField] float initialdelay;
    [SerializeField] float spawnDelay;
    [SerializeField] int spawnAmount, bulletAmount;
    [SerializeField] int amountRemaining;
    [SerializeField] bool chaseOnAwake;
    [SerializeField] Transform spawnLocation;
    [SerializeField] Collider spawnArea;
    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemy());
        amountRemaining = spawnAmount;
        gameManager = FindObjectOfType<GameManager>();
        Debug.Log("Chase on Awake is " + chaseOnAwake);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(initialdelay);
        int i = 0;
        int currentlySpawned = 0;
        do
        {
            yield return new WaitForSeconds(spawnDelay);
            if (!gameManager.activeInUse)
            {
                EnemyStats newEnemy = Instantiate(spawnArray[i % spawnArray.Length], new Vector3(spawnLocation.position.x + Random.Range(-5, 5), spawnLocation.position.y, spawnLocation.position.z + Random.Range(-5, 5)), Quaternion.identity, this.transform);
                newEnemy.GetComponent<EnemyChaseState>().SetAmmoCount(bulletAmount);
                currentlySpawned++;
                amountRemaining--;
                if (chaseOnAwake)
                {
                    Debug.Log("Chase State Called");
                    StartCoroutine(DelayChaseState(newEnemy));
                }
                i++;
            }
        } while (spawnAmount > currentlySpawned);
    }
    IEnumerator DelayChaseState(EnemyStats newEnemy)
    {
        yield return new WaitForSeconds(1f);
        newEnemy.GetComponent<EnemyChaseState>().SetStateToChase();
    }
}
