using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStats : MonoBehaviour
{
    [SerializeField] int maxHealth = 10;
    [SerializeField] int currentHealth;
    [SerializeField] int enemyArmor = 5;
    [SerializeField] Canvas enemyCanvas;
    [SerializeField] Image healthBar;

    Animator animator;
    EnemyChaseState chase;
    P_Input player;
    // Start is called before the first frame update
    void Start()
    {
        // NOTE: This is set to get component in children at the time of its creation, it may change, if there are errors in the future
        // it could be due to the fact that we are looking for the animator in the children if it gets moved elsewhere.
        animator = GetComponentInChildren<Animator>();
        chase = GetComponent<EnemyChaseState>();
        currentHealth = maxHealth;
        player = FindObjectOfType<P_Input>();
    }

    // Update is called once per frame
    void Update()
    {
        enemyCanvas.transform.LookAt(player.transform);   
    }

    public void TakeDamage(int damageToTake)
    {
        Debug.Log("Enemy is taking Damage " + damageToTake);
        currentHealth -= damageToTake;
        healthBar.fillAmount = (float)currentHealth / maxHealth;
        if(currentHealth <= 0)
        {
            animator.SetBool("dead", true);
            chase.dead = true;
        }
    }
}
