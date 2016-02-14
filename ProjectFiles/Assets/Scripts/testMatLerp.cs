using UnityEngine;
using System.Collections;

public class testMatLerp : MonoBehaviour {
	/*
	public Material hologramMaterial;
	public Material originMaterial;

	private float duration = 2.0f;
	// Update is called once per frame
	void Update () {
		float lerp = Mathf.PingPong (Time.time, duration) / duration;
		GetComponent<Renderer> ().material.Lerp (originMaterial, hologramMaterial,1);

		GetComponent<Renderer>().materials[1].Lerp(originMaterial, hologramMaterial,1);
	}
	*/
	public Material mat1;
	public Material mat2;
	public float duration = 2.0f;
	public Renderer rend;
	void Start(){
		rend = GetComponent<Renderer> ();
		rend.materials[1] = mat1;
	}

	void Update(){
		float lerp = Mathf.PingPong (Time.time, duration) / duration;
		rend.materials[1].Lerp (mat1, mat2, lerp);
	}
	 
}
