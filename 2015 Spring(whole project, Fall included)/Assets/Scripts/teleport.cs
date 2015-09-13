using UnityEngine;
using System.Collections;

public class teleport : MonoBehaviour {

	private GameObject gate;
	private bool playerIsNear;
	private GameObject ppl;
	private bool itemisNear;
	// Use this for initialization
	private float timer;
	private float timer2;
	void Start () {
		gate = GameObject.FindGameObjectWithTag("Tp Gate Destination");

		playerIsNear = false;
	}

	void OnTriggerEnter(Collider sth){

		if (sth.attachedRigidbody) {
			
			sth.attachedRigidbody.AddTorque(Vector3.up * 20);
		}

		/*
		WaitForSeconds (1);
		sth.transform.position = gate.transform.position;
		*/
		if ( sth.tag == "pickable") {

			playerIsNear = true;
			ppl = sth.gameObject;
			itemisNear = true;
		}

	}
	void OnTriggerStay(Collider sth){
		
		if (sth.attachedRigidbody) {
			
			sth.attachedRigidbody.AddForce(Vector3.up * 15);
		}
	}


	void OnTriggerExit(Collider sth)
	{
		if (sth.tag == "pickable") {
			
			playerIsNear = false;
			itemisNear = false;
		}


	}

	// Update is called once per frame


	void Update () {
	
		if (playerIsNear == true) {
			timer += Time.deltaTime;
		}
		if (timer >= 3.0f && itemisNear == true) {
			Vector3 pos = gate.transform.position;
			pos.y = pos.y + 1;
			ppl.transform.position = pos;
		}

	}

			
	
}