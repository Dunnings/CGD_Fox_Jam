using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour 
{
    public enum EnemyType
    {
        animal,
        farmer
    };

    public EnemyType type;
    public int id;
    public int health;
    public float score;
    public float speed;
    public float bufferDistance;
    public float pushBackForce;

    public void Start()
    {
        bufferDistance =  Random.Range(1, bufferDistance);
    }

    /// <summary>
    /// Called every frame
    /// </summary>
    public void FixedUpdate()
    {
        //check type and call appropriate update
        switch (type)
        {
            case EnemyType.animal:
                break;
            case EnemyType.farmer:
                FarmerUpdate();
                //if (EnemyManager.GetInstance().player.transform.position.y >= WorldGenerator.Instance.m_surfacePos)
                //{
                //    ShootAtPlayer();
                //}
                break; 
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
    
    public void ShootAtPlayer()
    {
        Debug.Log("Shooting at Player");
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
        switch(type)
        {
            case EnemyType.animal:
                EnemyManager.GetInstance().RemoveSpawnedFarmer(id);
                Destroy(gameObject);
                break;

            case EnemyType.farmer:
                 EnemyManager.GetInstance().RemoveSpawnedFarmer(id);
                 Destroy(gameObject);
                break;
        }
        
    }
}
