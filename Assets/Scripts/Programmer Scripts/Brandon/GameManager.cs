using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] int bladeColor;
    [SerializeField] int levelChosen;
    [SerializeField] List<EnemyStats> enemiesInLevel = new List<EnemyStats>();
    [SerializeField] Material[] aquaMaterial, redMaterial, blueMaterial, greenMaterial;
    public bool activeInUse = false;

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
    public void SetBladeColor(int newColor)
    {
        bladeColor = newColor;
    }
    public void SetLevel(int level)
    {
        levelChosen = level;
    }
    public int GetLevel()
    {
        return levelChosen;
    }
    public Material[] GetMaterials()
    {
        if (bladeColor == 0)
        {
            return aquaMaterial;
        }
        else if (bladeColor == 1)
            return redMaterial;
        else if (bladeColor == 2)
            return blueMaterial;
        else if (bladeColor == 3)
            return greenMaterial;
        else
            return aquaMaterial;
    }
}
