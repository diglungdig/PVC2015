using UnityEngine;
using System.Collections;

public class DoorOpen : MonoBehaviour {

	// Use this for initialization
	bool PlayerisNear = false;
	private Animator anim;
	private GameObject Key01;

	void Start(){
		anim = GetComponent<Animator>();
		anim.SetBool ("open", false);
		Key01 = GameObject.Find("Key");
	}

	void OnTriggerEnter(Collider something){
		if (something.name == "Player" || something.name == "OVRPlayerController") {
			Debug.Log("is a door!");
			PlayerisNear = true;
		}

	}
	void OnTriggerExit(Collider something){
		if (something.name == "Player" || something.name == "OVRPlayerController") {
			Debug.Log ("I left");
			PlayerisNear = false;
		}
	}

	void OnGUI(){

		if (PlayerisNear == true) {
			GUI.Box (new Rect (700, 200, 250, 25), "When you have a key, Press F to open");
		}
	}
	// Update is called once per frame
	void Update () {
		if(PlayerisNear == true && Input.GetKeyDown(KeyCode.F) && Key01.activeSelf == false){
			anim.SetBool("open", true);
	}
}
}
