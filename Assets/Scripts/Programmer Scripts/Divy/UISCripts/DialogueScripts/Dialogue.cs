using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/*
 * Momentus Dialogue Object
 * 
 * Purpose: To be the basis of all the dialogue within the game. It sets up the structure, position, and general setup of the dialogue
 * 
 * created by Divyansh Malik / 11/02/2021
 * 
 * Modded by: Divyansh Malik / --/--/----
 * 
 */

/* makes it so that the dialogue can be edited and seen in the inspector */
[System.Serializable]


public class Dialogue
{	
	public string name; // name of the person speaking the dialogue

	[TextArea(3,10)]
	public string[] scentences; // array of scentences which make up a single piece of dialogue.

	public Sprite characterProfilePic;

	public bool HasImage;
	
}