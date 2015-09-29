using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
    public static GameManager instance;

    public int KillCount = 0;

	void Awake () 
    {
        instance = this;
	}
	
	void Update () 
    {
        //for every 10 kills double enemy spawns and lessen animals
        if(KillCount%5.0f == 0.0f && KillCount > 0)
        {
            EnemySpawner.instance.UpdateSpawning();
        }
	}
}
