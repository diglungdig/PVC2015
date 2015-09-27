using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonListener : MonoBehaviour 
{
	public Text textSelected;

	public void MyClick (GameObject obj) 
	{
		Text text = obj.GetComponentInChildren<Text>();
		textSelected.text = "You selected " + (text != null ? text.text : obj.name);
		Debug.Log (textSelected.text);
	}
}
