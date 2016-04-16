using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HazardAudioPlayer : MonoBehaviour {

    public GameObject musicVisualizer;
    private AudioSource source;
    private Image thisImage;
    void Start()
    {
        source = GetComponent<AudioSource>();
        thisImage = GetComponent<Image>();
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
        while (source.isPlaying)
        {
            yield return null;
        }
        source.clip = null;
        musicVisualizer.SetActive(false);
        thisImage.enabled = false;
        yield return null;
    }
}
