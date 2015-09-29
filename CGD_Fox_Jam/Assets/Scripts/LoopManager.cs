using UnityEngine;
using System.Collections;

public class LoopManager : MonoBehaviour {

	public GameObject fox, foxCamera, gameOverCam;
	public Canvas GUI;
	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
		//"FOOOOOOOOOOOOOOOOOOOOOOX" - Solid Snake, Metal Gear Solid (1998)
		if (fox.GetComponent<Fox> ().m_hunger <= 0 && foxCamera.GetComponent<Camera>().enabled == true)
		{
			//set fox to not move at 0,0,0
			fox.transform.position = Vector3.zero;
			fox.GetComponent<FoxMovement>().addForce = 0;
			fox.GetComponent<FoxMovement>().maxVel = 0;

			GUI.enabled = false;
			foxCamera.GetComponent<Camera>().enabled = false;
			gameOverCam.GetComponent<Camera>().enabled = true;
		}
	
	}
}
