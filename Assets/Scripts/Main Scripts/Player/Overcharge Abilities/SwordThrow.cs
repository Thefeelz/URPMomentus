using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordThrow : MonoBehaviour
{
    bool hitEnemy = false;
    RaycastHit[] hit;
    Vector3[] directions;

    private void Start()
    {
        directions = new Vector3[]
        {
            transform.up,
            -transform.up,
            transform.right,
            -transform.right
        };
    }
    private void Update()
    {
        
        if (!hitEnemy)
        {
            HitEnemy();
        }
    }
    void HitEnemy()
    {
        hit = new RaycastHit[directions.Length];

        for (int i = 0; i < directions.Length; i++)
        {
            if (Physics.Raycast(transform.position, directions[i], out hit[i], 0.25f) && hit[i].transform.GetComponentInParent<EnemyStats>() && !hitEnemy)
            {
                EnemyStats enemy = hit[i].transform.GetComponentInParent<EnemyStats>();
                enemy.TakeDamage(20);
                FindObjectOfType<A_SwordThrow>().SwordHitEnemy(enemy);
                hitEnemy = true;
            }
        }
    }
    public void HitEnemyToFalse() { hitEnemy = false; }
}
