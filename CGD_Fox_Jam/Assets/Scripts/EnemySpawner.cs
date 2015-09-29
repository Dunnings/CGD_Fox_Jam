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
    public float PlayerToSpawnBuffer;

    float SpawnCounter = 0;      

    void Start()
    {
        EnemyManager.instance.MaxSpawnAmount += StartSpawnAmount;

        EnemyManager.instance.player = player;

        for (int i = 0; i < StartSpawnAmount; i++)
        {
            SpawnFarmer();
        }
    }

    /// <summary>
    /// called every frame
    /// </summary>
    void Update()
    {
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
        GameObject farmer = Instantiate(farmers[0], spawn, Quaternion.identity) as GameObject;
        
        //temp store and increment the unique ID
        int myID = EnemyManager.instance.id;
        EnemyManager.instance.id++;

        //add farmer to list in EnemyManager
        EnemyManager.instance.SpawnedEnemies.Add(farmer);

        //increment amount of Inactive enemies
        EnemyManager.instance.InactiveEnemies = EnemyManager.instance.SpawnedEnemies.Count;

        //get the enemy component and attach ID 
        farmer.GetComponent<Enemy>().id = myID;

        //give farmer a name 
        farmer.name = "Farmer " + (EnemyManager.instance.SpawnedEnemies.Count - 1).ToString();

        //set the farmers parent to the spawner = nice hierarchy 
        farmer.transform.parent = EnemyManager.instance.gameObject.transform;

        //turn of GameObject
        farmer.SetActive(false);
    }
}