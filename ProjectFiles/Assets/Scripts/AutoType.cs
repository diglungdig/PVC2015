using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AutoType : MonoBehaviour
{

    public float letterPause = 0.05f;
    public AudioClip sound;

    string message;
    private Text thisText;
    private AudioSource thisAudio;

    // Use this for initialization
    void Start()
    {
        thisText = GetComponent<Text>();
        thisAudio = GetComponent<AudioSource>();
        message = thisText.text;
        thisText.text = "";
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        foreach (char letter in message.ToCharArray())
        {
            thisText.text += letter;
            if (sound && Random.Range(0f,1f) > 0.4f && letter != ' ')
                thisAudio.PlayOneShot(sound);
            //yield return 0;
            yield return new WaitForSeconds(letterPause);
        }
        yield return new WaitForSeconds(4f);
        GameObject.Find("CutSceneManager").SendMessage("disableGUICanvas");
        GameObject.Find("CutSceneManager").SendMessage("startCamFadingIn");
        GameObject.Find("CutSceneManager").SendMessage("resumeAllAnimations");
    }
}