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

    FMOD.Studio.EventInstance mainMenuMusic;
    FMOD.Studio.EventInstance levelOneMusic;
    FMOD.Studio.EventInstance tutorialMusic;
    // Start is called before the first frame update
    void Start()
    {
        mainMenuMusic = FMODUnity.RuntimeManager.CreateInstance(mainMenuRef);
        tutorialMusic= FMODUnity.RuntimeManager.CreateInstance(tutorialMusicRef);
        levelOneMusic= FMODUnity.RuntimeManager.CreateInstance(levelOneMusicRef);


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
