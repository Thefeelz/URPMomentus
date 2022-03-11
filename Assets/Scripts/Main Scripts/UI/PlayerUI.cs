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

    // Start is called before the first frame update
    void Start()
    {
        opacityIncreaseOnDamage = opacityIncreaseOnDamage / 100;
        opacityDecreaseRateOverTime = opacityDecreaseRateOverTime / 100;
        ourPlayer = GetComponent<CharacterStats>();
        
    }

    // Update is called once per frame
    void Update()
    {
        HealthBar.fillAmount = ourPlayer.GetPlayerHealth() / ourPlayer.GetPlayerMaxHealth();
        OverchargeBar.fillAmount = ourPlayer.GetPlayerOvercharge() / ourPlayer.GetPlayerMaxOvercharge();
        if (hexDamageOverlayImage.color.a > 0)
            UpdatedamageOverlayOverTime();
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

    public void UpdateDamageOverlayOnDamageTaken()
    {
        Color c = hexDamageOverlayImage.color;
        hexDamageOverlayImage.color = new Color(c.r, c.g, c.b, (c.a + opacityIncreaseOnDamage));
    }
    public void UpdatedamageOverlayOverTime()
    {
        Debug.Log("In the update over time function");
        Color c = hexDamageOverlayImage.color;
        hexDamageOverlayImage.color = new Color(c.r, c.g, c.b, c.a - (opacityDecreaseRateOverTime * Time.deltaTime));
    }
}
