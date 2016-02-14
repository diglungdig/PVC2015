using UnityEngine;
using System.Collections;

public class timeSlowController : MonoBehaviour {
    public GameObject slowField;
    public Animator slowAnimator;
    public Animator cameraAnimator;
    // Use this for initialization

    private bool trigger = false;
    private bool trigger2 = false;

    public AudioSource clocktick;
    public AudioSource slowMotion;
    public float totalTime = 15f;

	public vkitTimeSlow thisvkitTimeSlow;
	public Transform sphere;
	void Start () {
        trigger = slowAnimator.GetBool("fireUp");
        trigger2 = cameraAnimator.GetBool("fireUp");
    }

	//!!
	public void triggerTheTimeSlow (int index){
		if (index >= 0) {
			//trigger the time slow mechanism when palyer enters a certain area.
			trigger = true;
			sphere.SetParent (null);
			slowAnimator.SetBool ("fireUp", trigger);
			thisvkitTimeSlow.releaseVkit ();
			cameraAnimator.SetBool ("fireUp", trigger);
			StartCoroutine (playSlowMotionSBX (1.5f));
			clocktick.Play ();
		}
	}
	//##

	// Update is called once per frame
	void Update () {
		//!!
		//use keyboard to trigger the time slow, depreacated
        /*
        if(Input.GetKeyDown(KeyCode.Q) || Input.GetMouseButtonDown(1)) { 
            //do something
            trigger = !trigger;
            slowAnimator.SetBool("fireUp", trigger);
            cameraAnimator.SetBool("fireUp",trigger);
            StartCoroutine(playSlowMotionSBX(1.5f));
            clocktick.Play();
        }
		//##
        */

        if (trigger)
        {
            totalTime -= Time.fixedDeltaTime;
            clocktick.pitch += Time.deltaTime * 0.08f;
            
            if(totalTime <= 0)
            {
                trigger = false;
                slowAnimator.SetBool("fireUp", trigger);
                cameraAnimator.SetBool("fireUp", trigger);
                StartCoroutine(playSlowMotionSBX(-1.5f));
                clocktick.Stop();
                clocktick.pitch = 0.2f;
                totalTime = 15f;
            }

        }
	}

    IEnumerator playSlowMotionSBX(float speed)
    {
        slowMotion.pitch = speed;
        slowMotion.Play();
        yield return new WaitForSeconds(5f);
        slowMotion.Stop();

    }

}
