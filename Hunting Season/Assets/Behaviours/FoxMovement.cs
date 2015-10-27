using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FoxMovement : MonoBehaviour
{    
    //Acceleration and Velocity
    public Vector3 acc, vel;
    //Other variables
    public float mass, addForce, gravForce, maxVel, breachHeight;
    //If the fox has breached and if it breached last frame
    bool breached, lastFrameBreached;

    //Are the left/right buttons pressed
    bool leftPressed, rightPressed;

    private bool hasBounced = false;

    public float world_width = 1000f;
    public float world_height = 500f;

    void Start()
    {
        acc = Vector3.zero;
        vel = Vector3.zero;
        breached = false;
        lastFrameBreached = false;
        breachHeight = 0f;
    }

    void Update()
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

        if (!breached && lastFrameBreached)
        {
            vel /= 2;
        }
        else if (breached && !lastFrameBreached)
        {
            vel *= 1.4f;
        }

        Vector3 proposedNewPos = transform.position;

        if (breached)
        {
            force = Vector3.down * (gravForce);
            force.z = 0f;
            Vector3 newVel, newPos;

            acc = force / mass;

            newVel = vel + (Time.deltaTime * acc);
            newPos = transform.position + (Time.deltaTime * vel);

            vel = newVel;
            proposedNewPos = newPos;
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

        //If the fox is below the world depth bounce it up
        if (proposedNewPos.y < -world_height)
        {
            //BounceBottom();
        }
        //If the fox is too far right bounce back
        else if (proposedNewPos.x > world_width / 2f)
        {
            //BounceSide();
        }
        //If the fox is too far left bounce it back
        else if (proposedNewPos.x < -(world_width / 2f))
        {
            //BounceSide();
        }
        else
        {
            transform.position = proposedNewPos;

            if ((Input.GetKey(KeyCode.RightArrow) || rightPressed) && !breached && !hasBounced)
            {
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
            if ((Input.GetKey(KeyCode.LeftArrow) || leftPressed) && !breached && !hasBounced)
            {
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
        Vector3 diff = vel.normalized;
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        if (diff.x <= 0)
        {
            gameObject.transform.rotation = Quaternion.AngleAxis(rot_z + 270, Vector3.forward) * Quaternion.Euler(180, 0, 270);
        }
        else if (diff.x > 0)
        {
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
    /// Getter for velocity
    /// </summary>
    /// <returns></returns>
    public Vector3 GetVel()
    {
        return vel;
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
