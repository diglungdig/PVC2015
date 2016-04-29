using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HazardAudioPlayer : MonoBehaviour {

    public GameObject musicVisualizer;
    private AudioSource source;
    private Image thisImage;

    private RectTransform recTransform;
    void Start()
    {
        source = GetComponent<AudioSource>();
        thisImage = GetComponent<Image>();
        recTransform = GetComponent<RectTransform>();
    }

    public void attachClipAndPlay(AudioClip thatClip)
    {
        source.clip = thatClip;
        StartCoroutine(playTheClip()); 
    }
         

	IEnumerator playTheClip()
    {
        musicVisualizer.SetActive(true);
        thisImage.enabled = true;
        source.Play();

        Vector3 setPostitionUp = new Vector3(0f, 0.1f, 0f);
        Vector3 setPostitionDown = new Vector3(0f, -0.05f, 0f);
        Vector3 pos = setPostitionDown;
        float factor = 0.5f;

        while (source.isPlaying)
        {
            recTransform.localPosition = Vector3.Lerp(recTransform.localPosition, pos,Time.deltaTime*factor);

            if (Vector3.Distance(recTransform.localPosition, setPostitionUp) <= 0.05f)
            {
                pos = setPostitionDown;
                factor = 0.6f;
            }
            else if (Vector3.Distance(recTransform.localPosition, setPostitionDown) <= 0.05f)
            {
                pos = setPostitionUp;
                factor = 0.5f;
            }

            yield return null;
        }
        source.clip = null;
        recTransform.localPosition = Vector3.zero;
        musicVisualizer.SetActive(false);
        thisImage.enabled = false;
        yield return null;
    }
}
