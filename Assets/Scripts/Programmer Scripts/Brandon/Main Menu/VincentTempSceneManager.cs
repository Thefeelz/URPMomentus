using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VincentTempSceneManager : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponentInParent<CharacterStats>())
        {
            FindObjectOfType<SceneController>().MainMenu();
        }
    }
}
