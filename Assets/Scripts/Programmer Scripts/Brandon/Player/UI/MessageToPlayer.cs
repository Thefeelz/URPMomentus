using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageToPlayer : MonoBehaviour
{
    Text messageText;
    void Start()
    {
        messageText = GetComponent<Text>();
        messageText.gameObject.SetActive(false);
    }
    public void SetMessageToScreen(string message, float timeForDisplay)
    {
        StopAllCoroutines();
        messageText.gameObject.SetActive(true);
        messageText.text = message;
        StartCoroutine(DisplayMessage(timeForDisplay));
    }
    IEnumerator DisplayMessage(float timeForDisplay)
    {

        yield return new WaitForSeconds(timeForDisplay);
        messageText.gameObject.SetActive(false);
    }
}
