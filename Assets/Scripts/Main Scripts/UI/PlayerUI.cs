using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] Image HealthBar;
    [SerializeField] Image OverchargeBar;
    [SerializeField] Image healthBarDots;

    [SerializeField] Image airDashImage;
    [SerializeField] Image bladeDanceImage;
    [SerializeField] Image containedHeatImage;
    [SerializeField] Image swordSlashImage;

    CharacterStats ourPlayer;

    // Start is called before the first frame update
    void Start()
    {
        ourPlayer = GetComponent<CharacterStats>();
        //healthBarDots = GetComponentInChildren<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        HealthBar.fillAmount = ourPlayer.GetPlayerHealth() / ourPlayer.GetPlayerMaxHealth();
        OverchargeBar.fillAmount = ourPlayer.GetPlayerOvercharge() / ourPlayer.GetPlayerMaxOvercharge();
        //healthBarDots.fillAmount = HealthBar.fillAmount;
    }
    public void UpdateAirDashFill(float fillAmount)
    {
        airDashImage.fillAmount = fillAmount;
    }
    public void UpdateBladeDanceFill(float fillAmount)
    {
        bladeDanceImage.fillAmount = fillAmount;
    }
    public void UpdateContainedHeatFill(float fillAmount)
    {
        containedHeatImage.fillAmount = fillAmount;
    }
    public void UpdateSwordSlashFill(float fillAmount)
    {
        swordSlashImage.fillAmount = fillAmount;
    }
}
