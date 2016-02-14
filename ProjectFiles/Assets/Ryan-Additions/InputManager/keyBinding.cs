using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using XInputDotNetPure;

public class keyBinding : MonoBehaviour {

    public static List<InputButton> buttons = new List<InputButton>();
	public void Start()
    {
        Debug.Log("keyBinding:Awake() starting");

        addButton(new InputKey("Jump", KeyCode.Space, XboxKey.A)); //0
        addButton(new InputAxis("LookX", "Mouse X", XboxAxis.RightStickX)); //1
        addButton(new InputAxis("LookY", "Mouse Y", XboxAxis.RightStickY)); //2
        addButton(new InputAxis("Horizontal", "Horizontal", XboxAxis.LeftStickX)); //3
        addButton(new InputAxis("Vertical", "Vertical", XboxAxis.LeftStickY)); //4
        addButton(new InputKey("Sprint", KeyCode.LeftShift, XboxKey.LeftStick)); //5
        addButton(new InputKey("JetPack", KeyCode.J, XboxKey.X)); //6
        addButton(new InputKey("Ctrl", KeyCode.LeftControl, XboxKey.RightStick)); //7
        addButton(new InputAxis("ShiftAxis", "", XboxAxis.LeftTrigger)); //8
        addButton(new InputAxis("CtrlAxis", "", XboxAxis.RightTrigger)); //9

        addButton(new InputKey("Pick", KeyCode.Mouse0, XboxKey.A));  //10
        addButton(new InputKey("Drop", KeyCode.Mouse1, XboxKey.B)); //11
        addButton(new InputKey("Tab", KeyCode.Tab, XboxKey.RightShoulder)); //12
        addButton(new InputKey("E key", KeyCode.E, XboxKey.Y));//13
        addButton(new InputKey("X Key", KeyCode.X, XboxKey.X));//14
    }
    public static void addButton(InputButton button) {
        buttons.Add(button);
    }
    public static List<InputButton> getButtons() {
        return buttons;
    }
    public static void printButtons()
    {
        Debug.Log("printButtons(): ");
        foreach (InputButton button in buttons)
        {
            Debug.Log(button.name);
        }
    }
    public static bool rebindButton(InputButton button) {
        string buttonName = button.name;
        InputButton modifyButton = null;
        foreach (InputButton inButton in buttons) {
            if (inButton.name == buttonName) {
                modifyButton = inButton;
                break;
            }
        }
        if (modifyButton == null) {
            return false;
        }
        else {
            if (button.isKey) {
                if (button.inputKey.lookupName) {
                   if (modifyButton.inputKey == null) {
                        Debug.LogError("rebindButton(): tried to assign an axis to a key");
                        return false;
                    }
                   else {
                        modifyButton.inputKey.setKeyCode(button.inputKey.getKeyCode());
                        modifyButton.inputKey.setXboxKey(button.inputKey.getXboxKey());
                        return true;
                    }
                }
                else {
                    if (modifyButton.inputKey == null) {
                        Debug.LogError("rebindButton(): tried to assign an axis to a key");
                        return false;
                    }
                    else {
                        modifyButton.inputKey.setKeyboardKey(button.inputKey.getKeyboardKey());
                        modifyButton.inputKey.setXboxKey(button.inputKey.getXboxKey());
                        return true;
                     }
                }
            }
            else if (!button.isKey) {
                if(button.inputAxis == null) {
                    Debug.LogError("rebindButton(): tried to assign a key to an axis");
                    return false;
                }
                else {
                    modifyButton.inputAxis.setMouseAxis(button.inputAxis.getMouseAxis());
                    modifyButton.inputAxis.setXboxAxis(button.inputAxis.getXboxAxis());
                    return true;
                }
            }
        }
        return false;
    }
}
public class InputButton {
    public string name;
    public void setName(string name) {
        this.name = name;
    }
    public bool isKey = false;
    public InputKey inputKey;
    public InputAxis inputAxis;
}
public enum XboxKey {
    //http://wiki.unity3d.com/index.php?title=Xbox360Controller
    A = 0,
    B = 1,
    X = 2,
    Y = 3,
    LeftShoulder = 4,
    RightShoulder = 5,
    Back = 6,
    Start = 7,
    LeftStick = 8,
    RightStick = 9,
    Guide = 10,
    DPadLeft = 11,
    DPadRight = 12,
    DPadDown = 13,
    DPadUp = 14,
}
public enum XboxAxis {
    //http://wiki.unity3d.com/index.php?title=Xbox360Controller with slight change
    LeftStickX = 1,
    LeftStickY = 2,
    RightTrigger = 3,
    LeftTrigger = 4,
    RightStickX = 5,
    RightStickY = 6,
}
public class InputKey : InputButton {
    KeyCode keyCode;
    string keyboardKey;         //To Look up
    XboxKey xboxKey;
    public bool lookupName;
    public InputKey(string name, KeyCode keyCode, XboxKey xboxKey) {
        setName(name);
        this.keyCode = keyCode;
        this.xboxKey = xboxKey;
        lookupName = false;
        isKey = true;
        inputKey = this;
    }
    public InputKey(string name, string keyboardKey, XboxKey xboxKey) {
        setName(name);
        this.keyboardKey = keyboardKey;
        this.xboxKey = xboxKey;
        lookupName = true;
        isKey = true;
        inputKey = this;
    }
    public void setKeyCode(KeyCode keyCode) { this.keyCode = keyCode; }
    public void setKeyboardKey(string keyboardKey) { this.keyboardKey = keyboardKey; }
    public void setXboxKey(XboxKey xboxKey) { this.xboxKey = xboxKey; }
    public KeyCode getKeyCode() { return this.keyCode; }
    public string getKeyboardKey() { return keyboardKey; }
    public XboxKey getXboxKey() { return xboxKey; }
}
public class InputAxis : InputButton {
    string mouseAxis;
    XboxAxis xboxAxis;
    public InputAxis(string name, string mouseAxis, XboxAxis xboxAxis) {
        setName(name);
        this.mouseAxis = mouseAxis;
        this.xboxAxis = xboxAxis;
        isKey = false;
        inputAxis = this;
    }
    public void setMouseAxis(string mouseAxis) { this.mouseAxis = mouseAxis; }
    public void setXboxAxis(XboxAxis xboxAxis) { this.xboxAxis = xboxAxis; }
    public string getMouseAxis() { return mouseAxis; }
    public XboxAxis getXboxAxis() { return xboxAxis; }
}
