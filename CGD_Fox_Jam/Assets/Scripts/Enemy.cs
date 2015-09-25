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

    public int id;
    public float score;
    public float speed;
    public float bufferDistance;
    public float pushBackForce;
    public float bulletSpeed;
    public float shootCoolDown;
    public float lineOfSight;
    public float rifleFarmerPercentage = 0.6f;
    public float shotgunFarmerPercentage = 0.8f;
    public float assualtFarmerPercentage = 0.9f;
    float shootCountdown;

    public GameObject bullet;

    public List<Sprite> farmerSprites = new List<Sprite>();

    public GameObject m_DeathParticle;

    public void Start()
    {
       //create a random type based on percent weights
        float thisRand = Random.Range(0.0f, 1.0f);

        //generate farmer type
        if(thisRand > assualtFarmerPercentage)
        {
            this.type = EnemyType.assaultFarmer;
            this.gameObject.GetComponent<SpriteRenderer>().sprite = farmerSprites[2];
            shootCoolDown = 0.1f;
        }
        else if (thisRand > shotgunFarmerPercentage)
        {
            this.type = EnemyType.shotgunFarmer;
            this.gameObject.GetComponent<SpriteRenderer>().sprite = farmerSprites[1];
            shootCoolDown = 0.5f;
        }
        else
        {
            this.type = EnemyType.rifleFarmer;
            this.gameObject.GetComponent<SpriteRenderer>().sprite = farmerSprites[0];
            shootCoolDown = 0.5f;
        }

        //create a rand buffer distance 
        bufferDistance =  Random.Range(1, bufferDistance);

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
           gameObject.GetComponent<Rigidbody2D>().velocity = vel * Random.Range(speed, speed * 1.5f);

       }
       //else increment down on the x 
       else if (gameObject.GetComponent<Rigidbody2D>().position.x >= EnemyManager.GetInstance().player.transform.position.x + bufferDistance)
       {
           Vector3 vel = new Vector3(-1, 0, 0);
           gameObject.GetComponent<Rigidbody2D>().velocity = vel * Random.Range(speed, speed * 1.5f);
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
                    SpawnBullet(); 
                break;
                case(EnemyType.shotgunFarmer):
                    SpawnBullet();
                    SpawnBullet();           
                break;

                case (EnemyType.assaultFarmer):
                    SpawnBullet(); 
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
    /// spawns a bullet
    /// </summary>
    void SpawnBullet()
    {
        //create bullet game object
        GameObject newBullet = GameObject.Instantiate(bullet, this.transform.position, Quaternion.identity) as GameObject;
        //create a direction for the bullet based on player
        Vector3 direction = CalcBulletDirection();
        newBullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed; 
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //player hits enemy
        if (col.tag == "Player")
        {
            KillInstance();
        }

        //enemy hits enemy
        if (col.tag == "Enemy")
        {
            PushBack(col);
        }
      
    }

    void OnTriggerEnterStay(Collider2D col)
    {
        //enemy hits enemy
        if (col.tag == "Enemy")
        {
            PushBack(col);
        }

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

        gameObject.GetComponent<Rigidbody2D>().velocity = (AwayFrom * pushBackForce) * (speed/2.0f * Time.deltaTime) ;
    }

    /// <summary>
    /// destroy instance and remove from appropriate manager
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
        Destroy(gameObject);        
    }
}
