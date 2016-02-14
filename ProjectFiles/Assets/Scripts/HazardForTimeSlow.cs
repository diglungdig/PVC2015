using UnityEngine;
using System.Collections;

public class HazardForTimeSlow : MonoBehaviour {

	public timeSlowController ctler;

	//an index num to make sure that the harzard will only be triggered once
	//it will be negated after the first use
	//timeslowController.triggerTheTimeSlow can only accept positive parameter to trigger the time slow mechanism
	[SerializeField]
	private int index = 1;

	void OnTriggerEnter(Collider sth){
		if (sth.tag == "Player") {
			ctler.triggerTheTimeSlow (index);
			index = -1;
		}
		//blah blah blah

	}
}
