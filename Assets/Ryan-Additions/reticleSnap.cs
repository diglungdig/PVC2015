using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class reticleSnap : MonoBehaviour {

    public SpriteRenderer spriteRen;
    public int CountDuration;
    public GameObject testobj;
    bool onObject;
    GameObject objectOn;
    public int count = 0;
    // Use this for initialization
    void Start () {
        //https://forums.oculus.com/viewtopic.php?t=16710
        try {
             GameObject obj = GameObject.Find("reticle");
             spriteRen = obj.GetComponent<SpriteRenderer>();
         }
         catch (System.Exception e) {
             Debug.LogError(e.Message);
         }
         StartCoroutine(looker());
        normal = spriteRen.color;
	}
    void Update() {
        Ray ray;
        RaycastHit hit;
        ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width/2,Screen.height/2,0));
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, 5 << 5)) {
            Debug.Log(hit.transform.gameObject.name);
            if(hit.transform.gameObject.layer == 5)
            {
                objectOn = hit.transform.gameObject;
                onObject = true;
            }
            else if(hit.transform.gameObject.layer == 12)
            {
                //Layer is "Pickables"

            }
            
        }
        else {
            objectOn = null;
            onObject = false;
        }
    }
    void test() {
        testScript ts = testobj.GetComponent<testScript>();
        ts.SendMessage("Test");
    }
    void ObjInvoke(GameObject obj) {
        //DoAction
        obj.SendMessage("DoAction");
    }
    void ObjHover(GameObject obj) {
        //DoHover
        obj.SendMessage("DoHover");
    }
    Color normal;
    int colorstatus = -1;
    //-1 Normal, 0 hover, 1 success
    void revertColor() {
        if(colorstatus == 0 || colorstatus == 1) {
            spriteRen.color = normal;
            colorstatus = -1;
        }
    }
	void hover() {
        if(colorstatus == -1 || colorstatus == 0) {
            spriteRen.color = new Color(1, 0, 0, 0.75f);
            colorstatus = 0;
        }
    }
	IEnumerator looker() {
        while(true) {
            count = 0;
            revertColor();
            while(onObject) {
                hover();
                yield return new WaitForSeconds(0.1f);
                count++;
                if(count == CountDuration) {
                    colorstatus = 1;
                    spriteRen.color = new Color(0, 1, 0, 0.75f);
                    ObjInvoke(objectOn);
                    yield return new WaitForSeconds(1);
                    onObject = false;
                    objectOn = null;
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
