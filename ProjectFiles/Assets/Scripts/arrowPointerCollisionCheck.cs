using UnityEngine;
using System.Collections;

public class arrowPointerCollisionCheck : MonoBehaviour {

    // Use this for initialization

    public Material originalMat;
    public Material newMat;

	void Start () {
        //originalMat = GetComponent<MeshRenderer>().material;
	}
	
	void OnTriggerEnter(Collider sth)
    {
        Destroy(GetComponent<MeshRenderer>().material);
        GetComponent<MeshRenderer>().material = newMat;
    }

    void OnTriggerExit(Collider sth)
    {
        Destroy(GetComponent<MeshRenderer>().material);
        GetComponent<MeshRenderer>().material = originalMat;
    }

}
