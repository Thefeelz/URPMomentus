using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_BladeDance : A_OverchargeAbilities
{
    [Header("Blade Dance")]
    [SerializeField] float dashTime;
    [SerializeField] int killCountMax = 7;
    [SerializeField] float maxRange = 30f;
    [SerializeField] GameObject bodyForAnimation;
    [SerializeField] GameObject currentSword;

    public EnemyStats currentTarget;
    bool allEnemiesAttacked = false;
    int killCount;
    

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
    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (camTransitioning)
            UpdateCameraPos(usingSpecial);
        if (abilityCooldownCurrent > 0)
            ui.UpdateBladeDanceFill((abilityCooldownMax - abilityCooldownCurrent) / abilityCooldownMax);
    }

    public bool Ability_BladeDance()
    {
        if (!abilityReady) { return false; }
        gameManager.SetActiveSpecialAbility(true);

        SetCurrentPlayerPosition();
        TogglePlayerMovementAndAnimator(false);

        // Make sure that all enemies attacked is set to false
        allEnemiesAttacked = false;

        // Start camera transition for the Camera Effect
        camTransitioning = true;

        // This bool sets the camera transition direction for if we are zooming in or out
        usingSpecial = true;

        // Set the global variable to be true so in case any enemies spawn, they are frozen
        gameManager.activeInUse = true;
        StartCoroutine(AttackEnemy());
        return true;
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
        if (camTransitionTime <= camElapsedTime)
        {
            camTransitioning = false;
            camElapsedTime = 0;
         
            if (!startingSpecial)
            {
                bodyForAnimation.SetActive(false);
                currentSword.SetActive(true);
                TogglePlayerMovementAndAnimator(true);
            }
        }
    }
    EnemyStats LocateClosestEnemy()
    {
        if (killCount >= killCountMax)
        {
            return null;
        }
        EnemyStats closestEnemy = null;
        foreach (var enemy in gameManager.GetActiveEnemiesInLineOfSightAndRange(maxRange, transform))
        {
            if (enemy.getCurrentHealth() > 0)
            {
                if (closestEnemy == null)
                {
                    closestEnemy = enemy;
                }
                else if (Vector3.Distance(transform.position, enemy.transform.position) <= Vector3.Distance(transform.position, closestEnemy.transform.position))
                {
                    closestEnemy = enemy;
                }
            }
        }
        if (closestEnemy == null)
        {
            return null;
        }
        else
        {
            return closestEnemy;
        }
    }
    IEnumerator AttackEnemy()
    {
        bodyForAnimation.SetActive(true);
        currentSword.SetActive(false);
        if (camTransitioning)
            yield return new WaitForSeconds(camTransitionTime);
        do
        {
            yield return new WaitForSeconds(dashTime);
            if(!bodyForAnimation.GetComponent<Animator>().GetBool("swordDance"))
                bodyForAnimation.GetComponent<Animator>().SetBool("swordDance", true);
            currentTarget = LocateClosestEnemy();
            if (currentTarget == null)
            {
                bodyForAnimation.GetComponent<Animator>().SetBool("swordDance", false);
                gameManager.SetActiveSpecialAbility(false);
                allEnemiesAttacked = true;
                gameManager.activeInUse = false;
                transform.position = startPos;
                transform.rotation = startRotation;
                killCount = 0;
                usingSpecial = false;
                camTransitioning = true;
                yield break;
            }
            else
            {
                transform.position = currentTarget.transform.position;
                transform.rotation = currentTarget.transform.rotation;
                transform.position -= transform.forward;

                // currentTarget.GetComponent<MeshRenderer>().material.color = Color.black;
                currentTarget.TakeDamage(player.GetPlayerAttack());
                killCount++;
            }
        } while (!allEnemiesAttacked);
    }
    void SetCurrentPlayerPosition()
    {
        startPos = myStartingPosition.transform.position;
        startRotation = myStartingPosition.transform.rotation;
    }
}
