using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using UnityStandardAssets.Characters.FirstPerson;

public class inspectSystem : MonoBehaviour {

	// Use this for initialization
	bool startToInspect = false;

	bool occupied = false;

	private string info = null;
	private Text objectInfo;

	public GameObject shade;

	public GameObject fpsController;

	void Start(){
		objectInfo = GameObject.Find ("objectInfo").GetComponent<Text> ();
		fpsController = GameObject.Find("FPSController");
	}

	void startInspect(GameObject a){
		gameObject.GetComponent<Text> ().enabled = true;
		occupied = true;

		a.AddComponent<inspect>();
		info = a.GetComponent<item> ().description;

		objectInfo.text = info;

		startToInspect = true;
		fpsController.GetComponent<FirstPersonController> ().enabled = false;

	}
	void endInspect(GameObject a){
		gameObject.GetComponent<Text> ().enabled = false;
		startToInspect = false;
		Destroy (a.GetComponent<inspect>());
		occupied = false;

		fpsController.GetComponent<FirstPersonController> ().enabled = true;

	}

	void Update(){
		if (Input.GetKey (KeyCode.X) && occupied) {
			objectInfo.enabled = true;
			shade.SetActive(true);

		}
		else{
			objectInfo.enabled = false;
			shade.SetActive(false);
	}
	}
}
