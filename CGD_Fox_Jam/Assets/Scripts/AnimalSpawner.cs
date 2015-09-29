using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimalSpawner : MonoBehaviour 
{
    //list stores all enemies
    public GameObject animal;
    public int SpawnRate;
    public int SpawnAmount;

    float SpawnCounter = 0;

    void Start()
    {
        AnimalManager.GetInstance().MaxSpawnAmount += SpawnAmount;
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
            if (AnimalManager.GetInstance().SpawnedAnimals.Count != AnimalManager.GetInstance().MaxSpawnAmount)
            {
                //spawn a farmer
                SpawnAnimal();
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
    void SpawnAnimal()
    {
        Vector2 spawn = new Vector2(Random.Range(0f - WorldGenerator.Instance.m_width / 2f, WorldGenerator.Instance.m_width / 2f), 
            WorldGenerator.Instance.m_surfacePos+0.25f);
        
        //create a new farmer
        GameObject newAnimal = Instantiate(animal, spawn, Quaternion.identity) as GameObject;

        //create key value pair instance 
        KeyValuePair<int, GameObject> instance = new KeyValuePair<int, GameObject>(AnimalManager.GetInstance().id, newAnimal);

        //temp store and increment the unique ID
        int myID = AnimalManager.GetInstance().id;
        AnimalManager.GetInstance().id++;

        //add animal to list in EnemyManager
        AnimalManager.GetInstance().SpawnedAnimals.Add(instance);

        //get the animal component and attach ID 
        newAnimal.GetComponent<Animal>().id = myID;

        //give animal a name 
        newAnimal.name = "Animal " + (AnimalManager.GetInstance().SpawnedAnimals.Count - 1).ToString();

        //set the animals parent to the spawner = nice hierarchy 
        newAnimal.transform.parent = this.transform;
    }
}
