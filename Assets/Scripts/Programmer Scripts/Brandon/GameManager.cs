using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] string bladeColor;
    [SerializeField] int levelChosen;
    [SerializeField] List<EnemyStats> enemiesInLevel = new List<EnemyStats>();
    public bool activeInUse = false;
    public bool levelComplete = false;

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
    public void AddEnemyToList(EnemyStats newEnemy)
    {
        enemiesInLevel.Add(newEnemy);
    }
    public List<EnemyStats> GetActiveEnemies()
    {
        return enemiesInLevel;
    }
    public List<EnemyStats> GetActiveEnemiesInRange(float range, Transform playerPos)
    {
        List<EnemyStats> enemiesInRange = new List<EnemyStats>();
        foreach (EnemyStats enemy in enemiesInLevel)
        {
            if(Vector3.Distance(enemy.transform.position, playerPos.position) <= range)
            {
                enemiesInRange.Add(enemy);
            }
        }
        return enemiesInRange;
    }
    public void RemoveFromActiveList(EnemyStats enemyToRemove)
    {
        enemiesInLevel.Remove(enemyToRemove);
    }
    public void SetActiveSpecialAbility(bool value)
    {
        foreach (var enemy in enemiesInLevel)
        {
            Debug.Log("Called " + value);
            enemy.GetComponent<EnemyChaseState>().SpecialInUse(value);
        }
    }
    public void SetBladeColor(string newColor)
    {
        bladeColor = newColor;
    }
    public void SetLevel(int level)
    {
        levelChosen = level;
    }
}
