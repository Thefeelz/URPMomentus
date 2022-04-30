using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationRefresh : MonoBehaviour
{
    private Locations holder;
    public Pooler roomPool;
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
        if (other.gameObject.layer == 6) // 6 should be player
        {
            holder.restartSpots();
            roomPool.playerInRoom = true;
        }

    }
}
