using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class testingScript : MonoBehaviour {

	public GameObject button1;
	public GameObject button2;
	public GameObject button3;

	private Vector3 centerPoint = new Vector3(0.1f,0f,0f);
	private Vector3 pos1;
	private Vector3 pos2;
	private Vector3 pos3;

	void Start(){
		pos1 = button1.GetComponent<RectTransform> ().localPosition;
		pos2 = button2.GetComponent<RectTransform> ().localPosition;
		pos3 = button3.GetComponent<RectTransform> ().localPosition;
	}
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			button1.GetComponent<RectTransform> ().localPosition = centerPoint;
		}else if (Input.GetKeyUp (KeyCode.Alpha1)) {
			button1.GetComponent<RectTransform>().localPosition = pos1;
		}
		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			button2.GetComponent<RectTransform> ().localPosition = centerPoint;		
		}
		else if (Input.GetKeyUp (KeyCode.Alpha2)) {
			button2.GetComponent<RectTransform>().localPosition = pos2;
		}
		if (Input.GetKeyDown (KeyCode.Alpha3)) {
			button3.GetComponent<RectTransform> ().localPosition = centerPoint;		
		}
		else if (Input.GetKeyUp (KeyCode.Alpha3)) {
			button3.GetComponent<RectTransform>().localPosition = pos3;
		}
	}
}
