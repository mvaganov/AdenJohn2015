using UnityEngine;
using System.Collections;

public class TeleportAfterFall : MonoBehaviour {
    Vector3 startLocation;
	// Use this for initialization
	void Start () {
        startLocation = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	    if (transform.position.y < -10)
        {
            transform.position = startLocation;
        }
	}
}
