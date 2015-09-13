using UnityEngine;
using System.Collections;

public class SwitchOnOff : MonoBehaviour {

	public Light lig;
	private bool PlayerisNear;
	// Use this for initialization
	void Start () {
		PlayerisNear = false;
	}

	void OnTriggerEnter(Collider sth){
		if (sth.name == "Player") {
			PlayerisNear = true;
		}

	}
	void OnTriggerExit(Collider sth){
		if (sth.name == "Player") {
			PlayerisNear = false;
		}
		
	}

	// Update is called once per frame
	void Update () {
		if (PlayerisNear == true && Input.GetKeyDown(KeyCode.E)) {
			lig.enabled = !lig.enabled;
		}

	}
}
