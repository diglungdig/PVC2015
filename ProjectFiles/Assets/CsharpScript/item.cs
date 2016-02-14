using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class item : MonoBehaviour {

	public enum Type
	{
		key, weapon,plain
	}

	public string description;
	public Type thisType;
    public Text detailedDesricption;

	//change the object's tag to pickable at the beginning

	void Start(){

		gameObject.tag = "grabable";
        if(GetComponent<Text>() != null)
        {
            detailedDesricption = GetComponent<Text>();
        }
	}
}
