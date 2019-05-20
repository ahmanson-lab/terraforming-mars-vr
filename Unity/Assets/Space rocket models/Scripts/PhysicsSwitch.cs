using UnityEngine;
using System.Collections;

public class PhysicsSwitch : MonoBehaviour {

	private Rigidbody RocketBodyPart;



	void Start () {
		RocketBodyPart = GetComponent<Rigidbody> ();


		
	
	}
	

	void Update () {

		if (Input.GetKey ("down")) {
			RocketBodyPart.isKinematic = false;
			RocketBodyPart.GetComponent<BoxCollider> ().enabled = true;

		}
	}
}
