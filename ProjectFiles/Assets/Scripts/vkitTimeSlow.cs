using UnityEngine;
using System.Collections;

public class vkitTimeSlow : MonoBehaviour {

	//the time slow sphere area
	public Transform sphere;
	public float totalTime = 15f;
	private Transform vkitholder;
    private Transform FPSController;

	void Start(){
		vkitholder = gameObject.transform.parent;
        FPSController = vkitholder.parent;
	}

	public void releaseVkit(){
		//gameObject.transform.SetParent (sphere);
		//approaching the center of the sphere
		StartCoroutine(startAnimating());
	}

	IEnumerator startAnimating(){
		//gameObject.transform.localPosition = Vector3.zero;
		while (Vector3.Distance (gameObject.transform.position, sphere.position) > 0.1f) {
			gameObject.transform.position = Vector3.Lerp (gameObject.transform.position, sphere.position, 0.2f);
			yield return null;
		}
		gameObject.transform.position = sphere.position;
		gameObject.transform.SetParent (null);
		while (totalTime > 0) {
			totalTime -= Time.fixedDeltaTime;
			gameObject.transform.Rotate (Vector3.left * Time.deltaTime * 640);
            //Debug.Log("what the fuck");
			yield return null;
		}
		totalTime = 15f;
        gameObject.transform.rotation = Quaternion.identity;
        while (Vector3.Distance (gameObject.transform.position, vkitholder.position) > 0.1f) {
			gameObject.transform.position = Vector3.Lerp (gameObject.transform.position,vkitholder.position, 0.1f);
			yield return null;
		}
        //reset everything
		gameObject.transform.SetParent (vkitholder);
        gameObject.transform.position = Vector3.zero;
        gameObject.transform.localRotation = Quaternion.identity;
        sphere.SetParent(FPSController);
        sphere.localPosition = new Vector3(0f,2f,10f);
        sphere.localRotation = Quaternion.identity;
        

	}


}
