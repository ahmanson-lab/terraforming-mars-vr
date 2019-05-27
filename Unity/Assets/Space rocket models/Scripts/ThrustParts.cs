using UnityEngine;
using System.Collections;

public class ThrustParts : MonoBehaviour {

	public float thrust = 50;
	public Rigidbody rocket;
	public bool EngineOn;
	private Transform RocketFlame;
	//private AudioSource engine;




	void Start () 
	{
		rocket = GetComponent<Rigidbody> ();
		EngineOn = false;
		RocketFlame = transform.Find ("flames_flame_red");

		//engine = GetComponent<AudioSource>();
	}


	void Update()
	{
		EngineStart ();
		if (EngineOn == true)
			rocket.AddRelativeForce (0, 0, thrust, ForceMode.Force);
	}

	private void EngineStart()
	{
		if (Input.GetKey ("down")) {
			EngineOn = true;
			StartCoroutine (NoFuel ());
			//engine.Play();
		}
	}
		IEnumerator NoFuel()
		{
		yield return new WaitForSeconds(20);
		thrust = 0F;
		RocketFlame.gameObject.SetActive (false);
		}
}
