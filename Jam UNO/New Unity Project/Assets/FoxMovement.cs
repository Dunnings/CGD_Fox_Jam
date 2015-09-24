using UnityEngine;
using System.Collections;

public class FoxMovement : MonoBehaviour
{
	Vector3 acc, vel;
	public float mass, addForce, gravForce, maxVel, breachHeight;
	bool breached, lastFrameBreached;

	void Start ()
	{
		acc = Vector3.zero;
		vel = Vector3.zero;
		breached = false;
		lastFrameBreached = false;
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
		}



		//once the fox has breached the surface gravity (a downwards force) pulls it back
		if (breached)
		{
			force = Vector3.down * (gravForce);

			force.z.Equals(0);
			Vector3 newVel, newPos;
			
			acc = force / mass;
			
			newVel = vel + (Time.deltaTime * acc);
			newPos = transform.position + (Time.deltaTime * vel);
			
			vel = newVel;
			transform.position = newPos;

			//roate the fox in the air
			float angle = Vector3.Angle(transform.up, Vector3.down);
			float step;

			//test angle up against left and right, if right is closest  
			if (Vector3.Angle(transform.up, Vector3.right) < Vector3.Angle(transform.up, Vector3.left))
			{
				angle *= -1;
			}

			step = (angle / vel.magnitude);
			transform.Rotate(Vector3.forward, step);
		}
		else
		{
			force = gameObject.transform.up * addForce;

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
			transform.position = newPos;
		}

		if (breached)
		{
			lastFrameBreached = true;
		}
		else
		{
			lastFrameBreached = false;
		}
	}


	void LateUpdate ()
	{
		if (Input.GetKey (KeyCode.RightArrow) && !breached)
		{
			gameObject.transform.Rotate(transform.forward, -2.5f);
			Vector3 tempDir = gameObject.transform.up.normalized;
			vel = vel.magnitude*tempDir;
		}
		
		if (Input.GetKey (KeyCode.LeftArrow) && !breached)
		{
			gameObject.transform.Rotate(transform.forward, 2.5f);
			Vector3 tempDir = gameObject.transform.up.normalized;
			vel = vel.magnitude*tempDir;
		}
	}
}
