using UnityEngine;
using System.Collections;

public class Key : MonoBehaviour {

	private bool PlayerisNear;

	void Start(){

		PlayerisNear = false;
	}

	void OnTriggerEnter(Collider something){

		if (something.name == "Player" || something.name == "OVRPlayerController") {
			PlayerisNear = true;

		}

	}
	void OnTriggerExit(Collider something){

		if (something.name == "Player" || something.name == "OVRPlayerController") {
			PlayerisNear = false;
			
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.F)&& PlayerisNear == true){

			gameObject.SetActive(false);
	}
}
}