using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTriggerGroup : MonoBehaviour
{
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
        enemiesInGroup.Remove(childThatDied);
        if (enemiesInGroup.Count == 0)
            TriggerObjects();
    }
    void SetUpTriggerChildren()
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
}
