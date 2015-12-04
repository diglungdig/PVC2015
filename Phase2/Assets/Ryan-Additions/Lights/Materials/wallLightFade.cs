using UnityEngine;
using System.Collections;

public class wallLightFade : MonoBehaviour {
    public bool isEmergency = false;
    public bool isNormal = true;
    public Light light;
    public float IntensityHigh;
    public float IntensityLow;
    public float interpStep = 1.2f;

    private bool emergencyActive = false;
	// Use this for initialization
	void Start () {
	    if((isEmergency && isNormal) || (isEmergency && !isNormal)) {
            isNormal = false;
            Debug.Log("Starting doEmergency");
            StartCoroutine(doEmergency());
        }
        if(isNormal) {
            light.color = new Color(1, 1, 1);
        }
        if (isEmergency) {
            light.color = new Color(1, 0, 0);
        }
    }
    public void status(int condition) {
        //<0 Emergency, >= 0 Normal
        if(condition < 0) {
            if(!emergencyActive) {
                isNormal = false;
                StartCoroutine(doEmergency());
            }
        }
        else if(condition >= 0) {
            if(emergencyActive) {
                isNormal = true;
                StopCoroutine(doEmergency());
                emergencyActive = false;
            }
        }
    }
    IEnumerator doEmergency() {
        emergencyActive = true;
        while(emergencyActive == true) {
            //Go from low to high
            light.intensity = IntensityLow;
            bool isHigh = false;
            bool isLow = true;
            while (isLow) {
                light.intensity = light.intensity * interpStep;
                Mathf.Clamp(light.intensity, IntensityLow, IntensityHigh);
                if (light.intensity == IntensityHigh) {
                    isLow = false;
                    isHigh = true;
                }
                if (light.intensity > IntensityHigh) {
                    //IF clamp fails
                    isLow = false;
                    isHigh = true;
                    light.intensity = IntensityHigh;
                }
                yield return new WaitForSeconds(0.1f);
            }
            //Go from High to low
            while (isHigh) {
                light.intensity = light.intensity / interpStep;
                Mathf.Clamp(light.intensity, IntensityLow, IntensityHigh);
                if (light.intensity == IntensityLow) {
                    isHigh = false;
                    isLow = true;
                }
                if (light.intensity < IntensityLow) {
                    //IF clamp fails
                    isHigh = false;
                    isLow = true;
                    light.intensity = IntensityLow;
                }
                yield return new WaitForSeconds(0.01f);
            }
        }
    }
}
