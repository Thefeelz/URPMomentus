using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    CharacterStats player;
    float maxLife = 3f;
    float timeAlive = 0f;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<CharacterStats>();
    }

    // Update is called once per frame
    void Update()
    {
        timeAlive += Time.deltaTime;
        if(timeAlive >= maxLife)
        {
            Destroy(gameObject);
        }
        if(Vector3.Distance(transform.position, player.transform.position) < 0.25f)
        {
            Debug.Log("Ouch Boi");
        }
    }
}
