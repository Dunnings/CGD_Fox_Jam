using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimalSpawner : MonoBehaviour 
{
    //list stores all enemies
    public GameObject player; 
    public GameObject animal;
    public float SpawnRate;
    public int SpawnAmount;

    float SpawnCounter = 0;

    void Start()
    {
        AnimalManager.instance.MaxSpawnAmount += SpawnAmount;
            
        for (int i = 0; i < SpawnAmount; i++)
        {
            SpawnAnimal();
        }
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
            //and the amount of enemies spawned is less than the max spawn and the enemy is not already active
            if (AnimalManager.instance.CurrentSpawned < AnimalManager.instance.MaxSpawnAmount &&
                 AnimalManager.instance.SpawnedAnimals[AnimalManager.instance.InactiveAnimals - 1].activeInHierarchy == false)
            {
                //Vector2 spawn;
                //if (player.transform.position.x < 0.0f)
                //{
                //     spawn = new Vector2(Random.Range(10.0f, WorldGenerator.Instance.m_width / 2f),
                //        WorldGenerator.Instance.m_surfacePos + 0.25f);
                //}
                //else
                //{
                //    spawn = new Vector2(Random.Range(0 - WorldGenerator.Instance.m_width / 2f, -10.0f),
                //        WorldGenerator.Instance.m_surfacePos + 0.25f);
                //}


                //AnimalManager.instance.SpawnedAnimals[AnimalManager.instance.InactiveAnimals - 1].transform.position = spawn;

                //re randomize enemy
                AnimalManager.instance.SpawnedAnimals[AnimalManager.instance.InactiveAnimals - 1].GetComponent<Animal>().Init();

                AnimalManager.instance.SpawnedAnimals[AnimalManager.instance.InactiveAnimals - 1].SetActive(true);

                //Decrement active enemies
                AnimalManager.instance.InactiveAnimals--;

                AnimalManager.instance.CurrentSpawned++;

                //reset counter
                SpawnCounter = SpawnRate;
            }
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
    void SpawnAnimal()
    {
        Vector2 spawn = new Vector2(Random.Range(0f - WorldGenerator.Instance.m_width / 2f, WorldGenerator.Instance.m_width / 2f), 
            WorldGenerator.Instance.m_surfacePos+0.3f);
        
        //create a new farmer
        GameObject newAnimal = Instantiate(animal, spawn, Quaternion.identity) as GameObject;

        //temp store and increment the unique ID
        int myID = AnimalManager.instance.id;
        AnimalManager.instance.id++;

        //add animal to list in EnemyManager
        AnimalManager.instance.SpawnedAnimals.Add(newAnimal);

        //increment amount of Inactive enemies
        AnimalManager.instance.InactiveAnimals = AnimalManager.instance.SpawnedAnimals.Count;

        //get the animal component and attach ID 
        newAnimal.GetComponent<Animal>().id = myID;

        //give animal a name 
        newAnimal.name = "Animal " + (AnimalManager.instance.SpawnedAnimals.Count - 1).ToString();

        //set the animals parent to the spawner = nice hierarchy 
        newAnimal.transform.parent = AnimalManager.instance.transform;

        newAnimal.gameObject.SetActive(false);
    }
}
