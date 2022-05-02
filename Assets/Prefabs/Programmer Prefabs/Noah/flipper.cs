using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flipper : MonoBehaviour
{
    public GameObject pool1;
    public GameObject pool2;
    public bool active1;
    public bool active2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            pool1.SetActive(false);
            pool2.SetActive(true);
        }
    }
}
