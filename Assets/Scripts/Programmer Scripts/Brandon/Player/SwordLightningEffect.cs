using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordLightningEffect : MonoBehaviour
{
    CharacterStats player;
    ParticleSystem.EmissionModule thisSystem;
    [SerializeField] Material[] emissionMaterial;
    Color color;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<CharacterStats>();
        thisSystem = GetComponent<ParticleSystem>().emission;
    }

    // Update is called once per frame
    void Update()
    {
        thisSystem.rateOverTime = Mathf.Floor(player.GetPlayerOvercharge() / 10);
        for (int i = 0; i < emissionMaterial.Length; i++)
        {
            emissionMaterial[i].SetColor("_EmissionColor", Color.white * player.GetPlayerOvercharge() / 10);
        }
    }
}
