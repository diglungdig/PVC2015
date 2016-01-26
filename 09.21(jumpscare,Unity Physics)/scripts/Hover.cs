using UnityEngine;
using System.Collections;

public class Hover : MonoBehaviour {

	Rigidbody sphere;
	float rotateSpeed = 5f;

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

		if(Physics.Raycast(ray, out hit, 5f)){

			float prop = (5f-hit.distance)/5f;
			Vector3 force = Vector3.up*prop*60f;
			sphere.AddForce(force, ForceMode.Force);

		}



		Debug.Log (sphere.rotation.y);
		if (Mathf.Abs(sphere.rotation.y) > 0.9f) {

			sphere.rotation.Set(0f,-Mathf.Sign(sphere.rotation.y)/10,0f,0f);
			Debug.Log("here");
			rotateSpeed = -rotateSpeed;
		}

		sphere.AddTorque (0f,rotateSpeed,0f);



	}

}
