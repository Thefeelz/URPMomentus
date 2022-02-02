using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] int bladeColor;
    [SerializeField] int levelChosen;
    [SerializeField] List<EnemyStats> enemiesInLevel = new List<EnemyStats>();
    [SerializeField] Material[] aquaMaterial, redMaterial, blueMaterial, greenMaterial;
    public bool activeInUse = false;

    [SerializeField] float mouseSensitivity = 50f;
    [SerializeField] Slider mainMenuSlider;


    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        
    }
    private void Start()
    {
        if (FindObjectOfType<GameManager>())
        {
            GameManager gm = FindObjectOfType<GameManager>();
            if (gm != this)
            {
                Debug.Log("destroyed");
                Destroy(gm.gameObject);
            }
        }
        if (mainMenuSlider)
            mainMenuSlider.onValueChanged.AddListener(delegate { SetMouseSenitivity(); });
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
            enemy.GetComponent<EnemyChaseState>().SpecialInUse(value);
            //enemy.GetComponent<Entity>().stateMachine.ChangeState(enemy.GetComponent<Entity>().specialUseState);
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

    public float GetMouseSensitivity()
    {
        return mouseSensitivity;
    }

    public void SetMouseSenitivity()
    {
        mouseSensitivity = mainMenuSlider.value;
    }

    public void SetMouseSenitivity(float value)
    {
        mouseSensitivity = value;
        FindObjectOfType<mouseLook>().UpdateMouseSensitivity(value);
    }
}
