using UnityEngine;
using System.Collections.Generic;

public class WorldGenerator : MonoBehaviour {

    public static WorldGenerator Instance;

    public GameObject m_bedrockPrefab;

    public GameObject m_rockPrefab;
    public List<Sprite> m_rockSpriteList;

    public SpriteRenderer m_background;
    public SpriteRenderer m_surfaceCover;

    public GameObject m_surfacePrefab;
    public Sprite m_surfaceSprite;

    public GameObject m_wheatPrefab;
    public Sprite m_wheatSprite;

    public float m_tileWidth = 0.7f;
    public float m_surfacePos = 2.7f;
    public float m_depth = 50f;
    public float m_width = 300f;
    public int m_rockCount = 100;
    public int m_wheatCount = 50;

    private List<GameObject> m_allSurfaceTiles = new List<GameObject>();
    private List<GameObject> m_allRocks = new List<GameObject>();
    private List<GameObject> m_allWheat = new List<GameObject>();

    public Color m_dirtColor = Color.white;

    void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start ()
    {
        int startNo = (int)(0f - ((m_width/m_tileWidth) / 2f));
        for (float i = startNo; i < -startNo; i += m_tileWidth)
        {
            GameObject newObj = Instantiate<GameObject>(m_surfacePrefab);
            newObj.GetComponent<SpriteRenderer>().sprite = m_surfaceSprite;
            newObj.transform.position = new Vector3(i, m_surfacePos, 0f);
            newObj.gameObject.transform.SetParent(gameObject.transform);
            m_allSurfaceTiles.Add(newObj);
        }

        //Bottom
        for (int x = 1; x < 40; x++)
        {
            for (float i = startNo; i < -startNo; i += m_tileWidth)
            {
                GameObject newObj = Instantiate<GameObject>(m_bedrockPrefab);
                newObj.transform.position = new Vector3(i, m_surfacePos - m_depth - (x * m_tileWidth), 0f);
                newObj.gameObject.transform.SetParent(gameObject.transform);
            }
        }

        //Right
        for (int x = -40; x < 30; x++)
        {
            for (int i = 1; i < 30; i++)
            {
                GameObject newObj = Instantiate<GameObject>(m_bedrockPrefab);
                newObj.transform.position = new Vector3((m_width / 2) + i * m_tileWidth, m_surfacePos - (x * m_tileWidth), 0f);
                newObj.gameObject.transform.SetParent(gameObject.transform);
            }
        }

        //Left
        for (int x = -40; x < 30; x++)
        {
            for (int i = 1; i < 30; i++)
            {
                GameObject newObj = Instantiate<GameObject>(m_bedrockPrefab);
                newObj.transform.position = new Vector3(-(m_width / 2) - i * m_tileWidth, m_surfacePos - (x * m_tileWidth), 0f);
                newObj.gameObject.transform.SetParent(gameObject.transform);
            }
        }


        m_dirtColor = m_surfaceSprite.texture.GetPixel(0, 0);

        m_background.transform.position = new Vector3(0f, m_surfacePos, 0f);
        m_background.color = m_dirtColor;

        m_surfaceCover.transform.position = new Vector3(0f, m_surfacePos, 0f);
        m_surfaceCover.color = Camera.main.backgroundColor;

        for (int i = 0; i < m_rockCount; i++)
        {
            SpawnRandomRock();
        }
        //for (int i = 0; i < m_wheatCount; i++)
        //{
        //    SpawnRandomWheat((0f - (m_width/2f)) + ((m_width / m_wheatCount) * i));
        //}
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    private void SpawnRandomRock()
    {
        GameObject newRock = GameObject.Instantiate<GameObject>(m_rockPrefab);
        newRock.GetComponent<SpriteRenderer>().sprite = m_rockSpriteList[Random.Range(0, m_rockSpriteList.Count - 1)];
        newRock.GetComponent<SpriteRenderer>().sortingOrder = 50;
        Vector3 newPos = new Vector3(Random.Range(0f - m_width / 2f, m_width / 2f), m_surfacePos - 1f - Random.Range(0f, m_depth), 0f);
        newRock.transform.position = newPos;
        newRock.transform.SetParent(gameObject.transform);
        m_allRocks.Add(newRock);
    }

    public void SpawnRandomWheat(float xPos)
    {
        GameObject newWheat = GameObject.Instantiate<GameObject>(m_wheatPrefab);
        newWheat.GetComponent<SpriteRenderer>().sprite = m_wheatSprite;
       
        xPos = Random.Range(0f - m_width / 2f, m_width / 2f);

        Vector3 newPos = new Vector3(xPos, m_surfacePos, 0f);
        newWheat.transform.position = newPos;
        newWheat.transform.SetParent(gameObject.transform);
        m_allWheat.Add(newWheat);
    }
}
