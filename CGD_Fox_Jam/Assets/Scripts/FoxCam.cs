using UnityEngine;
using System.Collections;

public class FoxCam : MonoBehaviour {

    public GameObject m_fox;
    public GameObject m_parent;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 newPos = m_fox.transform.position;
        newPos.z = -10f;
        gameObject.transform.position = newPos;
    }

    public void ShakeCamera()
    {
        iTween.ShakePosition(m_parent, iTween.Hash("x", 0.1f, "y", 0.1f, "isLocal", false, "time", 0.6f));
    }

}
