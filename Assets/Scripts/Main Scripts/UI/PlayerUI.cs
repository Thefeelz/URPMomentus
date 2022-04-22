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

    [Header("Damage Overlay")]
    [SerializeField] Image hexDamageOverlayImage;
    [SerializeField] float opacityIncreaseOnDamage, opacityDecreaseRateOverTime;

    CharacterStats ourPlayer;
    int bladeDanceCost, bladeSlashCost, airDashcost, containedHeatCost;

    // Start is called before the first frame update
    void Awake()
    {
        opacityIncreaseOnDamage = opacityIncreaseOnDamage / 100;
        opacityDecreaseRateOverTime = opacityDecreaseRateOverTime / 100;
        ourPlayer = GetComponent<CharacterStats>();
        bladeDanceCost = ourPlayer.GetComponent<A_BladeDance>().GetOverchargeCost();
        bladeSlashCost = ourPlayer.GetComponent<A_SwordSlash>().GetOverchargeCost();
        airDashcost = ourPlayer.GetComponent<A_AirDash>().GetOverchargeCost();
        containedHeatCost = ourPlayer.GetComponent<A_ContainedHeat>().GetOverchargeCost();
        UpdateUIColors();
    }

    // Update is called once per frame
    void Update()
    {
        HealthBar.fillAmount = ourPlayer.GetPlayerHealth() / ourPlayer.GetPlayerMaxHealth();
        OverchargeBar.fillAmount = ourPlayer.GetPlayerOvercharge() / ourPlayer.GetPlayerMaxOvercharge();
        if (hexDamageOverlayImage.color.a > 0)
            UpdatedamageOverlayOverTime();
        // UpdateUIColors();
    }
    public void UpdateAirDashFill(float fillAmount, float abilityCost)
    {
        airDashImage.fillAmount = fillAmount;
    }
    public void UpdateBladeDanceFill(float fillAmount, float abilityCost)
    {
        bladeDanceImage.fillAmount = fillAmount;
    }
    public void UpdateContainedHeatFill(float fillAmount, float abilityCost)
    {
        containedHeatImage.fillAmount = fillAmount;
    }
    public void UpdateSwordSlashFill(float fillAmount, float abilityCost)
    {
        swordSlashImage.fillAmount = fillAmount;
    }

    public void UpdateDamageOverlayOnDamageTaken()
    {
        Color c = hexDamageOverlayImage.color;
        hexDamageOverlayImage.color = new Color(c.r, c.g, c.b, (c.a + opacityIncreaseOnDamage));
    }
    public void UpdatedamageOverlayOverTime()
    {
        Color c = hexDamageOverlayImage.color;
        hexDamageOverlayImage.color = new Color(c.r, c.g, c.b, c.a - (opacityDecreaseRateOverTime * Time.deltaTime));
    }

    public void UpdateUIColors()
    {
        float currentOverCharge = ourPlayer.GetPlayerOvercharge();
        if (bladeDanceCost > currentOverCharge)
            UpdateUIImageColor(bladeDanceImage, false);
        else
            UpdateUIImageColor(bladeDanceImage, true);
        if (bladeSlashCost > currentOverCharge)
            UpdateUIImageColor(swordSlashImage, false);
        else
            UpdateUIImageColor(swordSlashImage, true);
        if (containedHeatCost > currentOverCharge)
            UpdateUIImageColor(containedHeatImage, false);
        else
            UpdateUIImageColor(containedHeatImage, true);
        if (airDashcost > currentOverCharge)
        {
            //Debug.Log("Air Dash Costs more than you got! Image Color is " + airDashImage.color + " name is " + airDashImage.name);
            UpdateUIImageColor(airDashImage, false);
            //Debug.Log("Updated Color is " + airDashImage.color + " name is " + airDashImage.name);
        }
        else
        {
            //Debug.Log("Air Dash Costs less than you got! Image Color is " + airDashImage.color + " name is " + airDashImage.name);
            UpdateUIImageColor(airDashImage, true);
            //Debug.Log("Updated Color is " + airDashImage.color + " name is " + airDashImage.name);
        }
    }
    void UpdateUIImageColor(Image image, bool value)
    {
        if (!value)
            image.color = Color.red;
        else
            image.color = Color.white;
    }
}
