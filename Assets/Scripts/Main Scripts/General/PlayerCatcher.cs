using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatcher : MonoBehaviour
{
    [SerializeField] bool destroyOnTrigger = false;
    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponentInParent<CharacterStats>())
        {
            if(gameManager)
            {
                gameManager.SendGameObjectToRespawn(other.GetComponentInParent<CharacterStats>().gameObject);
            }
            else
            {
                Debug.LogError("There is no Game Manager in the Scene, make sure to place a Game Manager in the Scene.");
            }
            if(destroyOnTrigger)
            {
                Destroy(this);
            }
        }
    }
}
