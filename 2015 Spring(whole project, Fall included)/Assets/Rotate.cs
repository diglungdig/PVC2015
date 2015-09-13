using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {

	void OnTriggerStay(Collider sth){


		if (sth.tag == "Player") {

			;

		}

		else if (sth.attachedRigidbody) {

			Vector3 pos = gameObject.transform.position;
			pos.y = pos.y + 1;
			sth.transform.position = pos;

			sth.attachedRigidbody.AddTorque(Vector3.up * 5);
			sth.attachedRigidbody.AddForce(Vector3.up * 2);


		}


	}
}
