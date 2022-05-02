using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }
    [SerializeField] int bladeColor;
    [SerializeField] int levelChosen, currentLevelBuildIndex, previousLevelBuildIndex;
    [SerializeField] List<EnemyStats> enemiesInLevel = new List<EnemyStats>();
    [SerializeField] Material[] aquaMaterial, redMaterial, blueMaterial, greenMaterial;
    [SerializeField] ParticleSystem[] aquaParticleSystem, redParticleSystem, blueParticleSystem, greenParticleSystem;
    [SerializeField] GameObject swordSlashLightning, swordSlashLightningRed, swordSlashLightningBlue, swordSlashLightningGreen;
    [SerializeField] GameObject containedHeat, containedHeatRed, containedHeatBlue, containedHeatGreen;
    public bool activeInUse = false;

    [SerializeField] float mouseSensitivity = 50f;
    [SerializeField] float fieldOfView = 60f;
    [SerializeField] bool volume = true;
    [SerializeField] Slider mainMenuSlider;
    Transform positionToRespawnCheckpoint, positionToRespawnDefault;
    bool[] levelsCompleted = { false, false, false };
    public bool respawnAtCheckpoint = false;
    public int respawnCheckpointIndex = 0;



    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        if (_instance != null && _instance != this)
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
                Destroy(gm.gameObject);
            }
        }
        if (!FindObjectOfType<CharacterStats>())
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        if (mainMenuSlider)
            mainMenuSlider.onValueChanged.AddListener(delegate { SetMouseSenitivity(); });

        currentLevelBuildIndex = SceneManager.GetActiveScene().buildIndex;
    }
    private void OnLevelWasLoaded(int level)
    {
        if(SceneManager.GetActiveScene().buildIndex != 9)
            currentLevelBuildIndex = SceneManager.GetActiveScene().buildIndex;
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
            if (GetInLOS(playerPos, enemy.transform))
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
            if (GetInLOS(enemy.transform, playerPos))
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
            if (Vector3.Distance(enemy.transform.position, playerPos.position) <= range)
            {
                enemiesInRange.Add(enemy);
            }
        }
        return enemiesInRange;
    }
    public List<EnemyStats> GetActiveEnemiesInRangeNotDrone(float range, Transform playerPos)
    {
        List<EnemyStats> enemiesInRange = new List<EnemyStats>();
        foreach (EnemyStats enemy in enemiesInLevel)
        {
            if (enemy && Vector3.Distance(enemy.transform.position, playerPos.position) <= range && !enemy.GetComponent<BomberEnemy>())
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
            else if (enemy.GetComponent<BomberEnemy>())
            {
                enemy.GetComponent<BomberEnemy>().SpecialInUse(value);
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
    public int GetCurrentLevelBuildIndex()
    {
        return currentLevelBuildIndex;
    }
    public void SetCurrentLevel(int level)
    {
        currentLevelBuildIndex = level;
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
    public ParticleSystem[] GetParticleSystems()
    {
        if (bladeColor == 0)
        {
            return aquaParticleSystem;
        }
        else if (bladeColor == 1)
            return redParticleSystem;
        else if (bladeColor == 2)
            return blueParticleSystem;
        else if (bladeColor == 3)
            return greenParticleSystem;
        else
            return aquaParticleSystem;
    }
    public GameObject GetSwordSlashPrefab()
    {
        if (bladeColor == 0)
        {
            return swordSlashLightning;
        }
        else if (bladeColor == 1)
            return swordSlashLightningRed;
        else if (bladeColor == 2)
            return swordSlashLightningBlue;
        else if (bladeColor == 3)
            return swordSlashLightningGreen;
        else
            return swordSlashLightning;
    }

    public GameObject GetContainedHeatPrefab()
    {
        if (bladeColor == 0)
        {
            return containedHeat;
        }
        else if (bladeColor == 1)
            return containedHeatRed;
        else if (bladeColor == 2)
            return containedHeatBlue;
        else if (bladeColor == 3)
            return containedHeatGreen;
        else
            return containedHeat;
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

    public void SetFieldOfView(float value)
    {
        fieldOfView = value;
        Camera.main.fieldOfView = value;
        FindObjectOfType<A_AirDash>().UpdateFieldOfViewValue(value);
    }

    public float GetFieldOfView()
    {
        return fieldOfView;
    }

    public void SetVolume(bool value)
    {
        volume = value;
        if (value)
        {
            Camera.main.GetComponent<AudioListener>().enabled = true;
        }
        else
        {
            Camera.main.GetComponent<AudioListener>().enabled = false;
        }
        
    }

    public bool GetVolume()
    {
        return volume;
    }

    bool GetInLOS(Transform player, Transform enemy)
    {
        RaycastHit hit;
        Physics.Raycast(player.position, enemy.transform.position - player.position, out hit);
        if (hit.collider != null && hit.collider.GetComponentInParent<P_Input>())
        {
            return true;
        }
        return false;
    }
    public Vector3 GetRespawnPointPosition() { return positionToRespawnCheckpoint.position; }
    public Vector3 GetRespawnPointRotation() { return positionToRespawnCheckpoint.rotation.eulerAngles; }
    public void SendGameObjectToRespawn(GameObject gameObject)
    {
        RespawnCheckpointManager rManage = FindObjectOfType<RespawnCheckpointManager>();
        Transform t = rManage.GetCurrentIndexTransform(respawnCheckpointIndex);
        gameObject.transform.position = t.position;
        gameObject.transform.rotation = t.rotation;
    }
    public void SetNewRespawnLocation(Transform newRespawnPosition) { positionToRespawnCheckpoint = newRespawnPosition; }
    public Transform GetRestartLevelLocation() { return positionToRespawnDefault; }
    public Transform GetRestartLevelFromCheckpointLocation() { return positionToRespawnCheckpoint; }
    public void SetLevelComplete(int levelCompleted)
    {
        levelsCompleted[levelCompleted - 1] = true;
    }

    public void SetRespawnCheckpointIndex(int newIndex)
    {
        respawnCheckpointIndex = newIndex;
    }
    public int GetRespawnCheckpointIndex() { return respawnCheckpointIndex; }
    public bool GetRespawnAtCheckpoint() { return respawnAtCheckpoint; }
    public void SetRespawnAtCheckpoint(bool value) { Debug.Log("Set Respawn at Checkpoint is " + value); respawnAtCheckpoint = value; Debug.Log("Set Respawn at Checkpoint is " + value); }
    public void ClearEnemyList() { enemiesInLevel.Clear(); }
}
