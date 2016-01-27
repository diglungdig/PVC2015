using UnityEngine;
using System.Collections;

public class onmouse : MonoBehaviour {

	public Renderer rend;
	// Use this for initialization
	void Start () {
		rend = GetComponent<Renderer> ();
	}
	void OnMouseEnter() {
		rend.material.color = Color.red;
	}
	void OnMouseOver() {
		rend.material.color -= new Color(0.1F, 0, 0) * Time.deltaTime;
	}
	void OnMouseExit() {
		rend.material.color = Color.white;
	}
	// Update is called once per frame

}
