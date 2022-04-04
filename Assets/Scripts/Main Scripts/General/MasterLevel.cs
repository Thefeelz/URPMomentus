using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MasterLevel : MonoBehaviour
{
    [SerializeField] int enemiesToKill;
    [SerializeField] int currentKillCount;
    [SerializeField] Image progressBar;
    [SerializeField] Text completeText;
    [SerializeField] GameObject levelContinueDoor;
    [SerializeField] Animator elevatorAnimator;
    [SerializeField] int currentLevelBuildIndex;
    public bool levelComplete;

    private void Start()
    {
        // CheckForLevelComplete();
        if (GameManager.Instance)
        {
            GameManager.Instance.SetLevel(currentLevelBuildIndex);
        }
        currentKillCount = 0;
    }

    public void AddToKillCount(int numberToAdd)
    {
        currentKillCount += numberToAdd;
        if(progressBar)
            progressBar.fillAmount = (float)currentKillCount / enemiesToKill;
        CheckForLevelComplete();
    }

    public bool CheckForLevelComplete()
    {
        if (!levelComplete)
        {
            if (currentKillCount >= enemiesToKill)
            {
                //StartCoroutine(ShowWinText());
                // elevatorAnimator.SetBool("startElevator", true);
                //levelContinueDoor.SetActive(true);
                levelComplete = true;
                return true;
            }
            else
            {
                //completeText.gameObject.SetActive(false);
                return false;
            }
        }
        return true;
    }
    IEnumerator ShowWinText()
    {
        completeText.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);
        completeText.gameObject.SetActive(false);
        StopCoroutine(ShowWinText());
    }
}
