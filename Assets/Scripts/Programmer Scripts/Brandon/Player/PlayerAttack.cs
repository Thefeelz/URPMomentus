using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class PlayerAttack : MonoBehaviour
{
    CharacterStats playerStats;
    Rigidbody myRb;
    [SerializeField] Image targetCrosshair;
    [SerializeField] Transform weaponRaycastTransformPosition;
    [SerializeField] Animator playerAnimator;
    [SerializeField] float dashMaxDistance;
    [SerializeField] float dashMinDistance;
    [SerializeField] float attackDashCooldown;
    [SerializeField] float dashTime;

    [SerializeField] Volume attackDashVolume;
    RaycastHit hitTarget;
    bool hitCast;
    bool dashing;
    bool ableToAttackDash = true;
    float elaspedTime = 0f;
    Color crosshairColor;
    Vector3 endingDashPosition;
    Vector3 startingDashPosition;
    EnemyStats currentEnemy;



    // Start is called before the first frame update
    void Start()
    {
        playerStats = GetComponentInParent<CharacterStats>();
        myRb = GetComponent<Rigidbody>();
        crosshairColor = targetCrosshair.color;
    }

    private void Update()
    {
        CheckEnemyInRange();
        if (dashing)
            DashToEnemy();
       
    }

    public void BasicAttack()
    {
        if(!playerAnimator.GetBool("swordSwing"))
        {
            StartCoroutine(WeaponSwing());
            
            if (ableToAttackDash && Physics.Raycast(Camera.main.transform.position, transform.forward * 10, out hitTarget))
            {
                if (hitTarget.transform.GetComponentInParent<EnemyStats>() && Vector3.Distance(transform.position, hitTarget.transform.position) < dashMaxDistance && Vector3.Distance(transform.position, hitTarget.transform.position) > dashMinDistance)
                {
                    endingDashPosition = hitTarget.transform.position;
                    startingDashPosition = transform.position;
                    dashing = true;
                    StartCoroutine(ResetAttackDash());
                    DashToEnemy();
                    currentEnemy = hitTarget.transform.GetComponentInParent<EnemyStats>();
                    currentEnemy.GetComponentInParent<EnemyChaseState>().SpecialInUse(true);
                }
            }
        }
    }
    public void BasicDefense()
    {
        playerAnimator.SetBool("swordBlock", true);
    }
    public void SwordBlockComplete()
    {
        playerAnimator.SetBool("swordBlock", false);
    }
    void DashToEnemy()
    {
        if (elaspedTime <= dashTime)
        {
            elaspedTime += Time.deltaTime;
            myRb.MovePosition(Vector3.Lerp(startingDashPosition, endingDashPosition, elaspedTime / dashTime));
            attackDashVolume.weight = Mathf.Lerp(0, 1f, elaspedTime / dashTime);
        }
        else
        {
            elaspedTime = 0;
            dashing = false;
            attackDashVolume.weight = 0;
            currentEnemy.TakeDamage(playerStats.GetPlayerStrength());
            currentEnemy.GetComponentInParent<EnemyChaseState>().SpecialInUse(false);
            currentEnemy = null;
        }
    }

    void CheckEnemyInRange()
    {
        // Debug.DrawRay(Camera.main.transform.position, transform.forward * 10, Color.red, 2f);
        hitCast = Physics.Raycast(Camera.main.transform.position, transform.forward * 10, out hitTarget); //Physics.BoxCast(weaponRaycastTransformPosition.position, new Vector3(0.5f, 0.5f, 0.5f), weaponRaycastTransformPosition.rotation * Vector3.forward, out hitTarget, Quaternion.identity, 2f);
        if (hitCast)
        {
            if (hitTarget.transform.CompareTag("Enemy") && Vector3.Distance(transform.position, hitTarget.transform.position) < dashMaxDistance && Vector3.Distance(transform.position, hitTarget.transform.position) > dashMinDistance)
            {
                targetCrosshair.color = Color.red;
            }
            else
            {
                targetCrosshair.color = crosshairColor;
            }
        }
        else
        {
            targetCrosshair.color = crosshairColor;
        }
    }
    IEnumerator WeaponSwing()
    {
        playerAnimator.SetBool("swordSwing", true);
        yield return new WaitForSeconds(.5f);
        playerAnimator.SetBool("swordSwing", false);
        StopCoroutine(WeaponSwing());
    }
    IEnumerator ResetAttackDash()
    {
        ableToAttackDash = false;
        yield return new WaitForSeconds(attackDashCooldown);
        ableToAttackDash = true;
    }
    public void SetSwordSwingComplete()
    {
        playerAnimator.SetBool("swordSwing", false);
    }
    public void SetStartSuperSlash()
    {
        playerAnimator.SetBool("superSlash", true);
    }
    public void SetSuperSlashComplete()
    {
        playerAnimator.SetBool("superSlash", false);
    }
    public bool GetSuperSlashStatus()
    {
        return playerAnimator.GetBool("superSlash");
    }

    public void CheckForDamage()
    {
        RaycastHit hit;
        Physics.Raycast(weaponRaycastTransformPosition.position, transform.forward, out hit, 2f);
        if(hit.collider != null && hit.collider.GetComponentInParent<EnemyStats>() && !dashing)
        {
            hit.transform.GetComponentInParent<EnemyStats>().TakeDamage(playerStats.GetPlayerStrength());
        }
    }
}
