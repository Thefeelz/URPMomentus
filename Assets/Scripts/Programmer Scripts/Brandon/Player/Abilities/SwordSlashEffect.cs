using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSlashEffect : MonoBehaviour
{
    // Enemies able to hit before 
    [SerializeField] int enemiesAbleToHit = 3;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroySelf());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }

    // This is called when the sword slash goes through enemies
    private void OnTriggerEnter(Collider other)
    {
        // Check to see if the component is in the parent of the collider
        if(other.GetComponentInParent<EnemyStats>())
        {
            other.GetComponentInParent<EnemyStats>().TakeDamage(20);
            enemiesAbleToHit--;
        } 
        // Check to see if the component in on the body of the collider
        else if (other.GetComponent<EnemyStats>())
        {
            other.GetComponent<EnemyStats>().TakeDamage(20);
            enemiesAbleToHit--;
        }
        // Check to see if this can destroy any more enemies before it iself is destroyed
        if(enemiesAbleToHit <= 0) { Destroy(gameObject); }
    }

    // This is called by the Sword_Slash OVercharge ability on creation
    public void SetEnemiesAbleToHit(int enemiesAbleToHit)
    {
        this.enemiesAbleToHit = enemiesAbleToHit;
    }
    // This is called by the Sword_Slash Overcharge ability on creation 
    public void SetVelocity(Vector3 direction, float speed)
    {
        GetComponent<Rigidbody>().velocity = direction * speed;
    }
}
