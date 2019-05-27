using UnityEngine;
using System.Collections;

public class PhysicsSwitch2 : MonoBehaviour {

	private Rigidbody MainBody;



	void Start () {
		MainBody = GetComponent<Rigidbody> ();




	}


	void Update () {

		if (Input.GetKey ("down")) {
			//MainBody.isKinematic = true;
			MainBody.GetComponent<BoxCollider> ().enabled = false;

		}
	}
}
