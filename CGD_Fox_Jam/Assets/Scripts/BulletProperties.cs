using UnityEngine;
using System.Collections;

public class BulletProperties : MonoBehaviour 
{
    float aliveTime = 1.0f;
    public float damage = 1.0f;
    
    /// <summary>
    /// Called when 2D trigger event occurs
    /// </summary>
    /// <param name="col"></param>
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            gameObject.SetActive(false);
        }

    }

    /// <summary>
    /// fixed update called every frame, 
    /// update decrements bullet alive time and destroys them when it is 0
    /// </summary>
    void FixedUpdate()
    {
        //negate time down
        aliveTime -= Time.deltaTime;

        //if alive time is out destroy game object
        if (aliveTime <= 0 || this.transform.position.y < WorldGenerator.Instance.m_surfacePos + 0.1f)
        {
            gameObject.SetActive(false);
        }
    }
}
