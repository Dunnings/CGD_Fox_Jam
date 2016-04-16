using UnityEngine;
using System.Collections.Generic;

public class SFXManager : MonoBehaviour
{

    public List<AudioClip> deathSounds = new List<AudioClip>();
    public AudioSource m_source;

    public void toggleSound(bool b)
    {
        GameObject[] audioComps= GameObject.FindGameObjectsWithTag("Audio");

        foreach(GameObject g in audioComps)
        {
            g.GetComponent<AudioSource>().mute = b;
        }
    }

    public void PlayDeathSound()
    {
        m_source.pitch = Random.Range(0.8f, 1.2f);
        int randNo = Random.Range(0, deathSounds.Count - 1);
        m_source.clip = deathSounds[randNo];
        m_source.Play();
    }
}
