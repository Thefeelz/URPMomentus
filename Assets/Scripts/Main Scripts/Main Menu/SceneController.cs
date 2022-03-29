using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void StartTheGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void MainMenu()
    {
        UnfreezeTime();
        Cursor.lockState = CursorLockMode.Confined;
        SceneManager.LoadScene(0);
    }
    public void EndScene()
    {
        SceneManager.LoadScene(2);
    }
    public void ChooseSceneByIndex(int index)
    {
        SceneManager.LoadScene(index);
    }
    public void ChooseSceneByGameManager()
    {
        SceneManager.LoadScene(FindObjectOfType<GameManager>().GetLevel());
    }
    public void UnfreezeTime()
    {
        Time.timeScale = 1f;
    }
    public void GoToNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void QuitTheFuckingGame()
    {
        Application.Quit();
    }
    public void GoToDeathScene()
    {
        Debug.Log(SceneManager.sceneCount);
        SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings - 1);
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(FindObjectOfType<GameManager>().GetLevel());
    }
}
