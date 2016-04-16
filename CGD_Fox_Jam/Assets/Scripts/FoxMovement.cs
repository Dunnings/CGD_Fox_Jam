using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FoxMovement : MonoBehaviour
{
    public GameObject menu;

    public Color m_col1;
    public Color m_col2;

    public GameObject m_particles;
    public AudioSource breachSound;
    //Acceleration and Velocity
	public Vector3 acc, vel;
    //Other variables
	public float mass, addForce, gravForce, maxVel, breachHeight;
    //If the fox has breached and if it breached last frame
	bool breached, lastFrameBreached;

    //Are the left/right buttons pressed
    bool leftPressed, rightPressed;

    //Link to the animator
    public Animator m_animator;

    private bool hasBounced = false;
    
    void Start ()
	{
        acc = Vector3.zero;
		vel = Vector3.zero;
		breached = false;
		lastFrameBreached = false;
        breachHeight = WorldGenerator.Instance.m_surfacePos;
	}
	
	void Update ()
	{
        Vector3 force;

		if (transform.position.y >= breachHeight)
		{
			breached = true;
		}
		else
		{
			breached = false;
		}


		//upon entering the ground again halve speed
		if (!breached && lastFrameBreached)
		{
			vel /= 2;
            //Toggle animation state
            m_animator.SetBool("burrowing", true);
            m_animator.SetBool("flying", false);

            m_particles.GetComponent<ParticleSystem>().enableEmission = true;
            m_particles.GetComponentInChildren<AudioSource>().volume = .3f;
        }
        else if(breached && !lastFrameBreached)
        {
            //Increase velocity by (40%) upon breaching
            vel *= 1.4f;
            //Toggle animation state
            m_animator.SetBool("flying", true);
            m_animator.SetBool("burrowing", false);
            breachSound.Play();
            m_particles.GetComponentInChildren<AudioSource>().volume = 0f;

            m_particles.GetComponent<ParticleSystem>().enableEmission = false;
        }

        //The proposed new position
        Vector3 proposedNewPos = transform.position;
        
		//once the fox has breached the surface gravity (a downwards force) pulls it back
		if (breached)
		{
			force = Vector3.down * (gravForce);
            force.z = 0f;
			//force.z.Equals(0);
			Vector3 newVel, newPos;
			
			acc = force / mass;
			
			newVel = vel + (Time.deltaTime * acc);
			newPos = transform.position + (Time.deltaTime * vel);
			
			vel = newVel;
			proposedNewPos = newPos;

			////roate the fox in the air
			//float angle = Vector3.Angle(transform.up, Vector3.down);
			//float step;

			////test angle up against left and right, if right is closest  
			//if (Vector3.Angle(transform.up, Vector3.right) < Vector3.Angle(transform.up, Vector3.left))
			//{
			//	angle *= -1;
			//}

			//step = (angle / vel.magnitude);
			//transform.Rotate(Vector3.forward, step);
		}
		else
		{
            if (vel.x >= 0)
            {
                force = Vector3.right * addForce;
            }
            else
            {
                force = -Vector3.right * addForce;
            }

            force.z.Equals(0);
			Vector3 newVel, newPos;
			
			if (vel.magnitude >= maxVel)
			{
				force = Vector3.zero;
			}
			
			acc = force / mass;
			
			newVel = vel + (Time.deltaTime * acc);
			
			newPos = transform.position + (Time.deltaTime * vel);
			
			vel = newVel;
			proposedNewPos = newPos;
		}
        
		if (breached)
		{
			lastFrameBreached = true;
		}
		else
		{
			lastFrameBreached = false;
        }

        #region bounce
        //If the fox is below the world depth bounce it up
        if (proposedNewPos.y < WorldGenerator.Instance.m_surfacePos - WorldGenerator.Instance.m_depth)
        {
            BounceBottom();
        }
        //If the fox is too far right bounce back
        else if (proposedNewPos.x > WorldGenerator.Instance.m_width / 2f)
        {
            BounceSide();
        }
        //If the fox is too far left bounce it back
        else if (proposedNewPos.x < -(WorldGenerator.Instance.m_width / 2f))
        {
            BounceSide();
        }
        //If the fox is in the map
        else
        {
            //Set the fox's position to the proposed new position
            transform.position = proposedNewPos;
            //If right is pressed and the fox has not breached
            if ((Input.GetKey(KeyCode.RightArrow) || rightPressed) && !breached && !hasBounced && !menu.activeSelf)
            {
                //Rotate the fox by (-2.5)
                //gameObject.transform.Rotate(Vector3.forward, 2.5f);
                Vector3 tempDir = -gameObject.transform.up.normalized * Time.deltaTime * 20f;
                if (vel.x > 0)
                {
                    vel = vel + tempDir;
                }
                else
                {
                    vel = vel - tempDir;
                }
                
            }
            //If left is pressed and the fox has not breached 
            if ((Input.GetKey(KeyCode.LeftArrow) || leftPressed) && !breached && !hasBounced && !menu.activeSelf)
            {
                //Rotate the fox by (2.5)
                //gameObject.transform.Rotate(Vector3.forward, 2.5f);
                Vector3 tempDir = gameObject.transform.up.normalized * Time.deltaTime * 20f;
                if (vel.x > 0)
                {
                    vel = vel + tempDir;
                }
                else
                {
                    vel = vel - tempDir;
                }
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                UnPressLeft();
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                UnPressRight();
            }

            vel = Vector3.ClampMagnitude(vel, maxVel);
        }
        #endregion
        //Get the velocity normalized
        Vector3 diff = vel.normalized;
        //Calculate z rotation based on the difference in x and y of normalized velocity
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        //Rotate the fox based on the rotation (plus 90 degrees)x 
        if (diff.x <= 0)
        {
            m_particles.transform.rotation = Quaternion.AngleAxis(rot_z + 90, Vector3.forward) * Quaternion.Euler(180, 0, 270);
            gameObject.transform.rotation = Quaternion.AngleAxis(rot_z + 270, Vector3.forward) * Quaternion.Euler(180, 0, 270);
        }
        else if (diff.x > 0)
        {
            m_particles.transform.rotation = Quaternion.AngleAxis(rot_z + 90, Vector3.forward) * Quaternion.Euler(180, 0, 270);
            gameObject.transform.rotation = Quaternion.AngleAxis(rot_z + 270, Vector3.forward) * Quaternion.Euler(0, 180, 270);
        }
    }
    
    /// <summary>
    /// Called when the user presses left
    /// </summary>
    public void PressLeft()
    {
        leftPressed = true;
    }

    /// <summary>
    /// Called when the user presses right
    /// </summary>
    public void PressRight()
    {
        rightPressed = true;
    }

    /// <summary>
    /// Called when the user releases left
    /// </summary>
    public void UnPressLeft()
    {
        ReleaseBounce();
        leftPressed = false;
    }

    /// <summary>
    /// Called when the user releases right
    /// </summary>
    public void UnPressRight()
    {
        ReleaseBounce();
        rightPressed = false;
    }
    
    /// <summary>
    /// Reverses the fox's y velocity and decreases the fox's velocity by 75%
    /// </summary>
    public void BounceBottom()
    {
        hasBounced = true;
        vel.y = -vel.y;

        Vector3 diff = vel;
        diff.Normalize();
        vel *= 0.25f;
    }

    /// <summary>
    /// Reverses the fox's x velocity and decreases the fox's velocity by 75%
    /// </summary>
    public void BounceSide()
    {
        hasBounced = true;
        vel.x = -vel.x;

        Vector3 diff = vel;
        diff.Normalize();
        vel *= 0.25f;
    }

    public void ReleaseBounce()
    {
        hasBounced = false;
    }
}
