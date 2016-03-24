using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class cutSceneManager : MonoBehaviour {

    public Animator[] animators;
    public GameObject cutSceneCam;

    public GameObject[] mainCamComponents;
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
        cutSceneCam.SetActive(false);
        foreach(GameObject a in mainCamComponents)
        {
            a.SetActive(true);
        }
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
