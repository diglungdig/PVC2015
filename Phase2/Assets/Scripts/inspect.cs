using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class inspect : MonoBehaviour {

	public bool released = true;

	private bool hitinfo = false;
	private string info;

	//private GameObject retController;
	void Start () {
		info = gameObject.GetComponent<item> ().description;
		//retController = GameObject.Find("Reticle");

	}
	
	// Update is called once per frame
	void Update () {

	if (Input.GetKey (KeyCode.Mouse0) || released == false) {

			transform.Rotate (Input.GetAxis ("Mouse Y") * 10, -Input.GetAxis ("Mouse X") * 10, 0f, Space.World);

			retController.inspecting = true;
		}
		//transform.Rotate ();
		if (Input.GetKeyUp (KeyCode.Mouse0)) {
			released = true;

			retController.inspecting = false;
		}
	}
}
