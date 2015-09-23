using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager 
{
    private static EnemyManager instance;
    public static EnemyManager GetInstance()
    {
        if(instance == null)
        {
            instance = new EnemyManager();
        }

        return instance;
    }

    public List<GameObject> SpawnedEnemies = new List<GameObject>();
    public List<GameObject> SpawnedAnimals = new List<GameObject>();
    public int MaxSpawnAmount;

    /// <summary>
    /// Remove farmer at instance 
    /// </summary>
    public void RemoveSpawnedFarmer (int id)
    {
        //remove instance from the list
        SpawnedEnemies.RemoveAt(id);
    }

    /// <summary>
    /// Remove animal at instance 
    /// </summary>
    public void RemoveSpawnedAnimal (int id)
    {
        //remove instance from the list
        SpawnedAnimals.RemoveAt(id);
    }
}
