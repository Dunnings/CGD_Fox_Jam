using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour 
{
    public GameObject player;
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

    bool left;

    public void Start()
    {
        bufferDistance =  Random.Range(1, bufferDistance);
    }

    /// <summary>
    /// Called every frame
    /// </summary>
    public void Update()
    {
        //check type and call appropriate update
        switch (type)
        {
            case EnemyType.animal:
                break;
            case EnemyType.farmer:
                FarmerUpdate(); 
                break; 
        }
    }

    /// <summary>
    /// Enemy Farmer update
    /// </summary>
    public void FarmerUpdate()
    {
       //if enemy is less than the player pos increment up on 
       if(this.transform.position.x <= player.transform.position.x - bufferDistance)
       {
           this.transform.position = new Vector2( (this.transform.position.x + (Random.Range(speed, speed * 1.5f) * Time.deltaTime)), 
               this.transform.position.y);

           left = false;
       }
       //else increment down on the x 
       else if (this.transform.position.x >= player.transform.position.x + bufferDistance)
       {
           this.transform.position = new Vector2((this.transform.position.x - (Random.Range(speed, speed * 1.5f) * Time.deltaTime)),
               this.transform.position.y);

           left = true;
       }
       else
       {
           if(left)
           {
               this.transform.position = new Vector2((this.transform.position.x - (Random.Range((speed / 2.0f), (speed / 2.0f) * 1.5f) * Time.deltaTime)),
                   this.transform.position.y);
           }
           else
           {
               this.transform.position = new Vector2((this.transform.position.x + (Random.Range((speed / 2.0f), (speed / 2.0f) * 1.5f) * Time.deltaTime)),
                 this.transform.position.y);
           }
           
       }
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
                Destroy(this);
                break;

            case EnemyType.farmer:
                 EnemyManager.GetInstance().RemoveSpawnedFarmer(id);
                 Destroy(this);
                break;
        }
        
    }
}
