﻿using UnityEngine;
using System.Collections;

public class BulletProperties : MonoBehaviour 
{
    float aliveTime = 1.0f;
    public float damage = 1.0f;
    public float speed;

    public Vector3 direction;
    
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

    void Update()
    {
        Vector3 newVel, newPos;

        if (this.gameObject.activeInHierarchy == true)
        {
            //negate time down
            aliveTime -= Time.deltaTime;


            newVel = speed * direction;
            newPos = transform.position + (Time.deltaTime * newVel);

            transform.position = newPos;

            //if alive time is out destroy game object
            if (aliveTime <= 0 || this.transform.position.y < WorldGenerator.Instance.m_surfacePos + 0.1f)
            {
                //gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0.0f, 0.0f, 0.0f)

                gameObject.SetActive(false);

            }
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
            //gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
            gameObject.SetActive(false);

        }
    }
}
