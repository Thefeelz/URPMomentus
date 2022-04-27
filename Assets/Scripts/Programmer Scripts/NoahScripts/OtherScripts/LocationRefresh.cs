using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationRefresh : MonoBehaviour
{
    private Locations holder;
    // Start is called before the first frame update
    void Start()
    {
        holder = GameObject.Find("SpotHolder").GetComponent<Locations>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "PlayerV2")
        {
            holder.restartSpots();
        }

    }
}
