using UnityEngine;
using System.Collections.Generic;

public class Cloud : MonoBehaviour {

    public SpriteRenderer m_renderer;
    public List<Sprite> m_cloudsSprites = new List<Sprite>();
    float speed = 1f;

	// Use this for initialization
	void Start () {
        m_renderer.sprite = m_cloudsSprites[Random.Range(0, m_cloudsSprites.Count - 1)];
        speed = Random.Range(0.5f, 1.3f);
    }
	
	// Update is called once per frame
	void Update () {
        transform.position -= new Vector3(speed * Time.deltaTime, 0f, 0f);
        if (transform.position.x < -(WorldGenerator.Instance.m_width / 2))
        {
            gameObject.SetActive(false);
        }
	}
}
