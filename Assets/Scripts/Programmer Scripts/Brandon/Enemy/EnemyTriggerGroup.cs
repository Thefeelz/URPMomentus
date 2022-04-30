using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTriggerGroup : MonoBehaviour
{
    public bool noahAi; //checks for who's ai is being used
    public int enemySize; // how many enemies are to spawn
    public int spawned;
    [SerializeField] List<EnemyStats> enemiesInGroup = new List<EnemyStats>();
    [SerializeField] List<GameObject> objectToTrigger = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
        SetUpTriggerChildren();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemySize == spawned && enemySize != 0)
        {
            TriggerObjects();
        }
    }
    public void TriggerObjects()
    {
        foreach (GameObject objectz in objectToTrigger)
        {
            objectz.SetActive(true);
        }
    }
    public void UpdateTriggerGroup(EnemyStats childThatDied)
    {
        if (noahAi == false)
        {
            enemiesInGroup.Remove(childThatDied);
            if (enemiesInGroup.Count == 0)
                TriggerObjects();
        }
        else // when the right number of enemies have been killed trigger objects is called
        {
            enemySize -= 1;
            if(enemySize <= 0)
            {
                TriggerObjects();
            }
        }
    }
    void SetUpTriggerChildren()
    {
        if (noahAi == false)
        {
            foreach (Transform transform in transform)
            {
                if (transform.GetComponent<EnemyStats>())
                {
                    transform.gameObject.AddComponent<EnemyTriggerChild>();
                    enemiesInGroup.Add(transform.GetComponent<EnemyStats>());
                }
            }
        }
        else
        {
            enemySize = GetComponentInChildren<Pooler>().totToSpawn; // totToSpawn is how many enemies must be killed to continue
        }
    }
}
