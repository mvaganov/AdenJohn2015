using UnityEngine;
using System.Collections;

public class MovingGuy : MonoBehaviour {

	Rigidbody rb;
	public float speed = 10;

	void Start () {
		rb = GetComponent<Rigidbody>();
	}
	
	void Update () {
		if(Vector3.Dot(rb.velocity, Vector3.down) < 0.1f) {
			rb.velocity = transform.forward * speed;
			transform.Rotate(Vector3.up, Random.Range(-360, 360) * Time.deltaTime);
		}
	}
}
