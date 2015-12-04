using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class timer : MonoBehaviour {
	//should be placed in the highest hierarchy so that disenabling/enabling on gameObjects wont affect the timer;

	public float leftTimeInSecs;

	private int roundedLeftTime;
	private int minDisplayed;
	private int secDisplayed;
	private float time;
	private string text;
	public Text textComponent;
	// Use this for initialization
	void Awake(){

		time = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		leftTimeInSecs -= Time.deltaTime;
		roundedLeftTime = Mathf.CeilToInt (leftTimeInSecs);

		minDisplayed = roundedLeftTime / 60;
		secDisplayed = roundedLeftTime % 60;

		text = string.Format ("{0:00}:{1:00}",minDisplayed,secDisplayed);
		textComponent.text = text;
		//Debug.LogWarning (text);
	}
}
