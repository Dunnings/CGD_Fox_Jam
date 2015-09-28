using UnityEngine;
using System.Collections.Generic;

public class SFXManager : MonoBehaviour {

    public List<AudioClip> deathSounds = new List<AudioClip>();

    public AudioSource m_source;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void PlayDeathSound()
    {
        m_source.pitch = Random.Range(0.8f, 1.2f);
        int randNo = Random.Range(0, deathSounds.Count - 1);
        m_source.clip = deathSounds[randNo];
        m_source.Play();
    }
}
