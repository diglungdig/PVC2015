using UnityEngine;
using System.Collections;

public class Laser2 : MonoBehaviour {
	
	private LineRenderer lr;
	public bool cubeGet;
	
	// Use this for initialization
	void Start () {
		lr = GetComponent<LineRenderer>();
		cubeGet = false;
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit ra;
		
		if (Physics.Raycast (lr.transform.position, lr.transform.forward, out ra)) 
		{
			if(ra.collider){
				lr.SetPosition(1, new Vector3(0,0,ra.distance));
				if(ra.collider.gameObject.tag == "pickable"){
					cubeGet = true;
				}
		
		}else{
			lr.SetPosition(1,new Vector3(0,0,500));
			cubeGet = false;
		}
	}
}
}
