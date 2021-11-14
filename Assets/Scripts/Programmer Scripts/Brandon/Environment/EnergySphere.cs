using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergySphere : MonoBehaviour
{
    [SerializeField] int energyAmount;

    // If Life Time is set to -1, orb will only despawn if picked up
    [SerializeField] float lifeTime;
    // Start is called before the first frame update
    void Start()
    {
        if(lifeTime >= 0)
            StartCoroutine(DespawnEneryOrb());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponentInParent<P_Input>())
        {
            other.GetComponentInParent<CharacterStats>().ReplenishHealth(energyAmount);
            Destroy(gameObject);
        }
    }

    IEnumerator DespawnEneryOrb()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}
