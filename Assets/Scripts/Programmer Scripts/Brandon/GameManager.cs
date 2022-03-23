using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }
    [SerializeField] int bladeColor;
    [SerializeField] int levelChosen;
    [SerializeField] List<EnemyStats> enemiesInLevel = new List<EnemyStats>();
    [SerializeField] Material[] aquaMaterial, redMaterial, blueMaterial, greenMaterial;
    public bool activeInUse = false;

    [SerializeField] float mouseSensitivity = 50f;
    [SerializeField] Slider mainMenuSlider;
    Transform positionToRespawn;
    bool[] levelsCompleted = { false, false, false};



    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        if(_instance !=null && _instance !=this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        
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

    public List<EnemyStats> GetActiveEnemiesInLineOfSight(Transform playerPos)
    {
        List<EnemyStats> enemiesInLoS = new List<EnemyStats>();
        foreach (EnemyStats enemy in enemiesInLevel)
        {
            if(GetInLOS(playerPos, enemy.transform))
            {
                enemiesInLoS.Add(enemy);
            }
        }
        return enemiesInLoS;
    }
    public List<EnemyStats> GetActiveEnemiesInLineOfSightAndRange(float range, Transform playerPos)
    {
        List<EnemyStats> enemiesInRange = new List<EnemyStats>();
        List<EnemyStats> enemiesInRangeAndLoS = new List<EnemyStats>();
        enemiesInRange = GetActiveEnemiesInRange(range, playerPos);
        foreach (EnemyStats enemy in enemiesInRange)
        {
            if(GetInLOS(enemy.transform, playerPos))
            {
                enemiesInRangeAndLoS.Add(enemy);
            }
        }
        return enemiesInRangeAndLoS;
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
            if (enemy.GetComponent<EnemyChaseState>())
                enemy.GetComponent<EnemyChaseState>().SpecialInUse(value);
            else if (enemy.GetComponent<Entity>())
            {
                enemy.GetComponent<Entity>().specialUseBool = value;
                enemy.GetComponent<Entity>().stateMachine.ChangeState(enemy.GetComponent<Entity>().specialUseState);
            }
            else if (enemy.GetComponent<Turret>())
            {
                enemy.GetComponent<Turret>().SetStateToSpecialInUse(value);
            }
        }
    }

    public void PlayerDead()
    {
        foreach (EnemyStats enemy in enemiesInLevel)
        {
            enemy.SetStateToPlayerDead();
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

    bool GetInLOS(Transform player, Transform enemy)
    {
        RaycastHit hit;
        Physics.Raycast(player.position + new Vector3(0, 1, 0), enemy.transform.position - player.position, out hit);
        if (hit.collider != null && hit.collider.GetComponentInParent<P_Input>())
        {
            return true;
        }
        return false;
    }
    public Vector3 GetRespawnPointPosition() { return positionToRespawn.position; }
    public Vector3 GetRespawnPointRotation() { return positionToRespawn.rotation.eulerAngles; }
    public void SetNewRespawnLocation(Transform newRespawnPosition) { positionToRespawn = newRespawnPosition; }
    public void SetLevelComplete(int levelCompleted)
    {
        levelsCompleted[levelCompleted - 1] = true;  
    }
}
