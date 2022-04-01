using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUpPlayer : MonoBehaviour
{
    [SerializeField] GameObject[] blade, cross, hilt, forearm, hand, finger, shield;
    [SerializeField] GameObject swordLighting;
    Material[] materialsToApply;
    ParticleSystem[] particlesystemToApply;
    [SerializeField] Material[] defaultMaterialsToApply;
    
    // Start is called before the first frame update
    void Start()
    {
        materialsToApply = GameManager.Instance.GetMaterials();
        particlesystemToApply = GameManager.Instance.GetParticleSystems();
        if (materialsToApply.Length == 0)
            Debug.Log("Set up is failing");
        ApplyMaterials();
        ApplyParticleSystems();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void ApplyMaterials()
    {
        if (materialsToApply.Length > 0)
        {
            //Debug.Log("Blade Not Null");
            foreach (GameObject mat in blade)
            {
                mat.GetComponent<MeshRenderer>().material = materialsToApply[0];
            }
        } 
        else
        {
            //Debug.Log("Blade Null");
            foreach (GameObject mat in blade)
            {
                mat.GetComponent<MeshRenderer>().material = defaultMaterialsToApply[0];
            }
        }

        if (materialsToApply.Length > 1)
        {
            //Debug.Log("Cross Not Null");
            foreach (GameObject mat in cross)
            {
                mat.GetComponent<MeshRenderer>().material = materialsToApply[1];
            }
        }
        else
        {
            //Debug.Log("Cross Null");
            foreach (GameObject mat in cross)
            {
                mat.GetComponent<MeshRenderer>().material = defaultMaterialsToApply[1];
            }
        }

        if (materialsToApply.Length > 2)
        {
            //Debug.Log("Hilt Not Null");
            foreach (GameObject mat in hilt)
            {
                mat.GetComponent<MeshRenderer>().material = materialsToApply[2];
            }
        }
        else
        {
            //Debug.Log("Hilt Null");
            foreach (GameObject mat in hilt)
            {
                mat.GetComponent<MeshRenderer>().material = defaultMaterialsToApply[2];
            }
        }

        if (materialsToApply.Length > 3)
        {
            //Debug.Log("Finger Not Null");
            foreach (GameObject mat in finger)
            {
                mat.GetComponent<SkinnedMeshRenderer>().material = materialsToApply[3];
            }
        }
        else
        {
            //Debug.Log("Finger Null");
            foreach (GameObject mat in finger)
            {
                mat.GetComponent<SkinnedMeshRenderer>().material = defaultMaterialsToApply[3];
            }
        }

        if (materialsToApply.Length > 4)
        {
            //Debug.Log("Forearm Not Null");
            foreach (GameObject mat in forearm)
            {
                mat.GetComponent<SkinnedMeshRenderer>().material = materialsToApply[4];
            }
        }
        else
        {
            //Debug.Log("Forearm Null");
            foreach (GameObject mat in forearm)
            {
                mat.GetComponent<SkinnedMeshRenderer>().material = defaultMaterialsToApply[4];
            }
        }

        if (materialsToApply.Length > 5)
        {
            //Debug.Log("Hand Not Null");
            foreach (GameObject mat in hand)
            {
                mat.GetComponent<SkinnedMeshRenderer>().material = materialsToApply[5];
            }
        }
        else
        {
            //Debug.Log("Hand Null");
            foreach (GameObject mat in hand)
            {
                mat.GetComponent<SkinnedMeshRenderer>().material = defaultMaterialsToApply[5];
            }
        }

        if (materialsToApply.Length > 6)
        {
            Debug.Log("Calling Shield Materials");
            foreach (GameObject mat in shield)
            {
                Material[] materialz = mat.GetComponent<MeshRenderer>().materials;
                materialz[0] = materialsToApply[6];
                materialz[1] = materialsToApply[7];
                
                mat.GetComponent<MeshRenderer>().materials = materialz;
            }
        }
        else
        {
            //Debug.Log("Hand Null");
            foreach (GameObject mat in shield)
            {
                Material[] materialz = mat.GetComponent<MeshRenderer>().materials;
                materialz[0] = defaultMaterialsToApply[6];
                materialz[1] = defaultMaterialsToApply[7];
                mat.GetComponent<MeshRenderer>().materials = materialz;
                
            }
        }
    }
    
    void ApplyParticleSystems()
    {
        // swordLighting.GetComponent<ParticleSystem>().main.startColor.color = particlesystemToApply[0].main.startColor.color;
        var main = swordLighting.GetComponent<ParticleSystem>().main;
        Debug.Log("Initial Colors");
        Debug.Log("[" + main.startColor.colorMin.r + ", " + main.startColor.colorMin.g + ", " + main.startColor.colorMin.b + "]");
        Debug.Log("[" + main.startColor.colorMax.r + ", " + main.startColor.colorMax.g + ", " + main.startColor.colorMax.b + "]");

        Debug.Log("Incoming Colors");
        Debug.Log("[" + particlesystemToApply[0].main.startColor.colorMin.r + ", " + particlesystemToApply[0].main.startColor.colorMin.g + ", " + particlesystemToApply[0].main.startColor.colorMin.b + "]");
        Debug.Log("[" + particlesystemToApply[0].main.startColor.colorMax.r + ", " + particlesystemToApply[0].main.startColor.colorMax.g + ", " + particlesystemToApply[0].main.startColor.colorMax.b + "]");


        main.startColor = particlesystemToApply[0].main.startColor;
        Debug.Log("Changed");
        Debug.Log("Changed Colors");
        Debug.Log("[" + main.startColor.colorMin.r + ", " + main.startColor.colorMin.g + ", " + main.startColor.colorMin.b + "]");
        Debug.Log("[" + main.startColor.colorMax.r + ", " + main.startColor.colorMax.g + ", " + main.startColor.colorMax.b + "]");

        FindObjectOfType<A_SwordSlash>().SetSwordSlashPrefab(GameManager.Instance.GetSwordSlashPrefab());
        FindObjectOfType<A_ContainedHeat>().SetContainedHeatPrefab(GameManager.Instance.GetContainedHeatPrefab());
        FindObjectOfType<A_ContainedHeat>().SetContainedHeatLightningEffect(particlesystemToApply[1]);
    }
}
