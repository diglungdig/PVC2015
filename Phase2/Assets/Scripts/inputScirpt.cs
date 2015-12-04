using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class inputScirpt : MonoBehaviour {

	//private Button changeColorButton;
	// Use this for initialization
	public Text text;

	public bool playerClick = false;
	void Start () {
		//changeColorButton = GameObject.Find ("Button");
	}

	public void playerClicked(){
		playerClick = !playerClick;

	}

	// Update is called once per frame
	void Update () {
	
		if (playerClick) {
			text.color = Color.blue;
			text.fontSize = 20;
		} else {
			text.color = Color.black;

		}

	}




}
