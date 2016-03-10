using UnityEngine;
using System.Collections;

public class spaceShipAnimation : MonoBehaviour {


    public Animator shipController;
	// Update is called once per frame
	void Update () {
        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("done"))
        {
            shipController.SetTrigger("landShip");
        }
	}
}
