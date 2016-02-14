using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using UnityStandardAssets.Characters.FirstPerson;

public class inspectSystem : MonoBehaviour {

	// Use this for initialization
	bool startToInspect = false;

	bool occupied = false;

	private string info = null;
	private Text objectInfo;

	public GameObject shade;
    public Material grab;
    public Material drag;
    public GameObject quad;
    //public FirstPersonController fpsController;
    public OVRPlayerController ovr;

    private Material GAZE;

	void Start(){
		objectInfo = GameObject.Find ("objectInfo").GetComponent<Text> ();

	}

	void startInspect(GameObject a){
		occupied = true;

		a.AddComponent<inspect>();
		info = a.GetComponent<item> ().description;

		objectInfo.text = info;
		startToInspect = true;

        //switch the reticle sprite
        GAZE = quad.GetComponent<Renderer>().material;
        quad.GetComponent<Renderer>().material = grab;

	}
	void endInspect(GameObject a){
		startToInspect = false;
		Destroy (a.GetComponent<inspect>());
		occupied = false;
        if (GAZE != null)
        {
            quad.GetComponent<Renderer>().material = GAZE;
        }
    }

	void Update(){
		if ((Input.GetKey (KeyCode.X)|| OVRInput.Get(OVRInput.Button.PrimaryShoulder)) && occupied) {
			objectInfo.enabled = true;
			shade.SetActive(true);

		}
		else{
			objectInfo.enabled = false;
			shade.SetActive(false);
        }

        if ((Input.GetMouseButtonDown(0) || OVRInput.GetDown(OVRInput.Button.One)) && occupied)
        {
            quad.GetComponent<Renderer>().material = drag;
            ovr.enabled = false;
            //fpsController.enabled = false;
        }
        else if((Input.GetMouseButtonUp(0) || OVRInput.GetUp(OVRInput.Button.One)) && occupied)
        {
            quad.GetComponent<Renderer>().material = grab;
            //fpsController.enabled = true;
            ovr.enabled = true;
        }

	}
}
