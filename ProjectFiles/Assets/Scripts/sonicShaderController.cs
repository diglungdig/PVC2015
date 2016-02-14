using UnityEngine;
using System.Collections;

public class sonicShaderController : MonoBehaviour {
	//developed by Wei

	[Range (0f, 0.05f)]
	public float value = 0f;
	public float vel = 0.05f;

	//if the light can breath or not
	public bool breathing = true;

	//if the light is turned on or not
	public bool switchOnOff = true;

	//apply a "turn on" flashing animation
	public bool animatedTurningOn = true;

	public Material thisMaterial;

	void Update () {
		
		if (switchOnOff) {
			if (breathing) {
				if (value >= 0.05f) {
					value = 0.05f;
					vel = -0.05f;
				} else if (value <= 0f) {
					value = 0f;
					vel = 0.05f;
				}
				value = value + Time.deltaTime * vel;
				thisMaterial.SetFloat ("_EmissionGain", 0.48f + value);
		
			}
		} 
	}

	public void setLightOn(){
		if (animatedTurningOn) {
			StartCoroutine (turningOnAnimation());
		} else {
			switchOnOff = true;
			thisMaterial.SetFloat ("_EmissionGain", 0.48f);
		}
	}
	public void setLightOff(){
		switchOnOff = false;
		thisMaterial.SetFloat ("_EmissionGain", 0f);
	}

	IEnumerator turningOnAnimation(){
		//GetComponent<Animation> ().Play();
		GetComponent<Animator> ().SetTrigger ("lightOn");
		yield return new WaitForSeconds(GetComponent<Animation> ().clip.length);
		switchOnOff = true;
		thisMaterial.SetFloat ("_EmissionGain", 0.48f);
	}
}
