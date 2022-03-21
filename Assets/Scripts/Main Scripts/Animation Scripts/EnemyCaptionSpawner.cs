using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCaptionSpawner : MonoBehaviour
{
    [SerializeField] GameObject takeDamageCaption;
    [SerializeField] Sprite[] spriteRenderers;
    [SerializeField] float timeCaptionIsActive;

    public void SpawnDamageCaption()
    {
        if (takeDamageCaption)
        {
            GameObject newObject = Instantiate(takeDamageCaption, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), Quaternion.identity);
            newObject.GetComponentInChildren<SpriteRenderer>().sprite = spriteRenderers[Random.Range(0, spriteRenderers.Length - 1)];
            newObject.transform.LookAt(Camera.main.transform.position);
            Destroy(newObject, timeCaptionIsActive);
        }
    }

    public void DestroyCaption() { Destroy(this); }
}
