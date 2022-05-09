using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CameraMovementScript : MonoBehaviour
{
    [SerializeField] FMODUnity.EventReference MainMenuMusicRef;
    [SerializeField] FMODUnity.EventReference ButtonClickedRef;
    FMOD.Studio.EventInstance MainMusic;
    [SerializeField] Transform camPosStartMainMenu;
    [SerializeField] Transform camPosPlayButton;
    [SerializeField] Transform camPosLoadOutSelection;
    [SerializeField] Transform camPosSettings;
    [SerializeField] Transform camPosCredits;
    [SerializeField] float camTransitionTime = 2f;
    [SerializeField] GameObject mainMenuSelectionObject;
    [SerializeField] GameObject levelSelectionObject;
    [SerializeField] GameObject loadOutSelectionObject;
    [SerializeField] GameObject settingsObject;
    [SerializeField] GameObject creditsObject;
    Vector3 startingPos, endingPos;
    Quaternion startingRot, endingRot;
    public GameObject currentSelelection, nextSelection;
    float elapsedTime = 0f;
    float currentLerpPos = 0f;
    public bool transitioning = false;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = camPosStartMainMenu.position;
        GameManager.Instance.SetRespawnCheckpointIndex(0);
        MainMusic = FMODUnity.RuntimeManager.CreateInstance(MainMenuMusicRef);
        MainMusic.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        MainMusic.start();

    }

    // Update is called once per frame
    void Update()
    {
        if(transitioning)
            CameraTransition();
    }

    void CameraTransition()
    {
        
        elapsedTime += Time.deltaTime;
        currentLerpPos = elapsedTime / camTransitionTime;
        transform.position = Vector3.Lerp(startingPos, endingPos, Mathf.SmoothStep(0, 1,currentLerpPos));
        transform.rotation = Quaternion.Lerp(startingRot, endingRot, Mathf.SmoothStep(0, 1, currentLerpPos));
        if(currentLerpPos >= 1)
        {
            // currentSelelection.SetActive(false);
            nextSelection.SetActive(true);
            transitioning = false;
            elapsedTime = 0f;
        }
    }

    public void PlayButtonClicked()
    {
        FMODUnity.RuntimeManager.PlayOneShot(ButtonClickedRef, transform.position);
        if (transitioning) { return; }
        startingPos = transform.position;
        startingRot = transform.rotation;
        endingPos = camPosPlayButton.position;
        endingRot = camPosPlayButton.rotation;
        currentSelelection = mainMenuSelectionObject;
        nextSelection = levelSelectionObject;
        foreach (Transform toggle in currentSelelection.transform)
        {
            if (toggle.GetComponent<Toggle>())
                toggle.GetComponent<Toggle>().isOn = false;
        }
        currentSelelection.SetActive(false);
        transitioning = true;
    }
    public void TutorialButtonClicked()
    {
        FMODUnity.RuntimeManager.PlayOneShot(ButtonClickedRef, transform.position);
        if (transitioning) { return; }
        startingPos = transform.position;
        startingRot = transform.rotation;
        endingPos = camPosLoadOutSelection.position;
        endingRot = camPosLoadOutSelection.rotation;
        currentSelelection = mainMenuSelectionObject;
        nextSelection = loadOutSelectionObject;
        foreach (Transform toggle in currentSelelection.transform)
        {
            if (toggle.GetComponent<Toggle>())
                toggle.GetComponent<Toggle>().isOn = false;
        }
        currentSelelection.SetActive(false);
        transitioning = true;
    }
    public void LevelButtonClicked(int level)
    {
        FMODUnity.RuntimeManager.PlayOneShot(ButtonClickedRef, transform.position);
        if (transitioning) { return; }
        startingPos = transform.position;
        startingRot = transform.rotation;
        endingPos = camPosLoadOutSelection.position;
        endingRot = camPosLoadOutSelection.rotation;
        currentSelelection = levelSelectionObject;
        nextSelection = loadOutSelectionObject;
        foreach (Transform toggle in currentSelelection.transform)
        {
            if (toggle.GetComponent<Toggle>())
                toggle.GetComponent<Toggle>().isOn = false;
        }
        currentSelelection.SetActive(false);
        transitioning = true;
        if (GameManager.Instance)
            GameManager.Instance.SetLevel(level);
        else
            Debug.Log("No Game Manager Found");
    }
    public void LoadOutBackButtonClicked()
    {
        FMODUnity.RuntimeManager.PlayOneShot(ButtonClickedRef, transform.position);
        //MainMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        //MainMusic.release();
        if (transitioning) { return; }
        startingPos = transform.position;
        startingRot = transform.rotation;
        endingPos = camPosPlayButton.position;
        endingRot = camPosPlayButton.rotation;
        currentSelelection = loadOutSelectionObject; 
        nextSelection = levelSelectionObject;
        foreach (Transform toggle in currentSelelection.transform)
        {
            if (toggle.GetComponent<Toggle>())
                toggle.GetComponent<Toggle>().isOn = false;
        }
        currentSelelection.SetActive(false);
        transitioning = true;
    }
    public void SettingsBackButtonClicked()
    {
        FMODUnity.RuntimeManager.PlayOneShot(ButtonClickedRef, transform.position);
        if (transitioning) { return; }
        startingPos = transform.position;
        startingRot = transform.rotation;
        endingPos = camPosStartMainMenu.position;
        endingRot = camPosStartMainMenu.rotation;
        currentSelelection = settingsObject;
        nextSelection = mainMenuSelectionObject;
        foreach (Transform toggle in currentSelelection.transform)
        {
            if (toggle.GetComponent<Toggle>())
                toggle.GetComponent<Toggle>().isOn = false;
        }
        currentSelelection.SetActive(false);
        transitioning = true;
    }
    public void CreditsBackButtonClicked()
    {
        FMODUnity.RuntimeManager.PlayOneShot(ButtonClickedRef, transform.position);
        if (transitioning) { return; }
        startingPos = transform.position;
        startingRot = transform.rotation;
        endingPos = camPosStartMainMenu.position;
        endingRot = camPosStartMainMenu.rotation;
        currentSelelection = creditsObject;
        nextSelection = mainMenuSelectionObject;
        foreach (Transform toggle in currentSelelection.transform)
        {
            if (toggle.GetComponent<Toggle>())
                toggle.GetComponent<Toggle>().isOn = false;
        }
        currentSelelection.SetActive(false);
        transitioning = true;
    }
    public void LevelSelectBackButtonClicked()
    {
        FMODUnity.RuntimeManager.PlayOneShot(ButtonClickedRef, transform.position);
        if (transitioning) { return; }
        startingPos = transform.position;
        startingRot = transform.rotation;
        endingPos = camPosStartMainMenu.position;
        endingRot = camPosStartMainMenu.rotation;
        currentSelelection = levelSelectionObject;
        nextSelection = mainMenuSelectionObject;
        foreach (Transform toggle in currentSelelection.transform)
        {
            if (toggle.GetComponent<Toggle>())
                toggle.GetComponent<Toggle>().isOn = false;
        }
        currentSelelection.SetActive(false);
        transitioning = true;
    }
    public void SettingsButtonClicked()
    {
        FMODUnity.RuntimeManager.PlayOneShot(ButtonClickedRef, transform.position);
        if (transitioning) { return; }
        startingPos = transform.position;
        startingRot = transform.rotation;
        endingPos = camPosSettings.position;
        endingRot = camPosSettings.rotation;
        currentSelelection = mainMenuSelectionObject;
        nextSelection = settingsObject;
        foreach (Transform toggle in currentSelelection.transform)
        {
            if (toggle.GetComponent<Toggle>())
                toggle.GetComponent<Toggle>().isOn = false;
        }
        currentSelelection.SetActive(false);
        transitioning = true;
        
    }
    public void CreditsButtonClicked()
    {
        FMODUnity.RuntimeManager.PlayOneShot(ButtonClickedRef, transform.position);
        if (transitioning) { return; }
        startingPos = transform.position;
        startingRot = transform.rotation;
        endingPos = camPosCredits.position;
        endingRot = camPosCredits.rotation;
        currentSelelection = mainMenuSelectionObject;
        nextSelection = creditsObject;
        foreach (Transform toggle in currentSelelection.transform)
        {
            if (toggle.GetComponent<Toggle>())
                toggle.GetComponent<Toggle>().isOn = false;
        }
        currentSelelection.SetActive(false);
        transitioning = true;
    }
    public void ChooseSword(int swordChoice) { GameManager.Instance.SetBladeColor(swordChoice); }

    private void OnDestroy()
    {
        MainMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        MainMusic.release();
    }
}
