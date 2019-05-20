using UnityEngine;
using System.Collections;

public class DotHskElevator2ConCollider : MonoBehaviour {

	private DotHskElevator2Console consoleScript = null;

	void Start () {
		consoleScript = GetComponentInParent<DotHskElevator2Console>();
	}
	
	void OnTriggerEnter(Collider other) {
		if ((consoleScript!=null) && (other.tag == "Player")) {
			consoleScript.OnConsoleEnter (other);
		}
	}

	void OnTriggerExit(Collider other){
		if ((consoleScript!=null) && (other.tag == "Player")) {
			consoleScript.OnConsoleExit (other);
		}
	}

}
