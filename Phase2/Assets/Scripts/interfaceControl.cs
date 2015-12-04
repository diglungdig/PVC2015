using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class interfaceControl : MonoBehaviour {

	public GameObject panel;
	public GameObject mappanel;
	public GameObject itempanel;

	public Animator vkit;
	public Rigidbody vkitRig;
	public Text mountText;


	//make it false when using oculus to test
	public bool oculusDebug;

	public Transform parent;
	public Transform child;
	public Transform environment;
	public Transform character;
	public Transform mapPanel;

	public Animation panelFade;
	public Animation itemshown;

	private Vector3 pos;
	private Vector3 vkitLocalPos;
	private Vector3 vkitMountPos = new Vector3(0, 0.6f, 0);

	private Vector3 mapLocalPos;
	private Quaternion rot;
	private Quaternion vkitLocalRot;

	//boolean values
	private bool clickedMap = false;
	private static bool playerhoverMap = false;

	private bool clickedInventory = false;
	private static bool playerhoverInventory = false;

	private bool hitE;
	private bool playerClickedTab = false;
	private static bool animationlock = true;
	// Use this for initialization
	void Start () {

		pos = GameObject.Find ("sysInterface").GetComponent<Transform> ().localPosition;
		rot = GameObject.Find ("sysInterface").GetComponent<Transform> ().localRotation;

		vkitLocalPos = GameObject.Find ("vkit").GetComponent<Transform> ().localPosition;
		vkitLocalRot = GameObject.Find ("vkit").GetComponent<Transform> ().localRotation;

		mapLocalPos = mapPanel.localPosition;

		//Screen.lockCursor = true;
		//Screen.lockCursor = false;
	}


	//map button
	public void mouseHoverOnMap (){
		if (clickedInventory == false) {
			playerhoverMap = true;
			if (clickedMap == false) {
				panelFade.Play ();	
			}
		}
	}

	public void mouseExitOnMap(){
		if (clickedMap == false && clickedInventory == false) {
			playerhoverMap = false;
			panelFade.Play ("panelFadein");
		}

	}
	public void mouseClickOnMap(){
		clickedMap = !clickedMap;
		clickedInventory = false;
	}

	//inventory button
	public void mouseHoverOnInventory(){
		if (!itempanel.activeInHierarchy && clickedMap == false) {
			itempanel.SetActive (true);
			itemshown["cubeMove"].layer = 5;
			itemshown["cubeMove"].speed = 2.0f;
			itemshown["cubeRot"].speed = 0.5f;
			itemshown.Blend("cubeRot");
			itemshown.Play ();
			panelFade.Play ();	
		}
	}
	public void mouseExitOnInventory(){
		if (clickedInventory == false && clickedMap == false) {
			itemshown.Play ("cubeBack");
			panelFade.Play ("panelFadein");
			if (itempanel.activeInHierarchy) {
				StartCoroutine (Delay (0.5f));
			}
		}
	}
	public void mouseClickOnInventory(){
		clickedInventory = !clickedInventory;
		clickedMap = false;
	}
	public void mouseClickOnItem(int num){
		Debug.Log ("click on item " + num);		
	}

	IEnumerator Delay(float secs){
		yield return new WaitForSeconds (secs);	
		itempanel.SetActive (false);
	}



	// Update is called once per frame
	void Update () {

			if (Input.GetKeyDown (KeyCode.Tab)) {
				//open Vkit
				playerClickedTab = ! playerClickedTab;

				if(playerClickedTab){
					
					parent.GetComponent<FirstPersonController>().enabled = false;

					transform.SetParent(parent);
					
					if(!oculusDebug){
						Screen.lockCursor = true;
					}
				}
				else{
					parent.GetComponent<FirstPersonController>().enabled = true;
					transform.SetParent(child);
					Screen.lockCursor = false;
				}
				

				vkit.SetBool("playerHitTab", playerClickedTab);

			}

			if (vkit.GetCurrentAnimatorStateInfo (0).IsName ("hanging")) {
				//activate panel and set the parent to FPScontroler so that the y axis is freezed
				panel.SetActive (true);
				transform.SetParent(parent);

				transform.SetParent(character);
				mountText.enabled = true;
				transform.localPosition = Vector3.Lerp(transform.localPosition, vkitMountPos, 0.1f);
				transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.identity, 0.1f);


			} 
			else {
				//deactivate panel and set the parent back to camera
				panel.SetActive(false);
				transform.SetParent(child);	
				

				if(vkit.GetCurrentAnimatorStateInfo(0).IsName("default")){
					//reset position and rotation
					if(Vector3.Distance(transform.localPosition ,pos) < 0.01f){
						transform.localPosition = pos;
					}
					else{

						transform.localPosition = Vector3.Lerp(transform.localPosition, pos, 0.1f);
						vkit.transform.localPosition = Vector3.Lerp(vkit.transform.localPosition, vkitLocalPos, 0.1f);
					
					}
						transform.localRotation = rot;
						vkit.transform.localRotation = vkitLocalRot;
				}


		}
		if (playerhoverMap == true) {
			mapPanel.localPosition = Vector3.Lerp (mapPanel.localPosition, Vector3.zero, 0.2f);
			mappanel.SetActive (true);
		} else {
			mapPanel.localPosition = Vector3.Lerp (mapPanel.localPosition, mapLocalPos, 0.2f);

			if(Vector3.Distance(mapPanel.localPosition ,mapLocalPos) < 0.01f)
				mappanel.SetActive (false);
		}

	}
}
