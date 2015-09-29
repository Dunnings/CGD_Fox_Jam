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

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        //make sure Inactive 
        if (InactiveAnimals <= 0)
        {
            InactiveAnimals = SpawnedAnimals.Count;
        }
    }

    /// <summary>
    /// Remove farmer at instance 
    /// </summary>
    public void RemoveSpawnedAnimal(int id)
    {
        //set animals at position inactive
        SpawnedAnimals[id].SetActive(false);

        CurrentSpawned--;
    }

}
