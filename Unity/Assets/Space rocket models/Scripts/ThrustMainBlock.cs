using UnityEngine;
using System.Collections;

public class ThrustMainBlock : MonoBehaviour {

	public float thrust = 50;
	public Rigidbody rocket;
	public bool EngineOn;
	[SerializeField] private AudioClip engine;
	private AudioSource engineSource;
	public ParticleSystem []flames;
	 





	void Start () 
	{
		rocket = GetComponent<Rigidbody> ();
		EngineOn = false;
		engineSource = gameObject.AddComponent<AudioSource> ();
		engineSource.clip = engine;


		 
		}
	

	void Update()
	{
		EngineStart ();
		if (EngineOn == true) 
		rocket.AddRelativeForce (0, thrust, 0, ForceMode.Force);
			}


	private void EngineStart()
	{
		
		if (Input.GetKeyDown ("up")) 
		{
			
			StartCoroutine (LiftOff());
			foreach (var flame in flames) 
			{
				flame.Play ();
			}
			engineSource.Play ();
		
		}
	}
	IEnumerator LiftOff()
	{
		yield return new WaitForSeconds(3);
		EngineOn = true;
	}

}
