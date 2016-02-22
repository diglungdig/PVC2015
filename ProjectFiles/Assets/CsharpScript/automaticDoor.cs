﻿using UnityEngine;
using System.Collections;

public class automaticDoor : MonoBehaviour {

	// Use this for initialization
	Animator autoDoor;


	void Start () {
		autoDoor = gameObject.GetComponent<Animator> ();
	}


	void OnTriggerEnter(Collider sth){
		if (sth.name == "FPSController") {
			//autoDoor.SetFloat("speed",2.0f);
			autoDoor.SetTrigger("newTrigger");
		}


	}

	void OnTriggerExit(Collider sth){

		if (sth.name == "FPSController") {
			//autoDoor.SetFloat("speed",-2.0f);
			autoDoor.SetTrigger("newTrigger");
				
		}


	}

	// Update is called once per frame
	void Update () {
	
	}
}