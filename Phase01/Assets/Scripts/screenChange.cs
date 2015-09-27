using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class screenChange : MonoBehaviour {

	public Renderer rend;
	public float speed = 0.5f;

	private float duration = 2.0f;
	private bool PlayerPressE = false;
	private bool PlayerIsNear = false;
	private Text text;

	// Use this for initialization
	void Start () {
	
		rend = GetComponentInChildren<Renderer> ();
		text = GameObject.Find ("Text").GetComponent<Text> ();
			
	}


	void OnTriggerEnter(Collider sth){
		if (sth.name == "FPSController") {
			PlayerIsNear = true;
			text.text = "Press E";
		}

	}

	void OnTriggerExit(Collider sth){

		if (sth.name == "FPSController") {
			PlayerIsNear = false;
			text.text = "";
		}
	}


	// Update is called once per frame
	void Update () {

		if (PlayerIsNear && Input.GetKeyDown (KeyCode.E)) {
			PlayerPressE = !PlayerPressE;
		}


		if (PlayerPressE && rend.materials [1].GetFloat ("_Cutoff") < 0.99f) {
				StartCoroutine(changeColor(1f));


		}
		else if(!PlayerPressE && rend.materials [1].GetFloat ("_Cutoff") > 0.7f){
				StartCoroutine(changeColor(0.7f));

		}
	}


	IEnumerator changeColor(float a2){

		//float lerp = Mathf.PingPong (Time.time, duration) / duration;
		rend.materials [1].SetFloat ("_Cutoff", Mathf.Lerp (rend.materials [1].GetFloat ("_Cutoff"), a2, 0.04f));
		yield return null;

	}


}

