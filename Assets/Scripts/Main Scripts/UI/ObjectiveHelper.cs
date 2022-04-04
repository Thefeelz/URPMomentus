using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ObjectiveHelper : MonoBehaviour
{
    [SerializeField] List<GameObject> objectiveText = new List<GameObject>();
    [SerializeField] List<Objective> objectives = new List<Objective>();
    // Start is called before the first frame update
    void Start()
    {
        RefreshDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddObjectivesToDisplay(List<Objective> _objectives)
    {
        foreach (Objective objective in _objectives)
        {
            objectives.Add(objective);
        }
        RefreshDisplay();
    }
    public void AddObjectivesToDisplay(Objective _objective)
    {   
        objectives.Add(_objective);      
        RefreshDisplay();
    }
    void RefreshDisplay()
    {
        int i = 0;
        foreach (Objective objective in objectives)
        {
            if(i >= objectiveText.Count) { break; }
            objectiveText[i].SetActive(true);
            objectiveText[i].GetComponent<TextMeshProUGUI>().text = objective.objectiveName;
            i++;
        }
        if(i < objectiveText.Count - 1)
        {
            
            for(int j = i;j < objectiveText.Count; j++)
            {
                objectiveText[j].GetComponent<TextMeshProUGUI>().text = "";
                objectiveText[j].SetActive(false);
            }
        }
    }

    public void RemoveObjectiveByID(int id)
    {
        foreach (Objective objective in objectives)
        {
            if(objective.objectiveID == id)
            {
                objectives.Remove(objective);
                RefreshDisplay();
                break;
            }
        }
    }
}
