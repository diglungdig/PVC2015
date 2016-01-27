using UnityEngine;
using System.Collections;

public class LeftBtn : MonoBehaviour {
    public GameObject sphere;
	public void DoAction() {
        sphere.SendMessage("Left");
    }
}
