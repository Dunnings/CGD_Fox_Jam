using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimalManager : MonoBehaviour
{
    public static AnimalManager instance;

    public List<GameObject> SpawnedAnimals = new List<GameObject>();
    public int MaxSpawnAmount;

    public int CurrentSpawned = 0; 
    public int InactiveAnimals = 0;
    public int id = 0;

    void Start()
    {
        instance = this; 
    }

    /// <summary>
    /// Remove farmer at instance 
    /// </summary>
    public void RemoveSpawnedAnimal(int id)
    {
        //remove instance from the list
        for (int i = 0; i < SpawnedAnimals.Count; i++)
        {
            if (SpawnedAnimals[i].Key == id)
            {
                //set farmer at position inactive
                SpawnedAnimals[id].SetActive(false);

                CurrentSpawned--;
            }
        }
    }

}
