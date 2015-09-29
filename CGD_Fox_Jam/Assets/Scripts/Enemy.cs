using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour 
{
    public enum EnemyType
    {
        rifleFarmer,
        shotgunFarmer, 
        assaultFarmer
        
    };
    public EnemyType type;

	Collider2D currentCol;

    public int id;
	public float score, speed, bufferDistance, pushBackForce, bulletSpeed, shootCoolDown, lineOfSight, rifleFarmerPercentage = 0.6f,
	shotgunFarmerPercentage = 0.8f, assualtFarmerPercentage = 0.9f ;
    float shootCountdown;

	bool stayHit = false, colHit = false;

	Vector3 poot = Vector3.zero;

    public GameObject bullet, m_DeathParticle;
    public List<Sprite> farmerSprites = new List<Sprite>();
    private List<GameObject> m_bullets = new List<GameObject>();
    

    public void Start()
    {
        //pool bullets, give each enemy 50 bullets
        for (int i = 0; i < 10; i++)
        {
            Debug.Log("Spawning Bullets");
            SpawnBullet();
        }

        //create a random type based on percent weights
        float thisRand = Random.Range(0.0f, 1.0f);

        //generate farmer type
        if(thisRand > assualtFarmerPercentage)
        {
            this.type = EnemyType.assaultFarmer;
            //this.gameObject.GetComponent<SpriteRenderer>().sprite = farmerSprites[2];
            shootCoolDown = 0.1f;
        }
        else if (thisRand > shotgunFarmerPercentage)
        {
            this.type = EnemyType.shotgunFarmer;
            //this.gameObject.GetComponent<SpriteRenderer>().sprite = farmerSprites[1];
            shootCoolDown = 0.5f;
        }
        else
        {
            this.type = EnemyType.rifleFarmer;
            //this.gameObject.GetComponent<SpriteRenderer>().sprite = farmerSprites[0];
            shootCoolDown = 0.5f;
        }

        

        //create a rand buffer distance 
        bufferDistance = Random.Range(1, bufferDistance);

        //set the shoot count down to the shoot coolOff
        shootCountdown = shootCoolDown;
    }

    /// <summary>
    /// Called every frame
    /// </summary>
    public void FixedUpdate()
    {
        FarmerUpdate();
        if (EnemyManager.GetInstance().player.transform.position.y >= WorldGenerator.Instance.m_surfacePos
            && Vector3.Distance(EnemyManager.GetInstance().player.transform.position, this.transform.position) < lineOfSight)
        {
            ShootAtPlayer();
        }             
    }

    /// <summary>
    /// Enemy Farmer update
    /// </summary>
    public void FarmerUpdate()
    {        
       //if enemy is less than the player pos increment up on 
       if (gameObject.GetComponent<Rigidbody2D>().position.x <= EnemyManager.GetInstance().player.transform.position.x - bufferDistance)
       {   
           Vector3 vel = new Vector3(1, 0, 0);
           
			if (colHit || stayHit)
			{
				gameObject.GetComponent<Rigidbody2D>().velocity = poot ;
			}
			else
			{
				gameObject.GetComponent<Rigidbody2D>().velocity = vel * Random.Range(speed, speed * 1.5f);
			}

			transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
       }
       //else increment down on the x 
       else if (gameObject.transform.position.x >= EnemyManager.GetInstance().player.transform.position.x + bufferDistance)
       {
           Vector3 vel = new Vector3(-1, 0, 0);

			if (colHit || stayHit)
			{
				gameObject.GetComponent<Rigidbody2D>().velocity = poot ;
			}
			else
			{
				gameObject.GetComponent<Rigidbody2D>().velocity = vel * Random.Range(speed, speed * 1.5f);
			}
           transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
       }
    }
    
    /// <summary>
    /// Script controls AI shooting
    /// </summary>
    public void ShootAtPlayer() 
    {
        //if shoot cool down is over shoot
        if (shootCountdown <= 0)
        {
            switch(type)
            {
                //different enemy types different firing types
                case(EnemyType.rifleFarmer):
                    FireBullet(); 
                break;
                case(EnemyType.shotgunFarmer):
                    FireBullet();
                    FireBullet();           
                break;

                case (EnemyType.assaultFarmer):
                    FireBullet(); 
                break;
            }

            //reset the cool off
            shootCountdown = shootCoolDown; 
        }
        else
        {
            //decrement shoot cool off
            shootCountdown -= Time.deltaTime;
        }
    }

    /// <summary>
    /// calculate a random bullet direction
    /// </summary>
    /// <returns></returns>
    Vector3 CalcBulletDirection()
    {
       return new Vector3(Random.Range(EnemyManager.GetInstance().player.transform.position.x, EnemyManager.GetInstance().player.transform.position.x + 1.0f),
                        Random.Range(EnemyManager.GetInstance().player.transform.position.y, EnemyManager.GetInstance().player.transform.position.y + 1.0f), 0.0f) -
                        this.gameObject.transform.position;
    }

    /// <summary>
    /// spawns a bullet and stores it in a list 
    /// of pooled bullets attached to the enemy
    /// </summary>
    void SpawnBullet()
    {
        //create bullet game object
        GameObject newBullet = GameObject.Instantiate(bullet, this.transform.position, Quaternion.identity) as GameObject;        

        newBullet.transform.parent = this.transform;
        newBullet.SetActive(false);

        m_bullets.Add(newBullet);
    }

    /// <summary>
    /// Fires a pooled GameObject Bullet from list of GameObjects attached 
    /// to the player
    /// </summary>
    void FireBullet()
    {
        for (int i = 0; i < m_bullets.Count; i++)
        {
            if(m_bullets[i].activeInHierarchy == false)
            {
                Debug.Log("bang");

                //create a direction for the bullet based on player
                Vector3 direction = CalcBulletDirection();
                
                m_bullets[i].SetActive(true);


                m_bullets[i].GetComponent<BulletProperties>().SetVelocity(direction, bulletSpeed);
                //m_bullets[i].GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
                              
                break;
            }
        }
    }

    /// <summary>
    /// Applies a push back force between two objects
    /// </summary>
    /// <param name="col"></param>
    void PushBack(Collider2D col)
    {
        Vector3 collisionPos = col.gameObject.GetComponent<Rigidbody2D>().transform.position;

        Vector3 AwayFrom = this.gameObject.GetComponent<Rigidbody2D>().transform.position - collisionPos;

        AwayFrom.Normalize();

        poot = (AwayFrom * pushBackForce) * (Time.deltaTime);
    }

    /// <summary>
    /// set instance of game object to be inactive
    /// </summary>
    private void KillInstance()
    {
        EnemyManager.GetInstance().player.GetComponent<Fox>().Reward();

        EnemyManager.GetInstance().RemoveSpawnedFarmer(id);
        GameObject deathParticles = Instantiate(m_DeathParticle);

        Vector3 pos = transform.position;
        pos.y = WorldGenerator.Instance.m_surfacePos;
        deathParticles.transform.position = pos;
        Destroy(deathParticles, 6f);
        FindObjectOfType<SFXManager>().PlayDeathSound();

        //decrease active amount 
        EnemyManager.GetInstance().ActiveEnemies--;
        //don't destory anymore due to object pooling
        //Destroy(gameObject);        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
		currentCol = col;

        //player hits enemy
        if (col.tag == "Player")
        {
            KillInstance();
        }

        //enemy hits enemy
        if (col.tag == "Enemy") {
			PushBack (col);
			colHit = true;
		}      
    }

    void OnTriggerStay2D(Collider2D col)
    {
		currentCol = col;

		//enemy hits enemy
		if (col.tag == "Enemy") {
			PushBack (col);
			stayHit = true;
		}
	}

	void OnTriggerExit2D(Collider2D col)
	{
		colHit = false;
		stayHit = false;
	}	
}
