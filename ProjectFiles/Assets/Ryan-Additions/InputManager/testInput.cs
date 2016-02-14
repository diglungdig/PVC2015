using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using PurdueVR.InputManager;

public class testInput : MonoBehaviour {
    //List<InputButton> buttons = new List<InputButton>();
    List<InputButton> buttons = keyBinding.buttons;
    public bool getKeyDown;
    public bool getKey;
    public bool getKeyUp;
    public float testy;
    public float testy2;

    public int keyDown = 0;
    public int keyUp = 0;
	// Use this for initialization
	void Start () {
        buttons.Add(new InputKey("Jump", KeyCode.Q, XboxKey.A));
        buttons.Add(new InputAxis("testy", "Mouse X", XboxAxis.LeftTrigger));
        buttons.Add(new InputAxis("testy2", "Mouse Y", XboxAxis.RightTrigger));
        keyBinding.printButtons();
    }
	
	// Update is called once per frame
	void Update () {
        getKeyDown = managerMain.GetKeyDown(buttons[0]);
        getKey = managerMain.GetKey(buttons[0]);
        getKeyUp = managerMain.GetKeyUp(buttons[0]);
        if (getKeyDown) { Debug.Log("getKeyDown"); keyDown++; }
        if (getKeyUp) { Debug.Log("getKeyUp"); keyUp++; }
        testy = managerMain.GetAxis(buttons[1]);
        testy2 = managerMain.GetAxis(buttons[2]);
	}
}
