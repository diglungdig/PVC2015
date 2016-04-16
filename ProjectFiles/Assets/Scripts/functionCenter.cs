using UnityEngine;
using System.Collections;

public class functionCenter : MonoBehaviour {

    public interfaceControl interfaceContl;
    public GameObject sysinterface;

    public GameObject Helmet;

    public timeSlowController timeContl;
    public GameObject timeSlowKit;

    public GameObject audioPlayer;

    public void startFunction(GameObject grabbed)
    {
        if(grabbed.GetComponent<item>().description == "Nostromo Kit Buddy")
        {
            //interfaceContl.enabled = true;
            sysinterface.SetActive(true);
        }
        else if(grabbed.GetComponent<item>().description == "Helmet")
        {
            Helmet.SetActive(true);
        }
        else if(grabbed.GetComponent<item>().description == "Nostromo Arc Dilation Machine")
        {
            timeContl.enabled = true;
            timeSlowKit.SetActive(true);
        }
        else if (grabbed.GetComponent<item>().description.Contains("Audio Log"))
        {
            audioPlayer.GetComponent<HazardAudioPlayer>().attachClipAndPlay(grabbed.GetComponent<AudioSource>().clip);
        }

    }
	
}
