using UnityEngine;
using System.Collections;

public class Fox : MonoBehaviour {

    public float m_hunger = 100f;
    public float m_maxHunger = 100f;

    public float m_healthBarStartWidth;

    public GameObject m_healthBar;

    public GameObject m_breachParticle;
    
    bool hasBreached = false;
    bool hasSpawnedThisBreach = false;

    public Vector3 lastPos;

    public FoxCam m_camera;

	// Use this for initialization
	void Start () {
        m_healthBarStartWidth = m_healthBar.GetComponent<RectTransform>().sizeDelta.x;
        lastPos = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        m_healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(m_healthBarStartWidth * (m_hunger / m_maxHunger), 0f);

        if(transform.position.y > WorldGenerator.Instance.m_surfacePos)
        {
            hasBreached = true;
        }
        else if (hasBreached)
        {
            hasBreached = false;
            hasSpawnedThisBreach = false;
            m_camera.ShakeCamera();
        }

        if (hasBreached && !hasSpawnedThisBreach)
        {
            hasSpawnedThisBreach = true;
            GameObject x = Instantiate(m_breachParticle);
            Vector3 pos = transform.position;
            pos.y = WorldGenerator.Instance.m_surfacePos;
            x.transform.position = pos;

            Vector3 dir = transform.position - lastPos;
            dir.Normalize();

            m_breachParticle.GetComponent<BreachParticle>().m_smallDebris.transform.rotation = Quaternion.Euler(270f, 0f, Mathf.Acos(Vector3.Dot(Vector3.up, dir)));
            m_breachParticle.GetComponent<BreachParticle>().m_largeDebris.transform.rotation = Quaternion.Euler(270f, 0f, Mathf.Acos(Vector3.Dot(Vector3.up, dir)));
            
            m_camera.ShakeCamera();

            Destroy(x, 3f);
        }

        lastPos = transform.position;
	}


    
}
