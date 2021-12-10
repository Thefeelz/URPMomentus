using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] string bladeColor;
    [SerializeField] int levelChosen;
    [SerializeField] List<EnemyStats> enemiesInLevel = new List<EnemyStats>();
    public bool activeInUse = false;
    MasterLevel masterLevel;

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        masterLevel = FindObjectOfType<MasterLevel>();
    }
    public void AddEnemyToList(EnemyStats newEnemy)
    {
        enemiesInLevel.Add(newEnemy);
    }
    public List<EnemyStats> GetActiveEnemies()
    {
        return enemiesInLevel;
    }
    public void RemoveFromActiveList(EnemyStats enemyToRemove)
    {
        //masterLevel.AddToKillCount(1);
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
