using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class cutSceneManager : MonoBehaviour {

    public Animator[] animators;

    void Awake()
    {
        foreach(Animator a in animators)
        {
            a.speed = 0f;
        }
    }


    public void disableGUICanvas()
    {
        GetComponentInChildren<Canvas>().enabled = false;
    }
    public void startCamFadingIn()
    {
        Camera.main.GetComponent<OVRScreenFade>().enabled = true;
    }
    public void resumeTimeLine()
    {
        Time.timeScale = 1f;
    }
    public void stopTimeLine()
    {
        Time.timeScale = 0f;
    }
    public void resumeAllAnimations()
    {
        foreach (Animator a in animators)
        {
            a.speed = 1f;
        }
    }
}
