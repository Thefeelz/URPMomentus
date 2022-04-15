using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordLightningEffect : MonoBehaviour
{
    // ==========THIS WHOLE CLASS CONTROLS THE GLOW OF THE SWORD DURING PLAY, IT INCREASES THE EMISSION BASED ON THE CURRENT OVERCHARGE========== NO TOUCHY PLZ <3
    CharacterStats player;
    ParticleSystem.EmissionModule thisSystem;
    [SerializeField] Material[] emissionMaterial;
    [SerializeField] GameObject[] emissionableGameObjects;
    Color color;
    bool controlEffect = true;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<CharacterStats>();
        thisSystem = GetComponent<ParticleSystem>().emission;
        emissionMaterial = GameManager.Instance.GetMaterials();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (controlEffect && player)
        {
            thisSystem.rateOverTime = Mathf.Floor(player.GetPlayerOvercharge() / 10);
            for (int i = 0; i < emissionMaterial.Length; i++)
            {
                emissionMaterial[i].SetColor("_EmissionColor", Color.white * (player.GetPlayerOvercharge() / 10 + 1));
            }
        }
    }
    public void SwordSlashControlsLightningAndGlow(bool value)
    {
        controlEffect = !value;
    }
    
}
