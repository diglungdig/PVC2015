using UnityEngine;
using System.Collections;

public class RightBtn : MonoBehaviour {

    public GameObject sphere;
    public void DoAction() {
        sphere.SendMessage("Right");
    }
}
