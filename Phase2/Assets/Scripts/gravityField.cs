using UnityEngine;
using System.Collections;

public class gravityField : MonoBehaviour {

	// Use this for initialization


	void OnTriggerStay(Collider sth){

		if (sth.attachedRigidbody) {

			sth.attachedRigidbody.AddForce(Vector3.up * 20);
		}
	}

	void OnTriggerEnter(Collider sth){

		if (sth.attachedRigidbody) {
			
			sth.attachedRigidbody.AddTorque(Vector3.up * 20);
		}
	}


}
