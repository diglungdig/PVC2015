using UnityEngine;
using System.Collections;

public class item : MonoBehaviour {

	public enum Type
	{
		key, weapon,plain
	}

	public string description;
	public Type thisType;

}
