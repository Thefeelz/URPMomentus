using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCaptionSpawner : MonoBehaviour
{
    [SerializeField] GameObject takeDamageCaption;
    [SerializeField] Sprite[] spriteRenderers;
    [SerializeField] float timeCaptionIsActive;
    GameObject newObject;

    private void Update()
    {
        if (newObject && Camera.main.transform)
            newObject.transform.LookAt(Camera.main.transform.position);
        else if (newObject && !Camera.main)
            newObject.transform.LookAt(Camera.current.transform.position);
    }
    public void SpawnDamageCaption()
    {
        if (takeDamageCaption)
        {
            newObject = Instantiate(takeDamageCaption, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), Quaternion.identity);
            newObject.GetComponentInChildren<SpriteRenderer>().sprite = spriteRenderers[Random.Range(0, spriteRenderers.Length - 1)];
            newObject.transform.LookAt(Camera.main.transform.position);
            Destroy(newObject, timeCaptionIsActive);
        }
    }

    public void DestroyCaption() { Destroy(this); }
}
