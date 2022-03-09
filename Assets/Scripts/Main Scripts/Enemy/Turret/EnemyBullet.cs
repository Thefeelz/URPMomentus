using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    CharacterStats player;
    float maxLife = 1f;
    float timeAlive = 0f;
    float velocity = 0;
    float damage = 10f;
    // Start is called before the first frame update
    void Start()
    {
        // player = FindObjectOfType<CharacterStats>();
    }

    // Update is called once per frame
    void Update()
    {
        timeAlive += Time.deltaTime;
        if(timeAlive >= maxLife)
        {
            Destroy(transform.root.gameObject);
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<CharacterStats>())
        {
            other.GetComponentInParent<CharacterStats>().RemoveHealthMelee(damage);
        }
    }
    public void SetVelocityToPlayer(float _velocity, CharacterStats _player, Transform headToRotate, float _damage)
    {
        damage = _damage;
        player = _player;
        transform.rotation = headToRotate.transform.rotation;
        GetComponent<Rigidbody>().velocity = transform.forward * _velocity;
    }
}
