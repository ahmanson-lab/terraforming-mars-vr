using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchPadFlames : MonoBehaviour {



	public ParticleSystem []baseflames;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown ("up")) 
		{
			foreach (var baseflame in baseflames) 
			{
				baseflame.Play ();
			}
		
	}
}
}