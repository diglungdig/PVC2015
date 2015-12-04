using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class toggleScript : MonoBehaviour {
    //dataStorage ds;
    Toggle tg;
    public GameObject sphere;
    sphereScript ss;
    private bool toggle;

	public void doUpdate() {
        toggle = tg.isOn;
        ss.ToggleShadows(toggle);
    }
    public void DoAction() {
        tg = GetComponent<Toggle>();
        ss = sphere.GetComponent<sphereScript>();
        if (tg.isOn)
        {
            tg.isOn = false;
        }
        else
        {
            tg.isOn = true;
        }
        doUpdate();
    }
}
