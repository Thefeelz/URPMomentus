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
                    SetSpecialInUse(true);
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
            currentEnemy.TakeDamage(playerStats.GetPlayerAttack());
            SetSpecialInUse(false);
            currentEnemy = null;
        }
    }

    void CheckEnemyInRange()
    {
        
        if (Physics.Raycast(Camera.main.transform.position, transform.forward * 10, out hitTarget))
        {
            if (hitTarget.transform.GetComponentInParent<EnemyStats>() && Vector3.Distance(transform.position, hitTarget.transform.position) < dashMaxDistance && Vector3.Distance(transform.position, hitTarget.transform.position) > dashMinDistance)
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
        yield return new WaitForSeconds(.8f);
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
        Vector3 pos = Camera.main.transform.position;
        Vector3 pos1 = Camera.main.transform.position + (Camera.main.transform.right * 0.25f);
        Vector3 pos2 = Camera.main.transform.position + (Camera.main.transform.right * -0.25f);
        RaycastHit hit, hit1, hit2;
        Physics.Raycast(pos, transform.forward, out hit, 2f);
        Physics.Raycast(pos1, transform.forward, out hit1, 2f);
        Physics.Raycast(pos2, transform.forward, out hit2, 2f);

        Debug.DrawRay(pos, transform.forward * 2, Color.red, 2f);
        Debug.DrawRay(pos1, transform.forward * 2, Color.red, 2f);
        Debug.DrawRay(pos2, transform.forward * 2, Color.red, 2f);

        if (hit.collider != null && hit.collider.GetComponentInParent<EnemyStats>() && !dashing)
        {
            hit.transform.GetComponentInParent<EnemyStats>().TakeDamage(playerStats.GetPlayerAttack());
        }
        else if(hit1.collider != null && hit1.collider.GetComponentInParent<EnemyStats>() && !dashing)
        {
            hit1.transform.GetComponentInParent<EnemyStats>().TakeDamage(playerStats.GetPlayerAttack());
        }
        else if (hit2.collider != null && hit2.collider.GetComponentInParent<EnemyStats>() && !dashing)
        {
            hit2.transform.GetComponentInParent<EnemyStats>().TakeDamage(playerStats.GetPlayerAttack());
        }
    }

    void SetSpecialInUse(bool value)
    {
        if (GetComponentInParent<EnemyChaseState>())
            currentEnemy.GetComponentInParent<EnemyChaseState>().SpecialInUse(value);
        else if (GetComponentInParent<Entity>())
        {
            currentEnemy.GetComponent<Entity>().specialUseBool = value;
            currentEnemy.GetComponent<Entity>().stateMachine.ChangeState(currentEnemy.GetComponent<Entity>().specialUseState);
        }
        else if (GetComponentInParent<Turret>())
        {
            currentEnemy.GetComponentInParent<Turret>().SetStateToSpecialInUse(value);
        }
    }
}
