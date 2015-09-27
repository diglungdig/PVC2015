using UnityEngine;
using System.Collections;

public class collcheck : MonoBehaviour {

	public static bool itcollide = false;
	// Use this for initialization
	void Start () {
	
	}

	void OnCollisionEnter(Collision collision){

		itcollide = true;
	}

	public bool getitcollide(){

		return itcollide;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
