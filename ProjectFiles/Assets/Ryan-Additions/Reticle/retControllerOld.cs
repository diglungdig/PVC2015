using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class retControllerOld : MonoBehaviour {
    public Image innerOutline;
    public Image progress;
    public Image fullOutline;
    public Image glow;
    public float progValue;
    public bool isGlowing = false;
    public bool onObject = false;
    ArrayList instances = new ArrayList();
    //Testing purposes
    public Toggle objectToggle;
	// Use this for initialization
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
                case "fullOutline":
                    fullOutline = t;
                    break;
                case "glow":
                    glow = t;
                    break;
            }
        }
        //Disable glow and fullOutline
        glow.gameObject.SetActive(false);
        fullOutline.gameObject.SetActive(false);
        //StartCoroutine(test());
    }
    void Update()
    {
        //Old retController Script
        onObject = objectToggle.isOn;
        if(!isGlowing)
        {
            if(!objectToggle.isOn)
            {
                fullOutline.gameObject.SetActive(false);
                progress.fillAmount = 0;
            }
            else
            {
                fullOutline.gameObject.SetActive(true);
                progress.fillAmount = progValue;
            }
        }
        if(progValue == 1)
        {
            doSuccess();
        }
    }
    public void doSuccess()
    {
        StartCoroutine(successGlow());
        progValue = 0;
    }
    public void addInstance(MonoBehaviour x)
    {
        instances.Add(x);
    }
    public void progressValue(float x)
    {
        progValue = x;
    }
    IEnumerator successGlow()
    {
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
	IEnumerator test()
    {
        progress.fillAmount = 1;
        fullOutline.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        Debug.Log("Glow");
        StartCoroutine(successGlow());
        yield return new WaitForSeconds(3);
        Debug.Log("Glow");
        StartCoroutine(successGlow());
    }
}
