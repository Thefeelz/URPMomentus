using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementScript : MonoBehaviour
{
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
    GameObject currentSelelection, nextSelection;
    float elapsedTime = 0f;
    float currentLerpPos = 0f;
    bool transitioning = false;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = camPosStartMainMenu.position;
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
        transform.position = Vector3.Lerp(startingPos, endingPos, currentLerpPos);
        transform.rotation = Quaternion.Lerp(startingRot, endingRot, currentLerpPos);
        if(currentLerpPos >= 1)
        {
            currentSelelection.SetActive(false);
            nextSelection.SetActive(true);
            transitioning = false;
            elapsedTime = 0f;
        }
    }

    public void PlayButtonClicked()
    {
        startingPos = transform.position;
        startingRot = transform.rotation;
        endingPos = camPosPlayButton.position;
        endingRot = camPosPlayButton.rotation;
        currentSelelection = mainMenuSelectionObject;
        nextSelection = levelSelectionObject;
        transitioning = true;
    }
    public void LevelButtonClicked()
    {
        startingPos = transform.position;
        startingRot = transform.rotation;
        endingPos = camPosLoadOutSelection.position;
        endingRot = camPosLoadOutSelection.rotation;
        currentSelelection = levelSelectionObject;
        nextSelection = loadOutSelectionObject;
        transitioning = true; 
    }
    public void LoadOutBackButtonClicked()
    {
        startingPos = transform.position;
        startingRot = transform.rotation;
        endingPos = camPosPlayButton.position;
        endingRot = camPosPlayButton.rotation;
        currentSelelection = loadOutSelectionObject; 
        nextSelection = levelSelectionObject;
        transitioning = true;
    }
    public void SettingsBackButtonClicked()
    {
        startingPos = transform.position;
        startingRot = transform.rotation;
        endingPos = camPosStartMainMenu.position;
        endingRot = camPosStartMainMenu.rotation;
        currentSelelection = settingsObject;
        nextSelection = mainMenuSelectionObject;
        transitioning = true;
    }
    public void CreditsBackButtonClicked()
    {
        startingPos = transform.position;
        startingRot = transform.rotation;
        endingPos = camPosStartMainMenu.position;
        endingRot = camPosStartMainMenu.rotation;
        currentSelelection = creditsObject;
        nextSelection = mainMenuSelectionObject;
        transitioning = true;
    }
    public void LevelSelectButtonClicked()
    {
        startingPos = transform.position;
        startingRot = transform.rotation;
        endingPos = camPosStartMainMenu.position;
        endingRot = camPosStartMainMenu.rotation;
        currentSelelection = levelSelectionObject;
        nextSelection = mainMenuSelectionObject;
        transitioning = true;
    }
    public void SettingsButtonClicked()
    {
        startingPos = transform.position;
        startingRot = transform.rotation;
        endingPos = camPosSettings.position;
        endingRot = camPosSettings.rotation;
        currentSelelection = mainMenuSelectionObject;
        nextSelection = settingsObject;
        transitioning = true;
        
    }
    public void CreditsButtonClicked()
    {
        startingPos = transform.position;
        startingRot = transform.rotation;
        endingPos = camPosCredits.position;
        endingRot = camPosCredits.rotation;
        currentSelelection = mainMenuSelectionObject;
        nextSelection = creditsObject;
        transitioning = true;
    }
}
