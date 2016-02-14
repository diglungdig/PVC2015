using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class sliderTest : MonoBehaviour {
    public retControllerOld retC;
    Slider slider;
	// Use this for initialization
	void Start () {
        retC.addInstance(this);
        slider = this.GetComponent<Slider>();

	}
    void Update()
    {   
        if(!retC.isGlowing)
        {
            retC.progressValue(slider.value);
        }
        if(slider.value >= 0.95)
        {
            slider.value = 0;
        }
    }
    public void resetValue()
    {
        slider.value = 0;
    }
}
