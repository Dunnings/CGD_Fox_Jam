﻿using UnityEngine;
using System.Collections;

public class BreachParticle : MonoBehaviour {

    public Color m_debrisColor;

    public ParticleSystem m_smallDebris;
    public ParticleSystem m_largeDebris;

    // Use this for initialization
    void Start ()
    {
        m_smallDebris.startColor = m_debrisColor;
        m_largeDebris.startColor = m_debrisColor;
        PlayBreach(); 
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void PlayBreach()
    {
        m_smallDebris.Play();
        m_largeDebris.Play();
    }
}
