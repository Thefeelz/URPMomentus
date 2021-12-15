using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUpPlayer : MonoBehaviour
{
    [SerializeField] GameObject[] blade, cross, hilt, forearm, hand, finger;
    Material[] materialsToApply;
    // Start is called before the first frame update
    void Start()
    {
        materialsToApply = FindObjectOfType<GameManager>().GetMaterials();
        ApplyMaterials();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void ApplyMaterials()
    {
        foreach(GameObject mat in blade)
        {
            mat.GetComponent<MeshRenderer>().material = materialsToApply[0];
        }
        foreach (GameObject mat in cross)
        {
            mat.GetComponent<MeshRenderer>().material = materialsToApply[1];
        }
        foreach (GameObject mat in hilt)
        {
            mat.GetComponent<MeshRenderer>().material = materialsToApply[2];
        }
        foreach (GameObject mat in finger)
        {
            mat.GetComponent<SkinnedMeshRenderer>().material = materialsToApply[3];
        }
        foreach (GameObject mat in forearm)
        {
            mat.GetComponent<SkinnedMeshRenderer>().material = materialsToApply[4];
        }
        foreach (GameObject mat in hand)
        {
            mat.GetComponent<SkinnedMeshRenderer>().material = materialsToApply[5];
        }
    }
}
