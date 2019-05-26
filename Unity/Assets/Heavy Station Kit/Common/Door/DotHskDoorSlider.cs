using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class dotHskDoorFlap : System.Object {
	public Vector3 openPos = new Vector3();
	public Vector3 closedPos = new Vector3();
	public Vector3 almostOpenPos = new Vector3();
	public Vector3 almostClosedPos = new Vector3();
	public Transform flap;
}

public abstract class DotHskDoorsEventsClass : MonoBehaviour {
	[HideInInspector]public DotHskDoorSlider door;
	public abstract void OnInit();
	public abstract void OnStartMotion(float fromPos, int dir);
	public abstract void OnChangeMode(bool isOff, bool isBlocked);
	public abstract void OnBlock();
	public abstract void OnStop();
}

[ExecuteInEditMode]
public class DotHskDoorSlider : MonoBehaviour {

	public List<dotHskDoorFlap> doorFlaps;
	public float motionTime = 1f;

	[HideInInspector]public DotHskDoorsEventsClass doorEvents;
	private int dir = 0; // -1 - closing, 0 - idle, 1 - opening
	private float curPos = 0f; // normalized current position of flap [0..1]
	private float targetPos = 0f; // normalized target position

	private bool isOff = false;
	private bool isBlocked = false;

	void Start(){
		Init ();
	}

	void Update () {
		if (Application.isPlaying) {
			if (dir != 0) {
				float newPos = curPos + dir * ((motionTime == 0) ? 1f : 1f / motionTime) * Time.deltaTime; 
				if (dir * (targetPos - newPos) < 0.01f) {
					newPos = targetPos;
					dir = 0;
				}
				moveTo (newPos);
				if ((doorEvents != null) && (dir==0)) { doorEvents.OnStop(); }
			}
		} else {
			Init();
		}
	}

	void Init(){
		if (doorEvents == null) {
			doorEvents = GetComponent<DotHskDoorsEventsClass>();
			if (doorEvents != null) {
				doorEvents.door = this;
				doorEvents.OnInit();
			}
		}
	}

	private void moveTo(float newPos){
		if (newPos != curPos) {
			for (int n = 0; n < doorFlaps.Count; n++) {
				doorFlaps[n].flap.localPosition = Vector3.Lerp (doorFlaps[n].closedPos, doorFlaps[n].openPos, newPos);
			}
			curPos = newPos;
		}
	}

	public bool setMode(dotHskDoorMode mode, bool isInit){
		int _mode = (int)mode;
		bool isClosed = (_mode & (int)dotHskDoorStats.closed) != 0;
		// set statuses
		isOff = (_mode & (int)dotHskDoorStats.off) != 0;
		isBlocked = (_mode & (int)dotHskDoorStats.blocked) != 0;
		if (doorEvents != null) { doorEvents.OnChangeMode(isOff, isBlocked); }
		// Check modes
		if ( (_mode & (int)dotHskDoorStats.broken) != 0 ) {
			// Broken
			for (int n = 0; n < doorFlaps.Count; n++) {
				Vector3 v = doorFlaps[n].flap.localPosition = isClosed ? doorFlaps[n].almostClosedPos : doorFlaps[n].almostOpenPos;
				float m = doorFlaps[n].openPos.magnitude;
				curPos = (m==0) ? 0 : v.magnitude / m;
			}
			dir = 0;
		} else {
			// Operate
			float newPos = isClosed ? 0f : 1f;
			targetPos = newPos;
			if (Application.isPlaying && (newPos != curPos) && !isInit) {
				// Move in new position
				dir = (newPos > curPos) ? 1 : -1;
				if (doorEvents != null) { doorEvents.OnStartMotion(curPos, dir); }
			} else {
				// Set new position
				dir = 0;
				moveTo (newPos);
			}
		}
		return true;
	}

	void operate(bool open){
		if (isOff) { return; }
		if (isBlocked) {
			if ((doorEvents != null) && open) { doorEvents.OnBlock(); }
		} else {
			targetPos = open ? 1 : 0f;
			dir = (curPos < targetPos) ? 1 : -1;
			if (doorEvents != null) { doorEvents.OnStartMotion(curPos, dir); }
		}
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "Player") { operate( true ); }
	}

	void OnTriggerExit(Collider other){
		if (other.tag == "Player") { operate( false ); }
	}

}
