using UnityEngine;
using System.Collections;

public class audioLog : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().Play();
            }
            else
            {
                GetComponent<AudioSource>().Pause();
            }
        }
        Debug.Log("Ayy lmao");
    }
}
