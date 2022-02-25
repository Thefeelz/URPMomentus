using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuSettingsSetUp : MonoBehaviour
{
    [SerializeField] Slider mouseSensitivitySlider;
    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        mouseSensitivitySlider.value = gameManager.GetMouseSensitivity();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
