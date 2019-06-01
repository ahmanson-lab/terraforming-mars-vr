using UnityEngine;
using System.Collections;

public enum dotHskDoorStats { closed=1, broken=2, off=4, blocked=8 };

public enum dotHskDoorMode {
	active = dotHskDoorStats.closed, 
	blocked = dotHskDoorStats.closed + dotHskDoorStats.blocked,
	brockenOpen = dotHskDoorStats.broken + dotHskDoorStats.off, 
	brockenClosed = dotHskDoorStats.closed + dotHskDoorStats.broken + dotHskDoorStats.off,
	inactiveOpen = dotHskDoorStats.off,
	inactiveClosed = dotHskDoorStats.closed+dotHskDoorStats.off
};

[ExecuteInEditMode]
public class DotHskDoor : MonoBehaviour {

	// Public setting
	public dotHskDoorMode mode = dotHskDoorMode.active;
	public DotHskDoorSlider doorScript;

	// Private setting
	private dotHskDoorMode prevMode = dotHskDoorMode.active;
	private bool first = true;

	void Update () {
		if ((doorScript != null) && ((prevMode != mode) || first) ) {
			if (doorScript.setMode (mode, first)) { prevMode = mode; } else { mode = prevMode; }
			first = false;
		}
	}

}

