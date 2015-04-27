using UnityEngine;
using System.Collections;

public class ZoneTo : MonoBehaviour {

	public string nextLevel;

	void OnTriggerEnter(Collider other) {
		if(other.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>()) {
			Application.LoadLevel(nextLevel);
		}
	}
}
