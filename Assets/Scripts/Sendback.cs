using UnityEngine;
using System.Collections;

public class Sendback : MonoBehaviour {

	private GameObject item;
	private pick newpick;
	// Use this for initialization
	void Start () {
		newpick = GameObject.FindWithTag ("Player").GetComponentInChildren<pick> ();

	}

	void OnTriggerEnter(Collider sth){
		if (sth.tag == "pickable") {

			newpick.enabled = false;
			pick.carrying = false;

			Vector3 pos = new Vector3(12f,1.2f,7f);
			sth.gameObject.transform.position = pos;
		}

	}

	void OnTriggerExit(Collider sth){

			newpick.enabled = true;

	}

	// Update is called once per frame
	void Update () {
	
	}
}
