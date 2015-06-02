using UnityEngine;
using System.Collections;

public class adensplayermove : MonoBehaviour {

	public float xSensitivity = 10;
	// Use this for initialization
	void Start () {
		GetComponent<Rigidbody> ().freezeRotation = true;
	
	}
	
	// Update is called once per frame
	void Update () {
		float mouseX = Input .GetAxis ("Mouse X");
		transform.Rotate (0, mouseX * xSensitivity, 0);
	
		float moveForward = Input.GetAxis ("Vertical");
	    if (moveForward != 0) {
			GetComponent<Rigidbody> ().velocity = transform.forward * moveForward;
		}
	}
}
