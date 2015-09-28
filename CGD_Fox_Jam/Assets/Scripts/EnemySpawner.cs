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
        EnemyManager.GetInstance().MaxSpawnAmount = StartSpawnAmount;

        EnemyManager.GetInstance().player = player;

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
        //calc distance between enemy and spawn point
        float distance = EnemyManager.GetInstance().player.transform.position.x - this.transform.position.x;
       
        //if my SpawnCounter is out and the player is far away 
        if (SpawnCounter <= 0 && 
            (distance > PlayerToSpawnBuffer || distance < -PlayerToSpawnBuffer))
        {
            //and the amount of enemies spawned is less than the max spawn
            if (EnemyManager.GetInstance().ActiveEnemies != EnemyManager.GetInstance().MaxSpawnAmount)
            {
                //loop over all pooled farmers
                for (int i = 0; i < EnemyManager.GetInstance().SpawnedEnemies.Count; i++)
                {
                    //if the farmer is not active
                    if(EnemyManager.GetInstance().SpawnedEnemies[i].Value.activeInHierarchy == false)
                    {
                        //set position and active 
                        EnemyManager.GetInstance().SpawnedEnemies[i].Value.transform.position = 
                            new Vector2(this.transform.position.x, WorldGenerator.Instance.m_surfacePos + 0.75f);

                        EnemyManager.GetInstance().SpawnedEnemies[i].Value.SetActive(true);
                        
                        //increment active enemies
                        EnemyManager.GetInstance().ActiveEnemies++;

                        break;
                    }
                }
            }

            //reset counter
            SpawnCounter = SpawnRate;
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

        Vector2 spawn = new Vector2(this.transform.position.x, WorldGenerator.Instance.m_surfacePos + 0.75f);

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

        //turn of GameObject
        farmer.SetActive(false);
    }
}