using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimalManager
{
    private static AnimalManager instance;
    public static AnimalManager GetInstance()
    {
        if(instance == null)
        {
            instance = new AnimalManager();
        }

        return instance;
    }

    public List<KeyValuePair<int, GameObject>> SpawnedAnimals = new List<KeyValuePair<int, GameObject>>();
    public int MaxSpawnAmount;
    public int id = 0;

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
                SpawnedAnimals.RemoveAt(i);
                break;
            }
        }
    }

}
