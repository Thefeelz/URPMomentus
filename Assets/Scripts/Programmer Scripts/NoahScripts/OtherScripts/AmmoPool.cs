using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPool : MonoBehaviour
{
    
    public GameObject bullet; // prefab for bullet
    public int size; // size of queue
    public Queue<GameObject> bulletPool; //queue to store bullets

    
    void Start()
    {
        //instantiates size number of bullets and places them in the queue after deactivating them
        bulletPool = new Queue<GameObject>();
        for(int i = 0; i < size; i++)
        {
            GameObject nBullet = Instantiate(bullet);

            nBullet.SetActive(false);
            bulletPool.Enqueue(nBullet);
        }
    }
    // when a bullet is needed it is pulled from the front of the queue
    public GameObject dequeBullet()
    {
        return bulletPool.Dequeue();
    }
    //when the bullet is finished it is put into the back of the queue
    public void enqueBullet(GameObject pBullet)
    {
        bulletPool.Enqueue(pBullet);
    }


}
