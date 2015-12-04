using UnityEngine;
using System.Collections;

public class Hover : MonoBehaviour {

	Rigidbody sphere;
	Rigidbody fpscontroller;
	// Use this for initialization
	void Start () {
		sphere = GetComponent<Rigidbody> ();

	}
	
	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate(){

		Ray ray = new Ray (transform.position, -transform.up);
		RaycastHit hit;

		if(Physics.Raycast(ray, out hit, 2.5f)){

			float prop = (2.5f-hit.distance)/2.5f;
			Vector3 force = Vector3.up*prop*20f;
			sphere.AddForce(force, ForceMode.Acceleration);
		}

		sphere.AddRelativeTorque (new Vector3(0f,1f,0f));


	}

}
