using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class blinkingLogo : MonoBehaviour {

    //public float speed = 2.0f;
    private Image thisImage;
    private bool hasStopped = true;
    private Material thismat;
	// Use this for initialization
	void Start () {
        thisImage = GetComponent<Image>();
        thismat = thisImage.material;
    }


    void Update()
    {
        thisImage = GetComponent<Image>();
        if (hasStopped)
        {
            StartCoroutine(blink());
        }
    }
	
    public IEnumerator blink()
    {
            hasStopped = false;
            thisImage.enabled = !thisImage.enabled;
            yield return new WaitForSeconds(.5f);
            hasStopped = true;       
    }
}
