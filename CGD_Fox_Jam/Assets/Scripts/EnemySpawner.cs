using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance;
    //list stores all enemies
    public GameObject farmer;
    public GameObject player;
    public float SpawnRate;
    public int StartSpawnAmount;
    public float PlayerToSpawnBuffer;

    float SpawnCounter = 0;   
   
    void Awake ()
    {
        instance = this;
    }

    void Start()
    {
        EnemyManager.instance.MaxSpawnAmount += StartSpawnAmount;

        EnemyManager.instance.player = player;

        for (int i = 0; i < 100; i++)
        {
            SpawnFarmer();
        }
    }

    /// <summary>
    /// called every frame
    /// </summary>
    void Update()
    {
        if(EnemyManager.instance.MaxSpawnAmount == 100)
        {
            EnemyManager.instance.MaxSpawnAmount = 100;
        }

        //calc distance between enemy and spawn point
        float distance = EnemyManager.instance.player.transform.position.x - this.transform.position.x;
       
        //if my SpawnCounter is out and the player is far away 
        if (SpawnCounter <= 0 && 
            (distance > PlayerToSpawnBuffer || distance < -PlayerToSpawnBuffer))
        {
            //and the amount of enemies spawned is less than the max spawn and the enemy is not already active
            if (EnemyManager.instance.CurrentSpawned < EnemyManager.instance.MaxSpawnAmount &&
                 EnemyManager.instance.SpawnedEnemies[EnemyManager.instance.InactiveEnemies - 1].activeInHierarchy == false)
            {
                //set position and active 
                EnemyManager.instance.SpawnedEnemies[EnemyManager.instance.InactiveEnemies-1].transform.position = 
                    new Vector2(this.transform.position.x, WorldGenerator.Instance.m_surfacePos + 0.75f);

                //re randomize enemy
                EnemyManager.instance.SpawnedEnemies[EnemyManager.instance.InactiveEnemies-1].GetComponent<Enemy>().Init();

                EnemyManager.instance.SpawnedEnemies[EnemyManager.instance.InactiveEnemies-1].SetActive(true);
                        
                //Decrement active enemies
                EnemyManager.instance.InactiveEnemies--;

                EnemyManager.instance.CurrentSpawned++;

            }

            //reset counter
            
            SpawnCounter = SpawnRate;
        }
        //else decrement the counter 
        else
        {
            
            SpawnCounter -= Time.deltaTime;
        }

        
    }

    /// <summary>
    /// Spawns a framer at the instance of the spawner object
    /// </summary>
    void SpawnFarmer()
    {
        //generate spawn point
        Vector2 spawn = new Vector2(this.transform.position.x, WorldGenerator.Instance.m_surfacePos + 0.75f);

        //create a new farmer
        GameObject newFarmer = Instantiate(farmer, spawn, Quaternion.identity) as GameObject;
        
        //temp store and increment the unique ID
        int myID = EnemyManager.instance.id;
        EnemyManager.instance.id++;

        //add farmer to list in EnemyManager
        EnemyManager.instance.SpawnedEnemies.Add(newFarmer);

        //increment amount of Inactive enemies
        EnemyManager.instance.InactiveEnemies = EnemyManager.instance.SpawnedEnemies.Count;

        //get the enemy component and attach ID 
        newFarmer.GetComponent<Enemy>().id = myID;

        //give farmer a name 
        newFarmer.name = "Farmer " + (EnemyManager.instance.SpawnedEnemies.Count - 1).ToString();

        //set the farmers parent to the spawner = nice hierarchy 
        newFarmer.transform.parent = EnemyManager.instance.gameObject.transform;

        //turn of GameObject
        newFarmer.SetActive(false);
    }

    public void UpdateSpawning()
    {
        //take 10 seconds off spawn time
        if (SpawnRate > 2.0f)
        {
            SpawnRate = SpawnRate - 1.0f;
            if (SpawnRate < 2.0f)
            {
                SpawnRate = 2.0f;
            }
        }

        //double amount of players to spawn
        if(StartSpawnAmount < 100)
        {
            
            StartSpawnAmount = + 5;
            EnemyManager.instance.MaxSpawnAmount += StartSpawnAmount;

            if(StartSpawnAmount > 100)
            {
                StartSpawnAmount = 100;
                EnemyManager.instance.MaxSpawnAmount = StartSpawnAmount;
            }
        }

    }
}