using UnityEngine;
using System.Collections;

public class collisionCheck : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("environment"))
        {
            Debug.Log("LOLOLODADSADAWDAD");
        }
    }


	// Update is called once per frame
	void Update () {
	
	}
}
