// This script is used together with a standard FirstPersonController class
// with the following changes (modified class FirstPersonController included in this
// package is named as DotFirstPersonController):
//
// public void _init(){
//   if((m_Camera != null) && (m_MouseLook != null)) {
//     m_MouseLook.Init (transform, m_Camera.transform);
//   }
// }
//
// This change allows to keep the same direction of the camera when
// control switches from the Class DotLadder to the Class DotFirstPersonController

using UnityEngine;
using System.Collections;

public class DotLadder: MonoBehaviour {
	
	public Texture tipOnLadder;
	public Texture tipOffLadder;
	
	private UnityStandardAssets.Characters.FirstPerson.DotFirstPersonController _fps = null;
	private UnityStandardAssets.Characters.FirstPerson.DotLadderController _lc = null;
	private bool _activated = false;
	private bool _use = false;
	private bool _has_controllers = false;
	
	void Start(){
		GameObject _go = GameObject.FindGameObjectWithTag("Player");
		if (_go != null) {
			_fps = _go.GetComponent<UnityStandardAssets.Characters.FirstPerson.DotFirstPersonController> ();
			_lc = _go.GetComponent<UnityStandardAssets.Characters.FirstPerson.DotLadderController> ();
			if ((_fps == null) || (_lc == null)) {
				Debug.LogWarning ("Not found \"LadderController\" or / and \"FirstPersonController\" components");
			} else {
				_activated = true;
			}
		}
	}
	
	void Update() {
		if(_activated && _has_controllers && Input.GetKeyDown(KeyCode.E)){
			if(_use){
				_use = _lc.enabled = false;
				_fps.enabled = true;
				_fps._init();
			} else {
				_fps.enabled = false;
				_use = _lc.enabled = true;
				_lc._init();
			}
		}
	}
	
	void OnTriggerEnter () {
		_has_controllers = true;
	}
	
	void OnTriggerExit () {
		_has_controllers = false;
		if (_activated) {
			_use = _lc.enabled = false;
			_fps.enabled = true;
			_fps._init();
		}
	}
	
	void OnGUI(){
		if (_has_controllers) {
			Texture _tmp = (_use) ? tipOffLadder : tipOnLadder;
			float _sw = Screen.width;
			float _sh = Screen.height;
			float _tw = _tmp.width;
			float _th = _tmp.height;
			GUI.DrawTexture (new Rect ((_sw - _tw) / 2, _sh - 36 - _th, _tw, _th), _tmp, ScaleMode.ScaleToFit, true); 
		}
	}
}