using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.FirstPerson;

public class GrabAndDrop : MonoBehaviour {
	GameObject grabbedObject = null;
	float grabbedObjectSize;
	float objectDistance = 0f;
	GameObject inspectSystem;

	public GameObject fpsController;
	public bool debugLerpTest;
	void Start(){
		inspectSystem = GameObject.Find ("InspectSystem");

		//below seems nonsense, but it actually fixes a weird bug happened on raycast against inspect system
		fpsController.GetComponent<FirstPersonController> ().enabled = false;
		fpsController.GetComponent<FirstPersonController> ().enabled = true;

	}

	GameObject GetMouseHoverObject(float distance) {

		RaycastHit raycastHit;
		Ray traget = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

		if (Physics.Raycast (traget, out raycastHit, distance)) {
			if(raycastHit.collider.gameObject.tag == "grabable"){
				Debug.LogWarning(raycastHit.collider.gameObject.name);
				return raycastHit.collider.gameObject;
			}
		}
		return null;
	}

	void TryGrabObject(GameObject grabObject) {

		if (grabObject == null || !CanGrab (grabObject)) {
			return;
		} else if (Input.GetMouseButtonDown (0)) {

			grabbedObject = grabObject;
			grabbedObjectSize = grabObject.GetComponent<Renderer> ().bounds.size.magnitude + 0.5f;
			objectDistance = grabbedObjectSize*1.5f;


			//coroutine fetch animation
			if(grabObject != null){
				Vector3 p = grabbedObject.transform.position;

				Debug.LogWarning("this is " +gameObject.transform.position);
				Debug.LogWarning("that is " + Camera.main.transform.position);
				if(!debugLerpTest){
					StartCoroutine(fetch(grabbedObject.transform, p, gameObject.transform.position));
				}
				else{
					grabbedObject.transform.position = gameObject.transform.position + Camera.main.transform.forward*objectDistance;
					grabbedObject.transform.SetParent(gameObject.transform);
				}
			}

			inspectSystem.GetComponent<inspectSystem>().SendMessage("startInspect", grabbedObject);
		}
	}

	bool CanGrab(GameObject candidate){

		return candidate.GetComponent<Rigidbody>() != null;
	}

	void DropObject() {
		if (grabbedObject == null)
			return;

		if (grabbedObject.GetComponent<Rigidbody> () != null && Input.GetMouseButtonDown(1)) {

			inspectSystem.GetComponent<inspectSystem>().SendMessage("endInspect",grabbedObject);
			grabbedObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
			grabbedObject.transform.SetParent(null);
			grabbedObject = null;

		}

	}
		

	void Update () {

		if (grabbedObject == null) {
			TryGrabObject (GetMouseHoverObject (5.0f));
		}
		else{
			DropObject ();
		}

		if(grabbedObject != null) {
			//store item into inventory 
			if(Input.GetKeyDown(KeyCode.E)){

			}

			grabbedObject.GetComponent<Rigidbody>().isKinematic = true;

	}
	
}
	IEnumerator fetch(Transform object1, Vector3 pos1, Vector3 pos2){
		float i = 0.0f;
		float rate = 1.0f / 0.2f;
		while (i < 1.0f || Vector3.Distance(gameObject.transform.position+Camera.main.transform.forward * objectDistance, grabbedObject.transform.position) > 0.2f) {
			i += Time.deltaTime * rate;
			object1.position = Vector3.Lerp(object1.position, pos2 + Camera.main.transform.forward*objectDistance, i);

			yield return null;
		}

		grabbedObject.transform.position = gameObject.transform.position + Camera.main.transform.forward*objectDistance;
		grabbedObject.transform.SetParent(gameObject.transform);
	}

}