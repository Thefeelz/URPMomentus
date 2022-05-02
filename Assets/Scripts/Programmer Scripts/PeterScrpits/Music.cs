using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Music : MonoBehaviour
{
    public FMODUnity.EventReference mainMenuRef;
    public FMODUnity.EventReference tutorialMusicRef;
    public FMODUnity.EventReference tutorialPrecombatRef;
    public FMODUnity.EventReference tutorialTransitionRef;
    public FMODUnity.EventReference levelOneMusicRef;
    //public FMODUnity.ParamRef 

    FMOD.Studio.EventInstance mainMenuMusic;
    FMOD.Studio.EventInstance levelOneMusic;
    FMOD.Studio.EventInstance TutorialPrecombat;
    FMOD.Studio.EventInstance tutorialMusic;
    // Start is called before the first frame update
    void Start()
    {
        mainMenuMusic = FMODUnity.RuntimeManager.CreateInstance(mainMenuRef);
        tutorialMusic= FMODUnity.RuntimeManager.CreateInstance(tutorialMusicRef);
        levelOneMusic= FMODUnity.RuntimeManager.CreateInstance(levelOneMusicRef);

        TutorialPrecombat = FMODUnity.RuntimeManager.CreateInstance(tutorialPrecombatRef);


    }

    public void PreCombatTut()
    {
        TutorialPrecombat.start();

    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
    }
    public void TransitionTut()
    {
        TutorialPrecombat.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        TutorialPrecombat.release();
        
    }
}
