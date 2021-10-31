using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
 * Momentus Style Ranking Handler
 * 
 * Purpose: To give the player a sense of score as they mow down enemies without taking a hit, this system promotes players to avoid getting hit in order to gain more health -> in turn leads to more overcharge.
 * 
 * created by Divyansh Malik / 10/28/2021
 * 
 * Modded by: / --/--/----
 * 
 */


public class StyleRankScript : MonoBehaviour
{
    [SerializeField]
    private float points; // the amount of points the player gains from killing

    [SerializeField]
    private float meter; // the amount of points the player currently has

    [SerializeField]
    private float styleBarDecrimentRate; // the rate at which the Style Bar decreases

    [SerializeField]
    private float healthGainRate; // the rate at which you gain health byt attacking or killing an enemy.

    [SerializeField]
    private int Rank; // the number which sets the rank word,first letter, decriment rate, and health gain rate

    [SerializeField]
    private float styleBarMax;

    [SerializeField]
    private Slider styleBar;

    [SerializeField]
    private  TMP_Text rankFirstLetter; // the first letter of the rank the player is at progression of empty -> d-> c-> b-> -> a -> s

    [SerializeField]
    private TMP_Text rankWord; // the rest of the word in the text

    /* for testing purposes these are dummy variables */


    public float health;
    public float overdrive;
    public int maxHealth = 100;
    public int maxOverdrive = 100;
    public int count; //enemy death count

    public Slider healthBar;
    public Slider overdriveBar;
    


    // Start is called before the first frame update
    void Start()
    {
        points = 20; // amount of points you get from killing an enemy
        meter = 0; // your style meter bar
        healthGainRate = 2f;
        styleBarDecrimentRate = 1f;
        rankFirstLetter.text = "";
        rankWord.text = "";
        health = 60;
        overdrive = 0;
        styleBarMax = 100;

        healthBar.maxValue = maxHealth;
        overdriveBar.maxValue = maxOverdrive;
        styleBar.maxValue = 600;

        styleBar.value = points;
        healthBar.value = health;
        overdriveBar.value = overdrive;

    }

    // Update is called once per frame
    void Update()
    {

        if (health > 0)
        {

            health -= 20 * .0001f;

        }

        if (Input.GetKeyDown(KeyCode.A))
        {

            meter += points;

            IncrimentHealth();
            

        }

        if (Input.GetKeyDown(KeyCode.D))
        {

            overdrive -= 30;


        }


        if (Input.GetKeyDown(KeyCode.S))
        {
            meter -= 100;
        }

        if (health > maxHealth)
        {
            float overcharge = health - maxHealth;
            health = 100;
            setOverdrive(overcharge);

        }

        UpdateRank(meter); //update the rank of the player
        setRankProperties(Rank); // set the properties of the rank

        if (meter > 0)
        {
            DecrimentMeterbarDecrimetMultiplier();
        }

        UpdateBars(health, meter, overdrive);

    }

	public void UpdateBars(float health, float meter, float overdrive)
	{
        styleBar.value = meter;
        healthBar.value = health;
        overdriveBar.value = overdrive;
    }

    /*
    void ResetStyleBar(int points)
    {

        meter = 0;


    }
    */

    void DecrimentMeterbarDecrimetMultiplier()
    {
        meter -= (.10f * styleBarDecrimentRate);
        styleBar.value = meter;

    }
   
    void IncrimentHealth()
    {
        health += 2 * healthGainRate;
        healthBar.value = health;

    }

    void setOverdrive(float overcharge)
    {
        overdrive += overcharge;
        overdriveBar.value = overdrive;
    }

    void UpdateRank( float points)
    {

        if (points >= 100 && points <= 200)
        {
            Rank = 1;
        }
        else if (points >= 200 && points <= 300)
        {
            Rank = 2;
        }
        else if (points >= 3000 && points <= 400)
        {
            Rank = 3;
        }
        else if (points >= 400 && points <= 500)
        {
            Rank = 4;
        }
        else if (points >= 500 && points <= 600)
        {
            Rank = 5;
        }
        else if (points > 600)
        {
            meter = 600;
        }

    }

    void setRankProperties(int Rank)
    {
        switch (Rank)
        {
            case 1:
                rankFirstLetter.text = "D";
                rankWord.text = "ull";
                healthGainRate = 2f;
                styleBarDecrimentRate = 3f;
                break;

            case 2:
                rankFirstLetter.text = "C";
                rankWord.text = "razy";
                healthGainRate = 4f;
                styleBarDecrimentRate = 4f;
                break;

            case 3:
                rankFirstLetter.text = "B";
                rankWord.text = "ased";
                healthGainRate = 6f;
                styleBarDecrimentRate = 5f;
                break;


            case 4:
                rankFirstLetter.text = "A";
                rankWord.text = "stronomical";
                healthGainRate = 8.5f;
                styleBarDecrimentRate = 6f;
                break;


            case 5:
                rankFirstLetter.text = "S";
                rankWord.text = "pacial";
                healthGainRate = 13f;
                styleBarDecrimentRate = 7f;
                break;

            default:
                rankFirstLetter.text = "";
                rankWord.text = "";
                healthGainRate = 1f;
                styleBarDecrimentRate = 2f;
                break;
        }


    }

}
