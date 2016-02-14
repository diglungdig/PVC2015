using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class inspect : MonoBehaviour {

	public bool released = true;

	private bool hitinfo = false;
	private string info;
	private Transform mainCam;
	private Vector3 originPoint;
	void Start () {
		info = gameObject.GetComponent<item> ().description;
		mainCam = Camera.main.transform;
		originPoint = gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetAxis ("Mouse ScrollWheel") != 0) {
			transform.position = Vector3.Lerp (transform.position,mainCam.position,1/2);
		}

		if (Input.GetKey (KeyCode.Mouse0) || released == false) {
			transform.Rotate (-Input.GetAxis ("Mouse Y") * 10, -Input.GetAxis ("Mouse X") * 10, 0f, Space.World);
            retController.inspecting = true;
        }

        if(OVRInput.Get(OVRInput.Button.One) || released == false)
        {
            transform.Rotate(-OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).y*10, -OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).x * 10, 0f, Space.World);
        }
		if (Input.GetKeyUp (KeyCode.Mouse0) || OVRInput.GetUp(OVRInput.Button.One)) {
			released = true;
            retController.inspecting = false;
        }
	}
}
