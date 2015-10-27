using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour 
{
    public GameObject menu;

    public Text timer;
    public Text kills;
    public static GameManager instance;

    public List<GameObject> m_toEnable = new List<GameObject>();

    public int KillCount = 0;

    public float timeElapsed = 0f;

    bool hasBeenEnabled = false;

	void Awake () 
    {
        instance = this;
        
	}

    void Start()
    {
        for (int i = 0; i < m_toEnable.Count; i++)
        {
            m_toEnable[i].gameObject.SetActive(false);
        }
    }
	
	void Update () 
    {
        if (!menu.activeSelf)
        {
            EnableAll();
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

    private void EnableAll()
    {
        if (!hasBeenEnabled)
        {
            for (int i = 0; i < m_toEnable.Count; i++)
            {
                m_toEnable[i].gameObject.SetActive(true);
            }
            hasBeenEnabled = true;
        }
    }
}
