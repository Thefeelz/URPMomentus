using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject playerHUD;
    // Start is called before the first frame update
    private void Start()
    {
        if (!GameIsPaused)
            pauseMenuUI.SetActive(false);
    }
    public void PauseGame()
    {
        if(GameIsPaused)
        {
            Resume();
        }
        else
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
}
