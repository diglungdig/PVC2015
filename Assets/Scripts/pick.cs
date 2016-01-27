using UnityEngine;
using System.Collections;

public class pick : MonoBehaviour {
	
	private GameObject item;
	private RaycastHit hit;
	private Camera mainCamera;
	public static bool carrying;
	//private static bool it;
	public float distance;
	public float smooth;

	void Start(){
		mainCamera = gameObject.GetComponentInChildren<Camera> ();
		carrying = false;
	}

	// Update is called once per frame
	void FixedUpdate () {
		//if(Physics.Raycast(transform.position, transform.forward,))
		if (carrying) {
			//carry using Lerp every frame
			//it = collcheck.itcollide;
			//if(it == false){

			item.transform.position = Vector3.Lerp (item.transform.position, mainCamera.transform.position + mainCamera.transform.forward * distance, Time.deltaTime * smooth);
			//}
			if (Input.GetMouseButtonDown (1)) {
				carrying = false;
				item.gameObject.GetComponent<Rigidbody> ().isKinematic = false;
				item = null;
			}
		} else {
			if (Input.GetMouseButtonDown (1)) {
				Debug.Log("HHHHERE");
				pickit ();
			}
		}
	
	}
	void pickit(){
		//if(Input.GetMouseButton(1)){
			int x = Screen.width/2;
			int y = Screen.height/2;

			Ray ray = mainCamera.ScreenPointToRay(new Vector3(x,y));
			RaycastHit hit;
			Debug.Log("Here!!");
			
			if(Physics.Raycast(ray, out hit)){
				
				item = hit.collider.gameObject;
				Debug.Log("here");

				if(item != null && item.tag == "pickable"){
					carrying = true;
					item.GetComponent<Rigidbody>().isKinematic = false;
					
					//item.transform.SetParent(transform);
				}

			}

		

	//}
}
}