using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour 
{
    public GameObject menu;

    public Text timer;
    public Text kills;
    public static GameManager instance;

    public int KillCount = 0;

    public float timeElapsed = 0f;

	void Awake () 
    {
        instance = this;
	}
	
	void Update () 
    {
        if (!menu.activeSelf)
        {
            timeElapsed += Time.deltaTime;
            timer.text = timeElapsed.ToString("0:00");
            //for every 10 kills double enemy spawns and lessen animals
            if(KillCount%5.0f == 0.0f && KillCount > 0)
            {
                EnemySpawner.instance.UpdateSpawning();
            }
            kills.text = KillCount + " kills";
        }
    }
}
