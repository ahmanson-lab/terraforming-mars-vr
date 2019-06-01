using UnityEngine;
using System.Collections;

public class DotFPCElevatorSupport : MonoBehaviour {

	private Transform _parent = null;

	void OnTriggerEnter(Collider collider){
		if (collider.gameObject.tag == "Elevator2") {
			_parent = gameObject.transform.parent;
			gameObject.transform.parent = collider.gameObject.transform;
		}
	}

	void OnTriggerExit(Collider collider){
		if (collider.gameObject.tag == "Elevator2") {
			gameObject.transform.parent = _parent;
		}
	}

}
