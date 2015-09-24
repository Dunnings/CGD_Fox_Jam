using UnityEngine;
using System.Collections;

public class FoxMovement : MonoBehaviour
{
	Vector3 acc, vel;
	float mass;
	public float addForce;
	public float maxVel;
	public GameObject skyBox;
	public float breachHeight;

	void Start ()
	{
		acc = Vector3.zero;
		vel = Vector3.zero;

		mass = 1;
	}
	
	void Update ()
	{
		Vector3 force;

		if (transform.position.y > breachHeight)
		{
			force = Vector3.down * (addForce);

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

			//if (transform.rotation.z < 360 && transform.rotation.z > 181)
			//{

			gameObject.transform.Rotate(transform.forward, 2.5f);
			//Vector3 tempDir = gameObject.transform.up.normalized;
			//vel = vel.magnitude*tempDir;
				
				/*float step = vel.magnitude * Time.deltaTime;
				
				Vector3 temp = transform.up;

				Vector3.RotateTowards(temp, Vector3.down, step, 0.0f);
				
				Vector3 difference = temp - transform.up;
				
				transform.Rotate(transform.forward, temp.z);

				Vector3 tempDir = gameObject.transform.up.normalized;
				vel = vel.magnitude*tempDir;
			/*}
			else
			{
				gameObject.transform.Rotate(transform.forward, 2.5f);
				Vector3 tempDir = gameObject.transform.up.normalized;
				vel = vel.magnitude*tempDir;
			}*/

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
			
			if (Input.GetKey (KeyCode.RightArrow))
			{
				gameObject.transform.Rotate(transform.forward, -2.5f);
				Vector3 tempDir = gameObject.transform.up.normalized;
				vel = vel.magnitude*tempDir;
			}
			
			if (Input.GetKey (KeyCode.LeftArrow))
			{
				gameObject.transform.Rotate(transform.forward, 2.5f);
				Vector3 tempDir = gameObject.transform.up.normalized;
				vel = vel.magnitude*tempDir;
			}
		}


	}
}
