using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class inputchange : MonoBehaviour {

	public Text text;

	private bool playerClicked = false;
	// Use this for initialization
	void Start () {
		
	}

	public void playerClick(){

		playerClicked = !playerClicked;
	}

	// Update is called once per frame
	void Update () {
	
		if (playerClicked) {

			text.color = Color.blue;
		}

	}
}
