using UnityEngine;
using System.Collections;

public class DeathParticleSystem : MonoBehaviour {

    public ParticleSystem m_blood1;
    public ParticleSystem m_blood2;

    public GameObject m_surfaceCollider;

    // Use this for initialization
    void Start ()
    {
        PlayBreach();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void PlayBreach()
    {
        m_blood1.Play();
        m_blood2.Play();
    }
}
