using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject playerHUD;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] Slider mouseSensitivitySlider;
    GameManager myManager;
    // Start is called before the first frame update
    private void Start()
    {
        if (!GameIsPaused)
        {
            pauseMenuUI.SetActive(false);
            settingsMenu.SetActive(false);
        }
        myManager = FindObjectOfType<GameManager>();
        if (myManager)
            Debug.Log("Found Game Manager");
    }
    public void PauseGame()
    {
        if(GameIsPaused && pauseMenuUI.activeSelf)
        {
            Resume();
        }
        else if (!GameIsPaused)
        {
            Pause();
        }
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Debug.Log("Resume");
        GameIsPaused = false;
        pauseMenuUI.SetActive(false);
        playerHUD.SetActive(true);
        Time.timeScale = 1f;
    }

    void Pause()
    {
        Cursor.lockState = CursorLockMode.Confined;
        GameIsPaused = true;
        pauseMenuUI.SetActive(true);
        playerHUD.SetActive(false);
        Time.timeScale = 0f;
    }

    public void OpenSettingsMenu()
    {
        settingsMenu.SetActive(true);
        pauseMenuUI.SetActive(false);
    }

    public void CloseSettingsMenu()
    {
        settingsMenu.SetActive(false);
        pauseMenuUI.SetActive(true);
        myManager.SetMouseSenitivity(mouseSensitivitySlider.value);
    }

    public void ReturnToMainMenu()
    {
        settingsMenu.SetActive(false);
        pauseMenuUI.SetActive(false);
        GameIsPaused = false;
        FindObjectOfType<SceneController>().MainMenu();
    }
}
