using UnityEngine;
using System.Collections;

public class Rock : MonoBehaviour
{
    public GameObject m_explosion;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            GameObject go = Instantiate(m_explosion);
            go.transform.position = transform.position + new Vector3(0f,0.1f,0f);
            go.GetComponent<ParticleSystem>().Play();

            col.gameObject.GetComponent<Fox>().m_camera.ShakeCamera(); 
            col.gameObject.GetComponent<FoxMovement>().vel *= 0.5f;
            Destroy(gameObject);
        }
    }
}
