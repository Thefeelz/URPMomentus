using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    [Header ("Circuit Breaker")]
    [SerializeField] float dashTime;
    [SerializeField] EnemyBad currentTarget;
    [SerializeField] bool allEnemiesAttacked = false;
    [SerializeField] int killCount;
    [SerializeField] int killCountMax = 7;
    bool circuitBreakerReady = true;

    [Header("Planet Shaker")]
    [SerializeField] int maxStrikingDistance;
    [SerializeField] float pullingSpeed;
    List<EnemyBad> enemiesInRangeOfAbility = new List<EnemyBad>();
    bool pullingEnemies = false;
    [SerializeField] float pullingTime;
    [SerializeField] float stunTime;
    bool planetShakerReady = true;

    [Header("Contained Heat")]
    [SerializeField] GameObject ballToExpandPrefab;
    [SerializeField] float containedHeatMaxDuration;
    [SerializeField] float sizeOfContainedHeat;
    [SerializeField] float containedHeatExpanionSpeed;
    GameObject instantiatedContainedHeat;
    bool containedHeatExpanding = false;
    bool containedHeatReady = false;

    [Header("Gamma Explosion")]
    [SerializeField] float gammaExplosionDelayTime;
    [SerializeField] float gammaExplosionSize;
    [SerializeField] float gammaExplosionSpeed;
    RaycastHit hitEnemy;

    [Header("General Things")]
    [SerializeField] GameObject myStartingPosition;
    [SerializeField] Camera myCam;
    [SerializeField] Transform firstPersonCamPos;
    [SerializeField] Transform specialAbilityCamPos;
    [SerializeField] float camTransitionTime;
    float camElapsedTime = 0f;
    bool camTransitioning;
    bool usingSpecial;

    Vector3 startPos;
    Quaternion startRotation;
    float elapsedTime = 0f;
    GameManager gameManager;
    Rigidbody myRB;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        myRB = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1) && circuitBreakerReady)
        {
            gameManager.SetActiveSpecialAbility(true);
            startPos = myStartingPosition.transform.position;
            startRotation = myStartingPosition.transform.rotation;
            allEnemiesAttacked = false;
            circuitBreakerReady = false;
            GetComponent<P_Movement>().enabled = false;
            GetComponent<mouseLook>().enabled = false;
            camTransitioning = true;
            usingSpecial = true;
            StartCoroutine(AttackEnemy());
        }
        if(Input.GetKeyDown(KeyCode.BackQuote))
        {
            gameManager.ResetAllEnemies();
        }
        if(Input.GetKeyDown(KeyCode.Alpha2) && planetShakerReady)
        {
            pullingEnemies = true;
            planetShakerReady = false;
            enemiesInRangeOfAbility = FindEnemiesWithinRange(gameManager.GetActiveEnemies());
        }
        if(pullingEnemies && pullingTime > elapsedTime)
        {
            PullEnemiesToYou(enemiesInRangeOfAbility);
        }
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            containedHeatReady = false;
            containedHeatExpanding = true;
            instantiatedContainedHeat = Instantiate(ballToExpandPrefab, transform);
            Destroy(instantiatedContainedHeat, containedHeatMaxDuration);
        }
        if(containedHeatExpanding)
        {
            ExpandContainedHeat();
        }
        if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            if(Physics.Raycast(transform.position, Vector3.forward * 5f, out hitEnemy))
            {
                if(hitEnemy.transform.gameObject.layer == 6)
                {
                    //hitEnemy.transform.GetComponent<EnemyBad>().StartGammaExplosion(gammaExplosionDelayTime, gammaExplosionSize, gammaExplosionSpeed);
                }
            }
        }
        if (camTransitioning)
            UpdateCameraPos(usingSpecial);
    }
    void UpdateCameraPos(bool startingSpecial)
    {
        camElapsedTime += Time.deltaTime;
        if (startingSpecial)
        {
            myCam.transform.position = Vector3.Lerp(firstPersonCamPos.position, specialAbilityCamPos.position, camElapsedTime / camTransitionTime);
        }
        else
        {
            myCam.transform.position = Vector3.Lerp(specialAbilityCamPos.position, firstPersonCamPos.position, camElapsedTime / camTransitionTime);
        }
        if(camTransitionTime <= camElapsedTime)
        {
            camTransitioning = false;
            camElapsedTime = 0;
        }
    }
    EnemyBad LocateClosestEnemy()
    {
        // If kill count is greater than the max allowed break the Enumerator
        if(killCount >= killCountMax)
        {
            return null;
        }
        EnemyBad closestEnemy = null;

        // Loop through all the enemies
        foreach (var enemy in gameManager.GetActiveEnemies())
        {
            // Check if they have been attacked
            if (!enemy.GetBeenAttacked())
            {
                // If closest enemy has not been set yet, set it to the first enemy
                if (closestEnemy == null)
                {
                    closestEnemy = enemy;
                }
                // Else we are checking to see if the new enemy in the list is closer than the current enemy
                else if (Vector3.Distance(transform.position, enemy.transform.position) <= Vector3.Distance(transform.position, closestEnemy.transform.position))
                {
                    closestEnemy = enemy;
                }
            }
        }
        // If closest enemy is null, returm null
        if (closestEnemy == null)
        {
            return null;
        }
        else
        {
            // Return the closest enemy
            return closestEnemy;
        }
    }
    IEnumerator AttackEnemy()
    {
        // If camera is going in or out wait for the transition to end
        if (camTransitioning)
            yield return new WaitForSeconds(camTransitionTime);
        // Do while loop that loops through and attacks the enemies in the list of enemies to attack
        do
        {
            yield return new WaitForSeconds(dashTime);
            // Current target is the closest target to us
            currentTarget = LocateClosestEnemy();

            // If the current target is null, we will exit the enumerator
            if (currentTarget == null)
            {
                // Allow the enemies to move again
                gameManager.SetActiveSpecialAbility(false);

                // Set all enemies attacked to true to allow gameflow to continue
                allEnemiesAttacked = true;

                // Reset the player position
                transform.position = startPos;
                transform.rotation = startRotation;

                // Reset the ability
                circuitBreakerReady = true;
                killCount = 0;

                // Turn back on movement and camera controls
                GetComponent<P_Movement>().enabled = true;
                GetComponent<mouseLook>().enabled = true;

                // Turn off the using special so you can use other abilities
                usingSpecial = false;

                // Transition the camera back to the original location
                camTransitioning = true;
                yield break;
            }
            // We have a target
            else
            {
                // Move behind a target
                transform.position = currentTarget.transform.position;
                transform.rotation = currentTarget.transform.rotation;
                transform.position -= transform.forward;

                // Attack the target
                currentTarget.SetBeenAttacked(true);

                // Incriment the kill count
                killCount++;
            }
        } while (!allEnemiesAttacked);
    }
    List<EnemyBad> FindEnemiesWithinRange(List<EnemyBad> enemyList)
    {
        List<EnemyBad> enemiesInRange = new List<EnemyBad>();
        foreach (var enemy in enemyList)
        {
            if(Vector3.Distance(transform.position, enemy.transform.position) < maxStrikingDistance)
            {
                enemiesInRange.Add(enemy);
            }
        }
        return enemiesInRange;
    }
    void PullEnemiesToYou(List<EnemyBad> enemyList)
    {
        elapsedTime += Time.deltaTime;
        foreach (var enemy in enemyList)
        {
            if (!enemy.GetStunned())
                enemy.SetStunned(true);
            Vector3 direction = transform.position - enemy.transform.position;
            enemy.transform.rotation = Quaternion.LookRotation(direction);
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, transform.position, pullingSpeed * Time.deltaTime);
            if((elapsedTime / pullingTime) < 0.5f)
            {
                enemy.transform.position += new Vector3(0, Time.deltaTime * pullingSpeed, 0);
            }
            else
            {
                Debug.Log("Got here");
                enemy.transform.position -= new Vector3(0, Time.deltaTime * pullingSpeed, 0);
            }
            enemy.transform.rotation = new Quaternion(0, enemy.transform.rotation.y, 0, enemy.transform.rotation.w);
        }
        if(pullingTime < elapsedTime)
        {
            elapsedTime = 0;
            pullingEnemies = false;
            StartCoroutine(UnstunEnemies());
        }
    }
    IEnumerator UnstunEnemies()
    {
        yield return new WaitForSeconds(stunTime);
        foreach (var enemy in enemiesInRangeOfAbility)
        {
            enemy.SetStunned(false);
            enemy.transform.position = new Vector3(enemy.transform.position.x, 0, enemy.transform.position.z);
        }
        enemiesInRangeOfAbility.Clear();
        planetShakerReady = true;
    }
    void ExpandContainedHeat()
    {
        if (instantiatedContainedHeat.transform.localScale.x < sizeOfContainedHeat)
            instantiatedContainedHeat.transform.localScale += instantiatedContainedHeat.transform.localScale * Time.deltaTime * containedHeatExpanionSpeed;
        else
            containedHeatExpanding = false;

    }
}
