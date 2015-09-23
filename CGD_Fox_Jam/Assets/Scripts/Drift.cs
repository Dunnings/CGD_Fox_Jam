using UnityEngine;
using System.Collections;

public class Drift : MonoBehaviour {

    public float m_speed = 1f;

	// Use this for initialization
	void Start () {
        m_speed = Random.Range(m_speed - 0.1f, m_speed + 0.2f);
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = transform.position += new Vector3(-m_speed * Time.deltaTime, 0f, 0f);
	}
}
