using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCaptionSpawner : MonoBehaviour
{
    [SerializeField] GameObject takeDamageCaption;
    [SerializeField] Sprite[] spriteRenderers;
    // Start is called before the first frame update
    public void SpawnDamageCaption()
    {
        GameObject newObject = Instantiate(takeDamageCaption, transform.position, Quaternion.identity);
        newObject.GetComponentInChildren<SpriteRenderer>().sprite = spriteRenderers[Random.Range(0, spriteRenderers.Length - 1)];
        newObject.transform.LookAt(Camera.main.transform.position);
    }
}
