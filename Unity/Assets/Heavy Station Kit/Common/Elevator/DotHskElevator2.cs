using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class DotHskFloor : System.Object {
	public DotHskElevator2Console console = null;
	[Tooltip("To type in Underground floor number hold Shift (in Game mode)")][Range(-9,9)]
	public int floorNumber;
    public string floorTitle;
	#if UNITY_EDITOR
	[ReadOnlyAttribute] 
	#endif
	public float floorHeight;
}

public abstract class DotHskElevatorEventsClass : MonoBehaviour {
	[HideInInspector]public DotHskElevator2 elevator;
	public abstract void OnStart ();
	public abstract void OnStop ();
	public abstract void OnInit();
	public abstract void OnFloorChange (int floor, bool willMove, int dir);
	public abstract void OnActivateConsole(int floor);
	public abstract void OnDeactivateConsole(int floor);
}

[ExecuteInEditMode]
public class DotHskElevator2 : MonoBehaviour {

	//public int floor = 0;
	public string elevatorLabel = "N";

	// Floor Control Console Array
	public List<DotHskFloor> floors;

	public int currentFloor = 0;
	[HideInInspector]public int waitTime = 3;

	// Elevator Platform
	public Transform platform;
	public DotHskElevator2Console platformConsole;
	public AudioSource platformSoundSource = null;

	public float platformSpeed = 1f;
	[HideInInspector]public bool multiCallMode = false;

	// Onscreen tips (textures)
	public Texture2D callElevatorTip; 
	public Texture2D enterFloorTip; 

	// Elevator sounds
	public AudioClip startSound;
	public AudioClip stopSound;
	public AudioClip motionSound;

	// Hot Keys for elevator call
	public Dictionary<int,int> hKeys;

	[HideInInspector] public int targetFloor = 0;
	[HideInInspector] public DotHskElevatorEventsClass elevatorEvents;

	// Elevator Motor section
	private bool isIdle = true;
	private int dir = 1; // Moving direction
	private float h0 = 0f; // Acceleration distance
	private float h2 = 0f; // Stopping distance
	private Queue<int> queue;
	private int phase = 0; // Phase of movement (0 - starting, 1 - moving, 2 - stopping)
	private float[] H = {0f,0f,0f,0f}; // Endpoints of phases
	private float[] T = {0f,0f}; // Starting / Stopping timing
	private float phaseStart = 0f;
	private bool waitState = false;
	private float waitStart = 0f;
	bool[] floorHStats = new bool[19];

	void Start () {
		init ();
		motorInit ();
	}

	void FixedUpdate () {
		if (Application.isPlaying) {
			if (isIdle) {
				if (!motorWait (false) && (queue.Count > 0) ){
					targetFloor = queue.Dequeue ();
					motorStart();
				}
			} else {
				motorMove (Time.fixedTime, Time.fixedDeltaTime);
			}
		}
	}

	#if UNITY_EDITOR
	void Update () {
		if (!Application.isPlaying) {
			autoRestrict();
			init();
		}
	}
	#endif

	// Initialization

	void init(){
		// Setup elevatorBase property of attached consoles to this script object
		hKeys = new Dictionary<int,int> ();
		for (int i = 0; i < floors.Count; i++) {
			if (floors [i].console != null) {
				floors [i].console.elevatorBase = this;
				floors [i].console.thisFloor = i;
				floors [i].floorHeight = floors [i].console.transform.position.y;
				if (Application.isPlaying) {
					hKeys [floors [i].floorNumber] = i;
				}
			} else {
				floors [i].floorHeight = 0;
			}
		}
		// Setup platfom console
		if( platformConsole != null ){
			platformConsole.thisFloor = -1;
			platformConsole.elevatorBase = this;
		}
		// Move platform to the beginning position
		if (currentFloor < floors.Count) {
			platform.position = new Vector3 (
				platform.position.x,
				floors[currentFloor].floorHeight,
				platform.position.z
			);
		}
		// Set up Events
		if (elevatorEvents != null) {
			elevatorEvents.OnInit();
			elevatorEvents.OnFloorChange (currentFloor, false, 0);
		}
	}

	public bool call(int floor){
		if( (floor < 0) || (currentFloor == floor) || (floor >= floors.Count) || queue.Contains (floor) ) {
			return false;
		}
		queue.Enqueue(floor);
		return true;
	}

	// Motor methods

	private void motorInit(){
		// Start / Stop distances
		if (startSound != null){
			h0 = 0.5f * platformSpeed * (T[0] = startSound.length);
			if (T[0] > 0) { T[0] = 1f / T[0]; }
		};
		if (stopSound != null) {
			h2 = 0.5f * platformSpeed * (T[1] = stopSound.length);
			if (T[1] > 0) { T[1] = 1f / T[1]; }
		};
		// Attach event handler script
		elevatorEvents = GetComponent<DotHskElevatorEventsClass>();
		if (elevatorEvents != null) { elevatorEvents.elevator = this; }
		// Init Call Queue
		queue = new Queue<int> ();
	}

	private void motorStart(){
	    if(currentFloor != targetFloor){
			H[0] = floors [currentFloor].floorHeight;
			H[3] = floors [targetFloor].floorHeight;
			dir = (H[3] > H[0]) ? 1 : -1;
			H[1] = H[0] + dir * h0;
			H[2] = H[3] - dir * h2;
			phase = 0;
			phaseStart = Time.time;
		    isIdle = false;
			if (elevatorEvents != null) { elevatorEvents.OnStart (); }
			if (elevatorEvents != null) { elevatorEvents.OnFloorChange (currentFloor, true, dir); }
			updateCurrentFloor(H[0], dir, true);
			if ((platformSoundSource != null) && (startSound!=null)) {
				platformSoundSource.clip = startSound;
				platformSoundSource.time = 0f;
				platformSoundSource.Play();
			}
		}
	}

	private void motorMove(float fixedTime, float deltaTime){
		float currentY = platform.position.y;
		float newY = currentY;
		if (dir * (H [phase + 1] - currentY) < 0.01f) {
			// End of phase
			newY = H [phase + 1];
			if (phase < 2) {
				phaseStart = fixedTime;
				phase++;
				if ((platformSoundSource != null) && (startSound!=null)) {
					if (platformSoundSource.isPlaying) { platformSoundSource.Stop (); }
					platformSoundSource.clip = (platformSoundSource.loop = (phase == 1)) ? motionSound : stopSound;
					platformSoundSource.time = 0f;
					platformSoundSource.Play();
				}
			} else {
				isIdle = true;
				phase = 0;
				currentFloor = targetFloor;
				if (elevatorEvents != null) { elevatorEvents.OnFloorChange (targetFloor, false, dir); }
				if (elevatorEvents != null) { elevatorEvents.OnStop (); }
				if (platformConsole != null) { platformConsole.OnStop (); }
				motorWait (true);
			}
		} else {
			// Moving
			if (phase == 1) {
				newY += dir * platformSpeed * deltaTime;
			} else {
				float dt = (fixedTime - phaseStart);
				if (phase == 0) {
					newY = H [0] + 0.5f * dir * dt * dt * platformSpeed * T [0];
				} else {
					newY = H [2] + dir * (platformSpeed * dt - 0.5f * dt * dt * platformSpeed * T [1]);
				}
			}
		}
		if (newY != currentY) {
			platform.position = new Vector3 (platform.position.x, newY, platform.position.z);
			updateCurrentFloor(newY, dir,false);
		}
	}

	private bool motorWait(bool start){
		if (start) {
			waitStart = Time.time;
			waitState = true;
		} else {
			if (waitState && (Time.time - waitStart >= waitTime )) {
				waitState = false;
			}
		}
		return waitState;
	}

	private void updateCurrentFloor(float curH, int dir, bool init){
		bool prev;
		if(isIdle){ return; }
		for (int i = 0; i < floors.Count; i++) {
			prev = floorHStats [i];
			floorHStats [i] = (dir * floors [i].floorHeight > dir * curH);
			if ( (!init) && (prev != floorHStats [i]) ){
				currentFloor = i;
				if (elevatorEvents != null) { elevatorEvents.OnFloorChange (i,true,dir); }
			};
		}
	}

	// Editor support
	#if UNITY_EDITOR
	void autoRestrict(){
		// Limit floors count
		if (floors.Count > 19) { floors.RemoveRange(19,floors.Count-19); }
		// Current floor
		if (currentFloor < 0) {	currentFloor = 0; }
		if (currentFloor >= floors.Count) {	currentFloor = floors.Count - 1; }
		// Make unique numbering of floors
		List<int> buf = new List<int>();
		int cnt = floors.Count;
		int prev = 0;
		bool rev = false;
		for (int i = 0; i < cnt; i++) {
			if (buf.Contains (floors [i].floorNumber)) {
				if ((prev+1!=floors [i].floorNumber) && (prev < 9)) {
					floors [i].floorNumber = prev+1;
				} else {
					if (floors [i].floorNumber < 9) { floors [i].floorNumber++; } else { rev = true; break; }
				}
			}
			buf.Add (prev = floors [i].floorNumber);
		}
		if(rev){
			for (int i = cnt-1; i>=0; i--) {
				floors [i].floorNumber = 9 + i - (cnt-1);
			}
		}
	}
	#endif


}

