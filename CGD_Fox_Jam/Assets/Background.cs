using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour {

    Vector3 startPos;
    Vector3 camStartPos;

    public float m_mod = 0.1f;

	// Use this for initialization
	void Start () {
        startPos = transform.position;
        camStartPos = Camera.main.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        float xPos = camStartPos.x - Camera.main.transform.position.x;
        xPos *= m_mod;
        transform.position = new Vector3(xPos, startPos.y, startPos.z);
	}
}
