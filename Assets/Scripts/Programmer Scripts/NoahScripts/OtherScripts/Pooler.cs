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
        [Tooltip("1 if bidedap, 2 if quadraped, 3 if ariel")]
        public int spawnType; // the type of enemy. Used for determining spawn location
        public GameObject prefab; // the prefab for this type
        public string dictionaryTag; // name of this type for dictionary reference
    }

    public List<ObjectPool> objectPools;
    public Dictionary<string, Queue<GameObject>> dictionaryPools;
    public Spawner spawner;
    public int totToSpawn; // how many are left to spawn in total
    public int spawned = 0;
    private int totOnField; // how many are on the field in total
    private Dictionary<string, int> onField; // a dictionary of how many of an individual type is on the field
    private Dictionary<string, int> minField; // minimum amount to exist on the field at a given time
    private Dictionary<string, int> spawnType; // type of enemy
    private List<string> tags; // a list of tags for referencing dictionarys in a cyclic order
    private int spot; // the spot of the despawned melee enemy
    public bool playerInRoom = false;


    // a dictionary of queues containing prefabs is created 
    // and all objects needed are created and stored in their respective queue
    void Start()
    {
        
        // make enemies not colide with each other
        //Physics.IgnoreLayerCollision(9, 9);
        //makes a dictionary of queues that hold different enemy types
        dictionaryPools = new Dictionary<string, Queue<GameObject>>();
        onField = new Dictionary<string, int>();
        minField = new Dictionary<string, int>();
        spawnType = new Dictionary<string, int>();
        tags = new List<string>();

        // for every objectPool in our list of object pools a queue of gameobjects is created and added to the dictionary of queues
        // using the variables of each objectPool to define the queue size and object type, and the dictionary tag
        foreach (ObjectPool objectPool in objectPools)
        {
            Queue<GameObject> poolQueue = new Queue<GameObject>();
            for (int i = 0; i < objectPool.size; i++)
            {
                GameObject obj = Instantiate(objectPool.prefab);
                obj.name = "Enemy" + i;
                obj.SetActive(false);
                obj.transform.parent = this.transform;
                obj.GetComponent<Entity>().myPool = this;
                //obj.transform.parent = this.gameObject.transform;
                poolQueue.Enqueue(obj);
            }
            //adds object pool to all the dictionaries that need it
            dictionaryPools.Add(objectPool.dictionaryTag, poolQueue);
            onField.Add(objectPool.dictionaryTag, 0);
            minField.Add(objectPool.dictionaryTag, objectPool.minField);
            spawnType.Add(objectPool.dictionaryTag, objectPool.spawnType);
            tags.Add(objectPool.dictionaryTag);
        }
    }

    //enques the passed gameobject in the queue in the dictionary that matches the passed tag
    public void queueObject(string tag, GameObject obj)
    {
        dictionaryPools[tag].Enqueue(obj);
        obj.SetActive(false);
        onField[tag] -= 1;
    }
    //dequeues the gameobject from the queue with the matching tag. No need to increase the dictionary of amount on field as that is performed
    // in update
    public GameObject dequeueObject(string tag)
    {
        GameObject objToSpawn = dictionaryPools[tag].Dequeue();
        if(tag == "Melee")
        {
            objToSpawn.GetComponent<Enemy_Melee>().stateMachine.ChangeState(objToSpawn.GetComponent<Enemy_Melee>().moveState);
            
        }
        return objToSpawn; 
    }
    /// <summary>
    /// checks each pool for if there is enough of that type of enemy on the field. If not it deques until there is
    /// </summary>
    public void Update()
    {
        //Debug.Log(tags);
        
        foreach (string tag in tags)
        {
            if(onField[tag] < minField[tag] && spawned < totToSpawn)
            {

                
                spawned += 1;
                onField[tag] += 1;
                GameObject enemy = dequeueObject(tag);
                spawner.spawn(enemy, spawnType[tag]);
                
                
            }
        }
    }

    public void spawn()
    {

    }


}
