using UnityEngine;
using System.Collections;

public class jumpScare01 : MonoBehaviour {

	public AudioSource scareYou;


	public MeshRenderer image;

	// Use this for initialization
	void Start () {
	
		scareYou = GetComponent<AudioSource> ();
		image = GetComponent<MeshRenderer> ();
	}


	void OnTriggerEnter(Collider sth){
		if (sth.name == "FPSController") {

			image.enabled = true;
			scareYou.Play();
			StartCoroutine(Delay());
		}

	}

	IEnumerator Delay(){

		yield return new WaitForSeconds (2.0f);
		image.enabled = false;
		scareYou.Stop ();


	}

	// Update is called once per frame
	void Update () {
	
	}
}
