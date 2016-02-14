using UnityEngine;
using System.Collections;

public class temporayrSconto : MonoBehaviour {

    public GameObject a;
    public GameObject b;
    public GameObject c;

    private bool ff = false;
	// Use this for initialization
	void Start () {
        a.SetActive(false);
        b.SetActive(false);
        c.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {


            a.SetActive(ff);
            b.SetActive(ff);
            c.SetActive(ff);

        if(Input.GetKeyDown(KeyCode.E)){
            ff = true;
        }
	
	}
}
