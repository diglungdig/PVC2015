using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DilationValue : MonoBehaviour {

    private Text text;
    private bool lerpLock = false;
    private float newValue = 2.0f;
	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!lerpLock)
        {
            newValue = Random.Range(2.1f, 4.5f);
            lerpLock = true;
        }

        text.text = Mathf.Lerp(float.Parse(text.text),newValue,Time.deltaTime*0.5f).ToString();

        if(Mathf.Abs(newValue - float.Parse(text.text)) < 0.05f){
            lerpLock = false;

        }

	}
}
