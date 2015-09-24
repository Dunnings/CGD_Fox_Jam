using UnityEngine;
using System.Collections;

public class LookAtFox : MonoBehaviour {

	public GameObject foxObject;

	void Start ()
	{
	}

	void LateUpdate ()
	{
		gameObject.transform.position = new Vector3(foxObject.transform.position.x, foxObject.transform.position.y, -100f);

	}
}
