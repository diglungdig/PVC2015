using UnityEngine;
using System.Collections;

public class inventorySys : MonoBehaviour {
	//developed by Wei

	public GameObject[] slot = new GameObject[4];

	public Material newMaterial;

	public void storeItem(GameObject theObject){
		//switch shader & attach the animation at runtime

		if (theObject.GetComponent<Renderer>() != null && theObject.GetComponent<Renderer> ().materials.Length > 0) {
			Material[] mats = theObject.GetComponent<Renderer> ().materials;
			for (int i = 0; i < theObject.GetComponent<Renderer> ().materials.Length; i++) {
				newMaterial.SetTexture("_MainTex",theObject.GetComponent<Renderer>().materials[i].mainTexture);
				mats [i] = newMaterial;
			}
			theObject.GetComponent<Renderer> ().materials = mats;
			theObject.AddComponent<Animator> ();
			//theObject.GetComponent<Animator> ().runtimeAnimatorController = Resources.Load("Assets/storeIt") as RuntimeAnimatorController;

		} else if(theObject.GetComponent<Renderer>() != null) {

			newMaterial.SetTexture("_MainTex",theObject.GetComponent<Renderer>().material.mainTexture);
			theObject.GetComponent<Renderer> ().material = newMaterial;
		}
	
		//switch back shader??

		theObject.transform.SetParent (getSlot().transform);
		theObject.transform.localPosition = Vector3.zero;
		theObject.transform.localRotation = Quaternion.identity;
		theObject.transform.localScale = Vector3.one;
	}



	private GameObject getSlot(){
		//unfold the for loop to 4 if statements
		if (slot [0].transform.childCount == 0) {
			return slot [0];
		} else if (slot [1].transform.childCount == 0) {
			return slot [1];
		} else if (slot [2].transform.childCount == 0) {
			return slot [2];
		} else if (slot [3].transform.childCount == 0) {
			return slot [3];
		}
		return null;
	}
			
}
