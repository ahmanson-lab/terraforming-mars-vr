using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorStartOpen : MonoBehaviour {

    [HideInInspector]
    public bool isDoorOpen = false;

    // Use this for initialization
	void Start ()
    {
		if(GetComponent<Animator>() != null)
        {
            GetComponent<Animator>().SetBool("shouldOpen", true);
            isDoorOpen = true;
        }
	}
	
	
}
