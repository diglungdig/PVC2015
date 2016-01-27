using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class retController : MonoBehaviour {
    public Image innerOutline;
    public Image progress;
    
	//public Image fullOutline;
    
	public Image glow;

	public Image grab;
	public Image drag;

    public float progValue;
    public bool isGlowing = false;
    public bool onObject = false;
	
	public static bool inspecting = false; 

    //From reticleSnap
    GameObject objectOn;
    public int count = 0;
    public int CountDuration;
    public ArrayList instances = new ArrayList();
    //Item Popup
    public GameObject ItemPopup;
    //Testing purposes
    public Toggle objectToggle;
	// Use this for initialization

	private GameObject thelastobject = null;

	void Start () {
        Image[] ts = gameObject.GetComponentsInChildren<Image>();
        //Everything has to start out Enabled
        foreach (Image t in ts)
        {
            Debug.Log(t.gameObject.name);
            switch (t.gameObject.name)
            {
                case "innerOutline":
                    innerOutline = t;
                    break;
                case "progress":
                    progress = t; ;
                    break;
                case "glow":
                    glow = t;
                    break;
				case "grab":
					grab = t;
					break;
				case "drag":
					drag = t;
					break;
            }            
        }
        //Disable glow and fullOutline
        //glow.gameObject.SetActive(false);
        //progress.gameObject.SetActive(false);
		//Wei add
		grab.gameObject.SetActive (false);
		drag.gameObject.SetActive (false);
		//StartCoroutine(looker());
    }
    void Update()
    {

	
		//Physics Raycast
		Ray ray;
		RaycastHit hit;
		ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        if (!inspecting) {
			if (Physics.Raycast (ray, out hit, 5.0f, 6 << 6)){
				//Debug.Log("hit 000:" + hit.transform.gameObject.name);
				//Debug.Log("this is" + thelastobject.name);
				if (hit.transform.gameObject.layer == 8 && (thelastobject == null)) {
					//Debug.LogWarning("fail");
					objectOn = hit.transform.gameObject;
					onObject = true;

					if (objectOn.tag == "grabable") {
						grab.gameObject.SetActive (true);
						innerOutline.gameObject.SetActive (false);
						drag.gameObject.SetActive(false);
					} else {
						grab.gameObject.SetActive (false);
						innerOutline.gameObject.SetActive (true);
						drag.gameObject.SetActive(false);
					}

				}
			} else {
				innerOutline.gameObject.SetActive (true);
				grab.gameObject.SetActive (false);
				drag.gameObject.SetActive(false);
				objectOn = null;
				thelastobject = null;
				onObject = false;
			}
		} else {
			drag.gameObject.SetActive(true);
			grab.gameObject.SetActive(false);
		}
    }
	
	/*
	void UIraycastHandler(RaycastResult x){
	}
	*/

    IEnumerator successGlow()
    {
        //Controls Glowing animation
        isGlowing = true;
        progress.fillAmount = 1;
        glow.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.15f);
        glow.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.05f);
        glow.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.20f);
        glow.gameObject.SetActive(false);
        progress.fillAmount = 0;
        isGlowing = false;
    }
	
}
