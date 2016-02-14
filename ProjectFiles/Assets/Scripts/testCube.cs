using UnityEngine;
using System.Collections;

public class testCube : MonoBehaviour {

	public GameObject mat;

	// Use this for initialization
	void Start () {
	
	}

	void OnTriggerEnter(Collider sth){
		if (sth.name == "FPSController") {
			mat.GetComponent<sonicShaderController> ().setLightOff ();
		}
	}

	void OnTriggerExit(Collider sth){
		if (sth.name == "FPSController") {
			mat.GetComponent<sonicShaderController> ().setLightOn ();
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
