using UnityEngine;
using System.Collections;

public class Fox : MonoBehaviour {

    public float m_hunger = 100f;
    public float m_maxHunger = 100f;

    public float m_healthBarStartWidth;

    public GameObject m_healthBar;

	// Use this for initialization
	void Start () {
        m_healthBarStartWidth = m_healthBar.GetComponent<RectTransform>().sizeDelta.x;
    }
	
	// Update is called once per frame
	void Update () {
        m_healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(m_healthBarStartWidth * (m_hunger / m_maxHunger), 0f);
	}
}
