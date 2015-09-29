using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {

	public bool gameOver;

	// Use this for initialization
	void Start ()
	{
		gameObject.GetComponent<Camera> ().enabled = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (gameObject.GetComponent<Camera>().enabled)
		{
			gameObject.GetComponent<Camera>().orthographicSize += 0.007875f;

			if (gameObject.GetComponent<Camera>().orthographicSize > 1.6f)
			{
				gameObject.GetComponent<Camera>().orthographicSize = 1.6f;
			}


			if (Input.GetMouseButtonDown(0))
			{
				Application.LoadLevel(0);
			}

		}
		
	}
}
