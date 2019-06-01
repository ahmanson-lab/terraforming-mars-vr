using UnityEngine;
using System.Collections;

public class DotHskElevator2ConControlCol : MonoBehaviour {

	private bool curVisible = true;
	private CursorLockMode curLock = CursorLockMode.None;
	private bool initialized = false;

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			if(!initialized){
				curVisible = Cursor.visible;
				curLock = Cursor.lockState;
				initialized = true;
			}
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
	}

	void OnTriggerExit(Collider other){
		if ((other.tag == "Player") && initialized ) {
			Cursor.lockState = curLock;
			Cursor.visible = curVisible;
		}
	}

}
