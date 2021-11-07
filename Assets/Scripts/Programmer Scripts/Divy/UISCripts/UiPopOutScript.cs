using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Momentus Mission Control UI animation handling
 * 
 * Purpose: To animate the mission control on the screen, activate the text, and then close the mission control when finished
 * 
 * created by Divyansh Malik / 10/26/2021
 * 
 * Modded by: Divyansh Malik / 10/27/2021
 * 
 * Changes: removed all semblence of coroutines in favor of using unity animations for opening and closing the screen
 */


public class UiPopOutScript : MonoBehaviour
{
    [SerializeField]
    private GameObject mControlUIObject;

    // Update is called once per frame
    void Update()
    {

        MissionControlUIHandler();
    }

    void MissionControlUIHandler()
    {
        //turn on the mission control UI with it's animation

        if (Input.GetKeyDown(KeyCode.D))
        {

            mControlUIObject.SetActive(true);

        }

        //stop the animation at the last frame (frame 70)


        //add in the animation for the icon if the mission control

        //play the text


        ///stuff to do after text stops playing

        //check if text is finished playing

        //play animation of icon appearing in reverse and make the object dissapear

        //when finished scrolling reverse the animation for the mission control set the object to invisible

        //switch off the mission control

    }

    public void AnimtationStop(Animation missionControl)
    {

        missionControl.Stop();

    }


}
