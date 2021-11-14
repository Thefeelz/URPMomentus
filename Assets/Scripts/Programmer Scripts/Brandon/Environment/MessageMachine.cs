using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageMachine : MonoBehaviour
{
    [SerializeField] bool oneShotMessage;
    [SerializeField] bool onExitTurnOff;
    [TextArea(15, 20)]
    [SerializeField] string messageOnEnter;
    [SerializeField] float messageOnEnterLength;
    [TextArea(15, 20)]
    [SerializeField] string messageOnStay;
    [SerializeField] float messageOnStayLength;
    [TextArea(15, 20)]
    [SerializeField] string messageOnExit;
    [SerializeField] float messageOnExitLength;

    MessageToPlayer messageToPlayer;
    bool stayCoroutineStarted = false;
    bool enterMessageComplete = false;
    private void Awake()
    {
        messageToPlayer = FindObjectOfType<MessageToPlayer>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponentInParent<P_Input>() && messageOnEnter != null)
            messageToPlayer.SetMessageToScreen(messageOnEnter, messageOnEnterLength);
        if (oneShotMessage && messageOnStay == null && messageOnExit == null)
            gameObject.SetActive(false);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponentInParent<P_Input>() && messageOnStay != null)
        {
            if(messageOnEnter != null && !stayCoroutineStarted && !enterMessageComplete)
            {
                StartCoroutine(MessageDelay(messageOnEnterLength, messageOnStay, messageOnStayLength));
            }
            else if (enterMessageComplete)
                messageToPlayer.SetMessageToScreen(messageOnStay, messageOnStayLength);
            else if(messageOnEnter == null)
                messageToPlayer.SetMessageToScreen(messageOnStay, messageOnStayLength);

        }
        if (oneShotMessage && messageOnExit == null)
            gameObject.SetActive(false);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<P_Input>() && messageOnExit != null)
            messageToPlayer.SetMessageToScreen(messageOnExit, messageOnExitLength);
        if (oneShotMessage)
            gameObject.SetActive(false);
        if (onExitTurnOff && other.GetComponent<P_Input>() && messageOnExit == null)
            messageToPlayer.SetMessageToScreen(" ", 0f);
    }

    IEnumerator MessageDelay(float delayAmount, string messageToSend, float messageLength)
    {
        stayCoroutineStarted = true;
        yield return new WaitForSeconds(delayAmount);
        messageToPlayer.SetMessageToScreen(messageToSend, messageLength);
        enterMessageComplete = true;
    }
}
