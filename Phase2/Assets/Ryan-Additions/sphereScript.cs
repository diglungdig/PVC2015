using UnityEngine;
using System.Collections;

public class sphereScript : MonoBehaviour {

    Rigidbody r;
    ForceMode f = ForceMode.VelocityChange;
    // Use this for initialization
    void Start () {
        r = GetComponent<Rigidbody>();
	}
	void Left () {   
        r.AddRelativeTorque(new Vector3(0, 0, 5), f);
    }
    void Right() {
        r.AddRelativeTorque(new Vector3(0, 0, -5), f);
    }
    public void ToggleShadows(bool x) {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        if(!x) {
            mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
            Debug.Log("Shadows Only");
        }
        else if(x) {
            Debug.Log("Regular Rendering");
            mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        }
    }
}
