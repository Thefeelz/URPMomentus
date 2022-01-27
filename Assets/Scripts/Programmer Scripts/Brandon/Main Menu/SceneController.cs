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
}
