using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GrabAndDrop : MonoBehaviour {
	GameObject grabbedObject;
	float grabbedObjectSize;
	float objectDistance = 0f;
	public float maxiumDistance = 5.0f; 
	public GameObject camera1;
	public inventorySys inventory;
    public AudioSource GAndD;
    public AudioClip[] aa;

    public functionCenter FuncCenter;

	bool grabCheck = false;
    private float tempFloat = 0f;

	bool distanceCheck(float maxDistance, GameObject theObject){
		if (Vector3.Distance (theObject.transform.position, camera1.transform.position) <= maxDistance) {
			return true;
		} else {
			return false;
		}
	}

	public void TryGrabObject(GameObject grabObject) {

		if (grabObject == null || !CanGrab (grabObject)|| grabObject.tag != "grabable"|| grabbedObject!=null ) {
			return;
		} else if(distanceCheck(maxiumDistance, grabObject)){

            if (grabObject.GetComponent<HighlightableObject>() != null)
            {
                grabObject.GetComponent<HighlightableObject>().enabled = false;
            }

			grabbedObject = grabObject;

            grabbedObjectSize = (grabObject.GetComponent<Renderer>() != null) ? grabObject.GetComponent<Renderer>().bounds.size.magnitude + 0.1f : 1f;
            //grabbedObjectSize = grabObject.GetComponent<Renderer>().bounds.size.magnitude + 0.1f;
            //Debug.Log(grabbedObjectSize);

            objectDistance = grabbedObjectSize*1.5f;
            grabObject.transform.SetParent(camera1.transform);

            GameObject.Find("InspectSystem").GetComponent<inspectSystem>().SendMessage("startInspect", grabbedObject);

			//coroutine fetch animation
			if(grabbedObject != null){
				StartCoroutine(fetch(grabbedObject.transform, camera1.transform.position,objectDistance));
			}
		}
	}

	bool CanGrab(GameObject candidate){

		return candidate.GetComponent<Rigidbody>() != null;
	}

	void DropObject() {


		if (grabbedObject == null)
			return;

		if (grabbedObject.GetComponent<Rigidbody> () != null && (Input.GetMouseButtonDown(1) || OVRInput.Get(OVRInput.Button.Two))) {

            if (grabbedObject.GetComponent<HighlightableObject>() != null)
            {
                grabbedObject.GetComponent<HighlightableObject>().enabled = true;
            }

            GameObject.Find("InspectSystem").GetComponent<inspectSystem>().SendMessage("endInspect",grabbedObject);
			grabbedObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;
            //grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
            grabbedObject.GetComponent<Rigidbody>().useGravity = true;
            grabbedObject.transform.SetParent(null);
			grabbedObject = null;
			grabCheck = false;
            
		}

	}
		

	void Update () {


		if(grabbedObject != null) {
            //store item into inventory 
            //grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
            grabbedObject.GetComponent<Rigidbody>().useGravity = false;
            //grabbedObject.transform.rotation = Quaternion.identity;
            grabbedObject.transform.position = Vector3.Lerp(grabbedObject.transform.position, Camera.main.transform.position+ Camera.main.transform.forward * objectDistance, Time.deltaTime*4f);

            if (Input.GetKeyDown(KeyCode.E) || OVRInput.Get(OVRInput.Button.Four))
            {
				Debug.Log ("press E!!@@!@");

                if (itemCheck(grabbedObject))
                {
                    GAndD.clip = aa[0];
                    GAndD.Play();
                    FuncCenter.startFunction(grabbedObject);
                    inventory.storeItem(grabbedObject);
                    //reset
                    GameObject.Find("InspectSystem").GetComponent<inspectSystem>().SendMessage("endInspect", grabbedObject);
                    grabbedObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
                    //grabbedObject.transform.SetParent(null);
                    //grabbedObject.GetComponent<Rigidbody>().useGravity = false;
                    grabbedObject = null;
                    grabCheck = false;
                }
                else
                {
                    //GAndD.clip = aa[1];
                   // GAndD.Play();
                }

			}

            //bring the object closer or farthur using mouse wheel
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                StartCoroutine(fetch(grabbedObject.transform, camera1.transform.position, objectDistance));
     
            }
            else if(Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                StartCoroutine(fetch(grabbedObject.transform, camera1.transform.position, objectDistance-0.8f));
      
            }

			DropObject ();

           

	}

    }
	IEnumerator fetch(Transform object1, Vector3 pos2, float distance){
		float i = 0.0f;
		float rate = 1.0f / 0.2f;
		while (i < 1.0f) {	
			object1.position = Vector3.Lerp(object1.position, pos2 + Camera.main.transform.forward*distance, i);
            i += Time.deltaTime * rate;
            //Debug.Log(Time.deltaTime);
            //Debug.LogWarning(object1.position);
            yield return null;
        }

    }
    bool itemCheck(GameObject grabedItem)
    {
        if (grabedItem.GetComponent<item>().thisType == item.Type.key)
            return true;

        else
            return false;       
    }

}