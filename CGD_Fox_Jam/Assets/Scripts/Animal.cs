﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Animal : MonoBehaviour 
{
    public enum AnimalType
    {
        worm,
        rabbit,
        chickhen
    };

    public AnimalType type;

    public int id;

    private float score;
    public float speed;
    public float wormPercentage;
    public float rabbitPercentage;
    public float chickhenPercentage;
    public float pushBackForce = 1.0f;

    public GameObject m_deathParticles;

    public List<Sprite> animalSprites = new List<Sprite>();

    private Vector2 wayPoint = new Vector2();

    public void Init(Vector3 _spawnPos)
    {
        //create a random type based on percent weights
        float thisRand = Random.Range(0.0f, 1.0f);

        //generate animal type
        if (thisRand > wormPercentage)
        {
            this.type = AnimalType.chickhen;
            //this.gameObject.GetComponent<SpriteRenderer>().sprite = animalSprites[2];
        }
        else if (thisRand > rabbitPercentage)
        {
            this.type = AnimalType.rabbit;
            //this.gameObject.GetComponent<SpriteRenderer>().sprite = animalSprites[1];
        }
        else
        {
            this.type = AnimalType.worm;
            //this.gameObject.GetComponent<SpriteRenderer>().sprite = animalSprites[0];
        }
        transform.position = _spawnPos;

    }

    public void FixedUpdate()
    {
        if (this.transform.position.x >= wayPoint.x - 1.0f
            && this.transform.position.x <= wayPoint.x + 1.0f)
        {
            wayPoint = CalcNewWayPoint();
        }

        //if enemy is less than the player pos increment up on 
        if (this.transform.position.x <= wayPoint.x)
        {
            Vector3 vel = new Vector3(1, 0, 0);
            gameObject.GetComponent<Rigidbody2D>().velocity = vel * Random.Range(speed, speed * 1.5f);
            gameObject.transform.localScale = new Vector3(-0.2f, 0.2f, 0.2f);
            

        }
        //else increment down on the x 
        else if (this.transform.position.x >= wayPoint.x)
        {
            Vector3 vel = new Vector3(-1, 0, 0);
            gameObject.GetComponent<Rigidbody2D>().velocity = vel * Random.Range(speed, speed * 1.5f);
            gameObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            
        }
    }

    Vector2 CalcNewWayPoint()
    {
        return new Vector2(Random.Range(0f - WorldGenerator.Instance.m_width / 2f, WorldGenerator.Instance.m_width / 2f), WorldGenerator.Instance.m_surfacePos);     
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //player hits enemy
        if (col.tag == "Player")
        {
            KillInstance();
        }

        if(col.tag == "Animal")
        {
            PushBack(col);
        }

    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Animal")
        {
            PushBack(col);
        }
    }

    private void KillInstance()
    {
        GameManager.instance.KillCount++;


        AnimalManager.instance.RemoveSpawnedAnimal(id);
        GameObject deathParticles = Instantiate(m_deathParticles);

        Vector3 pos = transform.position;
        pos.y = WorldGenerator.Instance.m_surfacePos;
        deathParticles.transform.position = pos;
        Destroy(deathParticles, 4f);
        FindObjectOfType<SFXManager>().PlayDeathSound();
        FindObjectOfType<Fox>().SmallReward();
        
    }

    /// <summary>
    /// Applies a push back force between two objects
    /// </summary>
    /// <param name="col"></param>
    void PushBack(Collider2D col)
    {
        Vector3 collisionPos = col.gameObject.GetComponent<Rigidbody2D>().transform.position;

        Vector3 AwayFrom = collisionPos - this.gameObject.GetComponent<Rigidbody2D>().transform.position;

        AwayFrom.Normalize();

        gameObject.GetComponent<Rigidbody2D>().velocity = (AwayFrom * pushBackForce) * (speed / 2.0f * Time.deltaTime);
    }

}
