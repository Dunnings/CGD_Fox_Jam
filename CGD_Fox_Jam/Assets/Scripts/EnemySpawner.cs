using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    //list stores all enemies
    public List<GameObject> farmers = new List<GameObject>();
    public GameObject player;
    public int SpawnRate;
    public int StartSpawnAmount;

    float SpawnCounter = 0;

    void Start()
    {
        EnemyManager.GetInstance().MaxSpawnAmount = StartSpawnAmount;

        EnemyManager.GetInstance().player = player;
    }

    /// <summary>
    /// called every frame
    /// </summary>
    void Update()
    {
        //if my SpawnCounter is out
        if (SpawnCounter <= 0)
        {
            //and the amount of enemies spawned is less than the max spawn
            if (EnemyManager.GetInstance().SpawnedEnemies.Count != EnemyManager.GetInstance().MaxSpawnAmount)
            {
                //spawn a farmer
                SpawnFarmer();
                //reset counter
                SpawnCounter = SpawnRate;
            }
        }
        //else decrement the counter 
        else
        {
            SpawnCounter -= Time.deltaTime;
            //Debug.Log(SpawnCounter);
        }
    }

    /// <summary>
    /// Spawns a framer at the instance of the spawner object
    /// </summary>
    void SpawnFarmer()
    {
        //get a random index of farmers
        int index = Random.Range(0, farmers.Count);

        Vector2 spawn = new Vector2(this.transform.position.x, WorldGenerator.Instance.m_surfacePos);

        //create a new farmer
        GameObject farmer = Instantiate(farmers[index], spawn, Quaternion.identity) as GameObject;

        //create key value pair instance 
        KeyValuePair<int, GameObject> instance = new KeyValuePair<int, GameObject>(EnemyManager.GetInstance().id, farmer);

        //temp store and increment the unique ID
        int myID = EnemyManager.GetInstance().id;
        EnemyManager.GetInstance().id++;

        //add farmer to list in EnemyManager
        EnemyManager.GetInstance().SpawnedEnemies.Add(instance);

        //get the enemy component and attach ID 
        farmer.GetComponent<Enemy>().id = myID;

        //give farmer a name 
        farmer.name = "Farmer " + (EnemyManager.GetInstance().SpawnedEnemies.Count - 1).ToString();

        //set the farmers parent to the spawner = nice hierarchy 
        farmer.transform.parent = this.transform;
    }
}