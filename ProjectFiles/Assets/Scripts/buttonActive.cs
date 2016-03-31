using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class buttonActive : MonoBehaviour {

    public interfaceControl interControl;
    private bool actived = false;

    public enum buttonType { Map, Tools, Inventory};
    public buttonType thisButtonIs;

    public void activate()
    {
        actived = true;
    }
    public void disactivate()
    {
        actived = false;
    }

    public void setupBt()
    {
        GetComponent<Button>().onClick.AddListener(delegate { clickTheButton(); });
    }

    public void clickTheButton()
    {
        GetComponent<AudioSource>().Play();
    }

	// Update is called once per frame
	void Update () {
        if (actived && (Input.GetMouseButtonDown(0) || OVRInput.GetDown(OVRInput.Button.One)))
        {
            clickTheButton();
            if (thisButtonIs == buttonType.Map) { }
            //interControl.mouseClickOnMap();
            else if (thisButtonIs == buttonType.Inventory)
                interControl.mouseClickOnInventory();
            else if (thisButtonIs == buttonType.Tools) { }
                //interControl.mouseClickOnTools();
        }
	}
}
