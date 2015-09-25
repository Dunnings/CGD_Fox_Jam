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

    public List<KeyValuePair<int, GameObject>> SpawnedEnemies = new List<KeyValuePair<int, GameObject>>();
    public int MaxSpawnAmount;
    public int id = 0;
    public GameObject player; 

    /// <summary>
    /// Remove farmer at instance 
    /// </summary>
    public void RemoveSpawnedFarmer (int id)
    {
        //remove instance from the list
        for (int i = 0; i < SpawnedEnemies.Count; i++)
        {
            if(SpawnedEnemies[i].Key == id)
            {
                SpawnedEnemies.RemoveAt(i);
                break;
            }
        }
    }
}
