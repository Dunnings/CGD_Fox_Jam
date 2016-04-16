using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    
    public List<GameObject> SpawnedEnemies = new List<GameObject>();
    public int MaxSpawnAmount;
    public int id = 0;
    public int InactiveEnemies = 0;
    public int CurrentSpawned = 0;
    public GameObject player; 

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        //make sure Inactive 
        if(InactiveEnemies <= 0)
        {
            InactiveEnemies = SpawnedEnemies.Count;
        }
    }

    /// <summary>
    /// Set farmer instance to inactive
    /// </summary>
    public void RemoveSpawnedFarmer (int id)
    {
        //set farmer at position inactive
        SpawnedEnemies[id].SetActive(false);

        CurrentSpawned--;
    }
}
