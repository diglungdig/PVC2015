using UnityEngine;
using System.Collections;

public class DoorOpen2 : MonoBehaviour {
	
	private Animator anim;
	private Laser one;
	private Laser2 two;
	private Light l;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		anim.SetBool ("open", false);
		one = GameObject.Find ("Sphere").GetComponentInChildren<Laser> ();
		two = GameObject.Find ("Sphere 1").GetComponentInChildren<Laser2> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(one.cubeGet == true && two.cubeGet == true){
			Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!");
			anim.SetBool("open", true);
			l = GameObject.Find("FinalLight").GetComponent<Light>();
			l.enabled = true;
		
		}
	}
}
