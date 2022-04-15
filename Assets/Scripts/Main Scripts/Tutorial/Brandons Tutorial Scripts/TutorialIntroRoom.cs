using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialIntroRoom : MonoBehaviour
{
    [SerializeField] GameObject panelOne, panelTwo;
    [SerializeField] Material panelOneWhite, panelOneGreen;
    [SerializeField] float timeOnTarget;
    [SerializeField] bool panelOneBool, panelTwoBool;
    [SerializeField] StartingDoorAnimBrain doorToTrigger;
    [SerializeField] float elapsedTime = 0f;
    ObjectiveHelper helper;
    [SerializeField]bool ableToTrigger;

    RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {
        helper = FindObjectOfType<ObjectiveHelper>();
        // StartCoroutine(SetUpTrigger());
    }

    // Update is called once per frame
    void Update()
    {
        // if (!ableToTrigger) { return; }
        if(!panelOneBool || !panelTwoBool)
            GetRaycastFromPlayer();
    }

    private void GetRaycastFromPlayer()
    {

        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 20f))
        {
            if(hit.transform.gameObject.CompareTag("PanelOne") && !panelOneBool)
            {
                ProcessPanelOne();
            }
            else if (hit.transform.gameObject.CompareTag("PanelTwo") && !panelTwoBool)
            {
                ProcessPanelTwo();
            }
            else if (!panelOneBool)
            {
                panelOne.GetComponent<MeshRenderer>().material = panelOneWhite;
                elapsedTime = 0f;
            }
            else if (!panelTwoBool)
            {
                panelTwo.GetComponent<MeshRenderer>().material = panelOneWhite;
                elapsedTime = 0f;
            }
            else
            {
                elapsedTime = 0f;
            }
        }
        else
        {
            elapsedTime = 0f;
        }
    }

    private void ProcessPanelOne()
    {
        elapsedTime += Time.deltaTime;
        panelOne.GetComponent<MeshRenderer>().material.Lerp(panelOneWhite, panelOneGreen, elapsedTime / timeOnTarget);
        if(elapsedTime > timeOnTarget)
        {
            panelOneBool = true;
            helper.RemoveObjectiveByID(0);
            panelOne.GetComponent<MeshRenderer>().material = panelOneGreen;
            elapsedTime = 0f;
            if(panelTwoBool)
            {
                doorToTrigger.SendSecondMesage();
                // gameObject.SetActive(false);
            }
        }
    }

    private void ProcessPanelTwo()
    {
        elapsedTime += Time.deltaTime;
        panelTwo.GetComponent<MeshRenderer>().material.Lerp(panelOneWhite, panelOneGreen, elapsedTime / timeOnTarget);
        if (elapsedTime > timeOnTarget)
        {
            panelTwoBool = true;
            helper.RemoveObjectiveByID(1);
            panelTwo.GetComponent<MeshRenderer>().material = panelOneGreen;
            elapsedTime = 0f;
            if (panelOneBool)
            {
                doorToTrigger.SendSecondMesage();
                // gameObject.SetActive(false);
            }
        }
    }
    IEnumerator SetUpTrigger()
    {
        yield return new WaitForSeconds(13f);
        ableToTrigger = true;
    }
}
