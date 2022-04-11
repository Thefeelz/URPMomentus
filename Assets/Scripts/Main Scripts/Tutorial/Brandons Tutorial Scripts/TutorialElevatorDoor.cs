using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialElevatorDoor : MonoBehaviour
{
    [SerializeField]float totalTime, elapsedTime;
    [SerializeField]bool turnOnDoor = false;
    [SerializeField] Material thisMat;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (turnOnDoor)
            TurnOnDoor();
    }

    void TurnOnDoor()
    {
        elapsedTime += Time.deltaTime;
        thisMat.SetFloat("Vector1_0603949f4ad84042a119dece73d418be", Mathf.Lerp(.1f, 0, elapsedTime / totalTime));
        if(elapsedTime >= totalTime)
        {
            turnOnDoor = false;
            elapsedTime = 0;
        }
        
    }
    public void TurnOnDoorFromBrain(float totalTime, Material startingMat)
    {
        thisMat = GetComponent<MeshRenderer>().material;

        this.totalTime = totalTime;
        turnOnDoor = true;
        
    }
    public void TurnOnDoorFromBrainLame(Material endingMat)
    {
        GetComponent<MeshRenderer>().material = endingMat;
        
    }
}
