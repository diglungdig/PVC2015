using UnityEngine;
using System.Collections;

public class tree : MonoBehaviour {

	public tree thistwig;

	public float twiglength = 1.0f;
	public float spreadAngle = 30.0f;
	public float iterations = 4;
	public float splits = 3;

	// Use this for initialization
	void Start () {
		if (iterations > 0) {

			for(int i = 0; i < splits; i++){

				Quaternion currentRotation = Quaternion.LookRotation(transform.forward); 
				Quaternion randomRotation  = Random.rotation;
				Quaternion twigRotation = Quaternion.RotateTowards(currentRotation, randomRotation, Random.Range(0.0f, spreadAngle));

				tree anotherTwig = Instantiate(thistwig, transform.position+transform.forward * twiglength, twigRotation) as tree;
				anotherTwig.iterations--;
			}




		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
