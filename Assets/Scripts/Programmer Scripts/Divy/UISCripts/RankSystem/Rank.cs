using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
* Momentus Rank Object
* 
* Purpose: TTo store all the information about the each rank in it's own special object to make it easier for the rank handler to change it's values.
* as well as protecting the data of the rank in it's own place.
* 
* created by Divyansh Malik / 11/11/2021
* 
* Modded by: Divyansh Malik / 11/11/2021
* 
*/



public class Rank
{
	private int MAX_Value = 100; // maximum points

	private int MIN_Value = 0; // minimum points

	public string firstLetter; // the first letter which represents the style rank the player is at

	public string rankWord; // the remaining parts of the word that represents the current rank the player is at

	public float decrimentRate; // how fast the style bar decreases

	public float healthGainRate; // rate of health gained from killing an enemy

	public float currentValue; //current amount of points the player has




	public int getMin_Value()
	{
		return MIN_Value;
	}

	public int getMax_Value()
	{

		return MAX_Value;
	}
}
