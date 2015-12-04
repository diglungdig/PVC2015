using UnityEngine;
using System.Collections;

public class moviePlayback : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
		((MovieTexture)GetComponent<Renderer> ().material.mainTexture).Play ();
		((MovieTexture)GetComponent<Renderer> ().material.mainTexture).loop = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
