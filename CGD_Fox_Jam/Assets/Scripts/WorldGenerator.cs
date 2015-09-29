using UnityEngine;
using System.Collections.Generic;

public class WorldGenerator : MonoBehaviour {

    public static WorldGenerator Instance;

    public GameObject m_bedrockPrefab;
    public GameObject m_cloud;
    float m_timeLastSpawnedCloud;
    float m_cloudTimeInterval = 3f;

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
    List<GameObject> m_pooledClouds = new List<GameObject>();

    public Color m_dirtColor = Color.white;

    public bool m_respawnRocks = false;
    private float m_timeAtLastRockSpawn = 0f;
    private float m_rockDelaySpawn = 3f;

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
        m_respawnRocks = true;

        for (int i = 0; i < 40; i++)
        {
            GameObject newCloud = Instantiate(m_cloud);
            m_timeLastSpawnedCloud = Time.time;
            m_cloudTimeInterval = Random.Range(2f, 4f);
            newCloud.transform.position = new Vector3(Random.Range(-(WorldGenerator.Instance.m_width / 2f), WorldGenerator.Instance.m_width / 2f), Random.Range(WorldGenerator.Instance.m_surfacePos + 3f, WorldGenerator.Instance.m_surfacePos + 5f), 0f);
            m_pooledClouds.Add(newCloud);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_respawnRocks && m_allRocks.Count < m_rockCount)
        {
            if (Time.time - m_timeAtLastRockSpawn > m_rockDelaySpawn)
            {
                SpawnRandomRock();
            }
        }

        if (Time.time - m_timeLastSpawnedCloud > m_cloudTimeInterval)
        {
            GameObject newCloud = GetNextCloud();
            if (newCloud != null)
            {
                newCloud.SetActive(true);
                m_timeLastSpawnedCloud = Time.time;
                m_cloudTimeInterval = Random.Range(2f, 4f);
                newCloud.transform.position = new Vector3(WorldGenerator.Instance.m_width / 2f, Random.Range(WorldGenerator.Instance.m_surfacePos + 3f, WorldGenerator.Instance.m_surfacePos + 5f), 0f);
            }
        }
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
        m_timeAtLastRockSpawn = Time.time;
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

    public GameObject GetNextCloud()
    {
        for (int i = 0; i < m_pooledClouds.Count; i++)
        {
            if (!m_pooledClouds[i].activeSelf) {
                return m_pooledClouds[i];
            }
        }
        return null;
    }
}
