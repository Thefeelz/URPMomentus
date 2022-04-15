using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretRatatatatatata : MonoBehaviour
{
    [SerializeField] GameObject ra, ta;
    [SerializeField] float timeBetweenImages;
    public bool raBool = false;
    public bool right = false;
    public bool ready = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void FireRaTaTa(GameObject transformz)
    {
        if (!ready) { return; }
        if (!raBool)
        {
            GameObject raObject = Instantiate(ra, transformz.transform.position + Vector3.up, Quaternion.identity);
            raObject.transform.LookAt(FindObjectOfType<CharacterStats>().transform);
            raObject.GetComponent<Animator>().SetBool("left", true);
            StartCoroutine(TurnReadyOn());
            Destroy(raObject, 2f);
            raBool = true;
        }
        else if (raBool)
        {
            if (right)
            {
                GameObject taObject = Instantiate(ta, transformz.transform.position + Vector3.up - transformz.transform.right, Quaternion.identity);
                taObject.transform.LookAt(FindObjectOfType<CharacterStats>().transform);
                taObject.GetComponent<Animator>().SetBool("right", true);
                StartCoroutine(TurnReadyOn());
                Destroy(taObject, 2f);
            }
            else
            {
                GameObject taObject = Instantiate(ta, transformz.transform.position + Vector3.up - transformz.transform.right, Quaternion.identity);
                taObject.transform.LookAt(FindObjectOfType<CharacterStats>().transform);
                taObject.GetComponent<Animator>().SetBool("left", true);
                StartCoroutine(TurnReadyOn());
                Destroy(taObject, 2f);
            }
            right = !right;
        }
    }

    public void EndedFiring() { raBool = false; }

    IEnumerator TurnReadyOn()
    {
        ready = false;
        yield return new WaitForSeconds(timeBetweenImages);
        ready = true;
    }
}
