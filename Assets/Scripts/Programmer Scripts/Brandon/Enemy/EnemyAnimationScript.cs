using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationScript : MonoBehaviour
{
    EnemyChaseState enemy;
    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponentInParent<EnemyChaseState>();
    }

    public void ShootDaHoe()
    {
        enemy.ShootAtPlayer();
    }
}
