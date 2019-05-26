using UnityEngine;
using System.Collections;

public class DotFPCLight : MonoBehaviour {

	private Light _light=null;

	void Start () {
		_light = GetComponent<Light>();
	}
	
	void Update () {
		if ( (_light != null) && (Input.GetKeyDown (KeyCode.L)) ) {
			_light.enabled = !_light.enabled;
		}	
	}
}
