using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pooler : MonoBehaviour
{
    // Object pool is an abstract class that holds values allowing us to define different enemy pools
    [System.Serializable]
    public class ObjectPool
    {
        public int size; // how many of this type exist 
        public int minField; // minimum amount that must exist on the field
        public GameObject prefab; // the prefab for this type
        public string dictionaryTag; // name of this type for dictionary reference
    }

    public List<ObjectPool> objectPools;
    public Dictionary<string, Queue<GameObject>> dictionaryPools;
    private int totToSpawn; // how many are left to spawn in total
    private int totOnField; // how many are on the field in total
    private Dictionary<string, int> onField; // a dictionary of how many of an individual type is on the field
    private Dictionary<string, int> minField; // minimum amount to exist on the field at a given time



    // a dictionary of queues containing prefabs is created 
    // and all objects needed are created and stored in their respective queue
    void Start()
    {
        // make enemies not colide with each other
        Physics.IgnoreLayerCollision(9, 9);
        //makes a dictionary of queues that hold different enemy types
        dictionaryPools = new Dictionary<string, Queue<GameObject>>();
        onField = new Dictionary<string, int>();
        minField = new Dictionary<string, int>();

        // for every objectPool in our list of object pools a queue of gameobjects is created and added to the dictionary of queues
        // using the variables of each objectPool to define the queue size and object type, and the dictionary tag
        foreach (ObjectPool objectPool in objectPools)
        {
            Queue<GameObject> poolQueue = new Queue<GameObject>();

            for (int i = 0; i < objectPool.size; i++)
            {
                GameObject obj = Instantiate(objectPool.prefab);
                obj.SetActive(false);
                obj.GetComponent<Entity>().myPool = this;
                obj.transform.parent = this.gameObject.transform;
                poolQueue.Enqueue(obj);
            }

            dictionaryPools.Add(objectPool.dictionaryTag, poolQueue);
            onField.Add(objectPool.dictionaryTag, 0);
            minField.Add(objectPool.dictionaryTag, objectPool.minField);
        }
    }

    //enques the passed gameobject in the queue in the dictionary that matches the passed tag
    public void queueObject(string tag, GameObject obj)
    {
        dictionaryPools[tag].Enqueue(obj);
        obj.SetActive(false);
        onField[tag] -= 1;
    }

    //dequeues the gameobject from the queue with the matching tag
    void dequeueObject(string tag)
    {
        GameObject objToSpawn = dictionaryPools[tag].Dequeue();
        objToSpawn.SetActive(true);
        onField[tag] += 1;
    }

    public void Update()
    {
        
    }

    public void spawn()
    {

    }


}