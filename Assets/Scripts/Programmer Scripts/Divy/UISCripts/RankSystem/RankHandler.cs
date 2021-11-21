using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/*
 * Momentus Style Ranking Handler
 * 
 * Purpose: To give the player a sense of score as they mow down enemies without taking a hit, this system promotes players to avoid getting hit in order to gain more health -> in turn leads to more overcharge. this script calculates
 * what rank the player is at anc then displays it on screen. 
 * 
 * Currently not implemented: refrence to player to get the amount of points they have.
 * 
 * created by Divyansh Malik / 10/28/2021
 * 
 * Modded by: / 11/11/2021
 * 
 * 
 * 
 */
public class RankHandler : MonoBehaviour
{

    private Rank[] StyleSystem = new Rank[6]; // array of 6 ranks

    [SerializeField]
    int currentRank;

    //testing
    [SerializeField]
    float currentPlayerPoints;

    [SerializeField]
    private Slider uiRankBar;

    [SerializeField]
    private Text uiFirstLetter; // the first letter of the rank the player is at progression of empty -> d-> c-> b-> -> a -> s

    [SerializeField]
    private Text uiRankWord; // the rest of the word in the text

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < StyleSystem.Length; ++i)
        {
            setInitialRankProperties(i);
        }

        currentRank = 0;
        
    }

    // Update is called once per frame
    void Update()
    {

        //display current rank
        DisplayRank(currentRank);

        // check if the current rank is 'Dull' and if the points <= 50. if true then set current rank to 0
        if (currentRank == 1 && currentPlayerPoints < 50f)
        {
            currentRank = 0;
            DisplayRank(currentRank);
        }
        //else change rank to dull
        else
        {
            // set the rank of the player
            SetPlayerRank(currentPlayerPoints);
        }

        
    }

    /// <summary>
    /// calculates and sets the rank of the player when playing the game. the function will first check if current amount of points held by the player is above 100 or below 0
    /// then using that information it will either increase or lower the player's current rank. if it's in between it will just set the current value of the rank to the new value
    /// </summary>
    /// <param name="points"> points is the number of points currently held by the player, if the player has more than 100 points then those points convert to the next rank otherwise they are decrimented from the previous rank</param>
    /// 
    private void SetPlayerRank(float currentPoints)
    {
        // if wihtin function it assumes the rank is greater than 1


        //check if the current points is above 100
        if (currentPoints < StyleSystem[currentRank].getMin_Value())
        {
            StyleSystem[currentRank].currentValue = StyleSystem[currentRank].getMin_Value(); // sets the current rank to 0 because you are less than the minimum amount of points

            currentRank = currentRank - 1; //decriments the rank
            StyleSystem[currentRank].currentValue = StyleSystem[currentRank].getMax_Value() - currentPoints; // decreases from the points of the previous rank
            

        }
        else if (currentPoints > StyleSystem[currentRank].getMax_Value())
        {
            StyleSystem[currentRank].currentValue = StyleSystem[currentRank].getMax_Value(); // set the maximum value if above 100

            // if the rank is not the final rank, then increment. Otherwise just set the current points to 100 and be done
            if (currentRank != 5)
            {
                currentRank = currentRank + 1;
                StyleSystem[currentRank].currentValue = StyleSystem[currentRank].getMin_Value() + currentPoints; // if you are advancing to the next round it will always be 0
               
            }
            

        }
        else
        {

            StyleSystem[currentRank].currentValue = currentPoints;


        }


    }


    /// <summary>
    /// this function exists the display the current rank on the UI
    /// </summary>
    /// <param name="currentRank"> current rank is an integer that tells the function what element within the rank array should be displayed</param>
    /// Input: integer representation of element of rank within the array
    /// Output: displays correct rank on the screen
    private void DisplayRank(int currentRank)
    {

        uiFirstLetter.text = StyleSystem[currentRank].firstLetter;
        uiRankWord.text = StyleSystem[currentRank].rankWord;
        uiRankBar.value = StyleSystem[currentRank].currentValue;



    }

    /// <summary>
    /// This function sets the base values for all the ranks in the system through a switchstatement
    /// </summary>
    /// <param name="rank">the rank is an integer which is used as a refrence to a certain rank object within the array</param>
    /// input: an integer that represents which element is being set in the rank Array
    private void setInitialRankProperties(int rank)
    {
            switch (rank)
            {
                case 1:
                    StyleSystem[rank].firstLetter = "D";
                    StyleSystem[rank].rankWord = "ull";
                    StyleSystem[rank].healthGainRate = 2f;
                    StyleSystem[rank].decrimentRate = 3f;
                    StyleSystem[rank].currentValue = StyleSystem[rank].getMin_Value();
                    break;

                case 2:
                    StyleSystem[rank].firstLetter = "C";
                    StyleSystem[rank].rankWord = "razy";
                    StyleSystem[rank].healthGainRate = 4f;
                    StyleSystem[rank].decrimentRate = 4f;
                    StyleSystem[rank].currentValue = StyleSystem[rank].getMin_Value();
                break;

                case 3:
                    StyleSystem[rank].firstLetter = "B";
                    StyleSystem[rank].rankWord = "ased";
                    StyleSystem[rank].healthGainRate = 6f;
                    StyleSystem[rank].decrimentRate = 5f;
                    StyleSystem[rank].currentValue = StyleSystem[rank].getMin_Value();
                break;


                case 4:
                    StyleSystem[rank].firstLetter = "A";
                    StyleSystem[rank].rankWord = "stronomical";
                    StyleSystem[rank].healthGainRate = 8.5f;
                    StyleSystem[rank].decrimentRate = 6f;
                    StyleSystem[rank].currentValue = StyleSystem[rank].getMin_Value();
                break;


                case 5:
                    StyleSystem[rank].firstLetter  = "S";
                    StyleSystem[rank].rankWord  = "pacial";
                    StyleSystem[rank].healthGainRate = 13f;
                    StyleSystem[rank].decrimentRate = 7f;
                    StyleSystem[rank].currentValue = StyleSystem[rank].getMin_Value();
                break;

                default:
                    StyleSystem[rank].firstLetter  = "";
                    StyleSystem[rank].rankWord  = "";
                    StyleSystem[rank].healthGainRate = 1f;
                    StyleSystem[rank].decrimentRate = 2f;
                    StyleSystem[rank].currentValue = StyleSystem[rank].getMin_Value();
                break;
            }


        }


}
