using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class Fox : MonoBehaviour {
    
    public float m_hunger = 100f;
    public float m_maxHunger = 100f;

    public float m_healthBarStartWidth;

    public GameObject m_healthBar;
    public Image m_healthBarBack;

    public GameObject m_breachParticle;
    public GameObject m_enterParticle;

    bool hasBreached = false;
    bool hasSpawnedThisBreach = false;
    

    public Vector3 lastPos;

    public FoxCam m_camera;

    public GameObject menu;
    
    // Use this for initialization
    void Start ()
    {
        m_healthBarStartWidth = m_healthBar.GetComponent<RectTransform>().sizeDelta.x;
        lastPos = transform.position;
    }
	
	// Update is called once per frame
	void Update () {

        if (!menu.activeSelf)
        {
            DecreaseHunger(3f * Time.deltaTime);
        }
        m_healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(m_healthBarStartWidth * (m_hunger / m_maxHunger), 0f);

        if(transform.position.y > WorldGenerator.Instance.m_surfacePos)
        {
            hasBreached = true;
        }
        else if (hasBreached)
        {
            hasBreached = false;
            hasSpawnedThisBreach = false;

            PlayEnterEffect();
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

            x.GetComponent<BreachParticle>().m_smallDebris.transform.rotation = Quaternion.Euler(270f, 0f, Mathf.Acos(Vector3.Dot(Vector3.up, dir)));
            x.GetComponent<BreachParticle>().m_largeDebris.transform.rotation = Quaternion.Euler(270f, 0f, Mathf.Acos(Vector3.Dot(Vector3.up, dir)));
            
            m_camera.ShakeCamera();

            Destroy(x, 10f);
        }

        lastPos = transform.position;

        if (gameObject.GetComponent<FoxMovement>().GetVel().x > 0)
        {
            gameObject.transform.localScale = new Vector3(0.2f, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
        }
        else if (gameObject.GetComponent<FoxMovement>().GetVel().x < 0)
        {
            gameObject.transform.localScale = new Vector3(-0.2f, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
        }
    }

    void LateUpdate()
    {
        if(transform.position.y < WorldGenerator.Instance.m_surfacePos - WorldGenerator.Instance.m_depth)
        {
            GetComponent<FoxMovement>().BounceBottom();
        }
        if (transform.position.x > WorldGenerator.Instance.m_width / 2f)
        {
            GetComponent<FoxMovement>().BounceSide();
        }
        else if (transform.position.x < -(WorldGenerator.Instance.m_width / 2f))
        {
            GetComponent<FoxMovement>().BounceSide();
        }
    }

    private void PlayEnterEffect()
    {
        GameObject x = Instantiate(m_enterParticle);
        Vector3 pos = transform.position;
        x.transform.position = pos;

        Vector3 dir = transform.position - lastPos;
        dir.Normalize();

        x.GetComponent<BreachParticle>().m_smallDebris.transform.rotation = Quaternion.Euler(270f, 0f, Mathf.Acos(Vector3.Dot(Vector3.up, dir)));
        x.GetComponent<BreachParticle>().m_largeDebris.transform.rotation = Quaternion.Euler(270f, 0f, Mathf.Acos(Vector3.Dot(Vector3.up, dir)));

        m_camera.ShakeCamera();

        Destroy(x, 6f);
    }

    public Color m_col1;
    public Color m_col2;
    public Color m_col3;

    public void DecreaseHunger(float amount)
    {
        m_hunger -= amount;

        if(m_hunger <= 0)
        {

            //GAME OVER
            return;
        }


        m_healthBar.transform.localScale = new Vector3(m_hunger / m_maxHunger, 1f, 1f);
        if (m_hunger / m_maxHunger > 0.6f)
        {
            m_healthBarBack.color = m_col1;
        }
        else if (m_hunger / m_maxHunger > 0.2f)
        {
            m_healthBarBack.color = m_col2;
        }
        else
        {
            m_healthBarBack.color = m_col3;
        }
    }

    public void SmallReward()
    {
        float rewardAmount = 5f;
        m_hunger = Mathf.Min(m_maxHunger, m_hunger + rewardAmount);
    }
    public void Reward()
    {
        float rewardAmount = 10f;
        m_hunger = Mathf.Min(m_maxHunger, m_hunger + rewardAmount);
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.GetComponent<BulletProperties>() != null)
        {
            m_hunger = m_hunger - col.gameObject.GetComponent<BulletProperties>().damage;
        }
    }
}
