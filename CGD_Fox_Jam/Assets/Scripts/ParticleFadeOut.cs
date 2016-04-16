using UnityEngine;
using System.Collections;

public class ParticleFadeOut : MonoBehaviour {

    public SpriteRenderer m_renderer;
    bool fadeOut = false;


	// Use this for initialization
	void Awake () {
        gameObject.hideFlags = HideFlags.HideInHierarchy;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void KillMe(float lifeSpan)
    {
        StartCoroutine(startFade(lifeSpan));
    }

    IEnumerator startFade(float startTime)
    {
        yield return new WaitForSeconds(startTime);
        for (float i = 1f; i > 0f; i-= 0.1f)
        {
            yield return new WaitForSeconds(0.05f);
            Color c = m_renderer.color;
            c.a = i;
            m_renderer.color = c;
        }
        gameObject.SetActive(false);
    }
}
