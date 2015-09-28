using UnityEngine;
using System.Collections;

public class SFXManager : MonoBehaviour {
    public static SFXManager Instance;

    public AudioSource m_source;

	// Use this for initialization
	void Start () {
        Instance = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void PlayDeathSound()
    {
        m_source.pitch = Random.Range(0.8f, 1.2f);  
        m_source.Play();
    }
}
