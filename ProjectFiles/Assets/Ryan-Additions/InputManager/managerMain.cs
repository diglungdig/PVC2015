using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Threading;
using XInputDotNetPure;             //https://github.com/speps/XInputDotNet

namespace PurdueVR.InputManager {
    public class managerMain : MonoBehaviour {
        //Delegates for external 
        public delegate void inputDeviceChanged(InputDevice device);
        public delegate void inputDeviceAdded(InputDevice device);
        public delegate void inputDeviceRemoved(InputDevice device);
        struct preferences {
            public bool useController;                  //Prefer Controller over keyboard
            public bool attemptGenericController;       //Attempt to use a generic Controller layout for unsupported controllers
        }

        public static List<InputDevice> devices = new List<InputDevice>();
        public static InputDevice currentInput;
        public static InputDevice prevInput;
        public static bool isPlaying = Application.isPlaying;
        public bool isConnected;
        Thread updateThread;
        private preferences prefs = new preferences();
        
        void Awake() {
            addDevice(InputDevice.KEYBOARDMOUSE, 0);        
            prefs.useController = true;
            prefs.attemptGenericController = false;
            Debug.Log("Operating System: " + SystemInfo.operatingSystem);
            StartCoroutine(checkChange());
            StartCoroutine(update());
        }
        IEnumerator update() {
            while(isPlaying) {
                foreach (InputDevice device in devices) {
                    if (device.isXbox) {
                        device.previousState = device.currentState;
                        device.currentState = GamePad.GetState(device.currentIndex);
                    }
                }
                yield return new WaitForSeconds(0.001f);
            }
        }
        IEnumerator checkChange() {
            while (true) {
                isPlaying = Application.isPlaying;  //To give InputDevice.cs something nice to work with
                //Check if anything has been added
                for (int i = 0; i < 4; i++) {
                    bool inList = false;
                    GamePadState state = GamePad.GetState((PlayerIndex)i);
                    if (state.IsConnected) {
                        //Check if the Device is already in the list, if it is ignore it
                        foreach (InputDevice d in devices) {
                            if (d.currentIndex == (PlayerIndex)i) {
                                inList = true;
                                break;
                            }
                        }
                        if (!inList) {
                            addDevice("xbox", (PlayerIndex)i);
                        }
                    }
                }
                //Handle disconnects/reconnects
                correctInput();
                isConnected = currentInput.isConnected;
                yield return new WaitForSeconds(0.25f);
            }
        }
        void addDevice(string inputName, PlayerIndex index) {
            bool supported = true;
            bool controller = false;
            InputDevice newDevice = null;

            if (inputName == InputDevice.KEYBOARDMOUSE) {
                newDevice = new InputDevice(InputDevice.KEYBOARDMOUSE);
                System.Random r = new System.Random();
                newDevice.DeviceID = r.Next(0, 1000);
            }
            else if (inputName.Contains("xbox")) {
                Debug.Log("Connected an XBOX controller.");
                //Default to a 360 controller
                newDevice = new InputDevice(index);
                System.Random r = new System.Random();
                newDevice.DeviceID = r.Next(0, 1000);
                supported = true;
                controller = true;
            }
            else if (inputName.Contains("playstation")) {
                Debug.Log("Connected a Playstation controller. Not supported");
                supported = false;
                controller = true;
            }
            
            if (controller && supported) {
                //We can use this controller, make it the default input device
                if (currentInput != null) Debug.Log("currentInput: " + currentInput.inputName + ", isXbox: " + currentInput.isXbox);
                if (newDevice == null) {
                    Debug.LogError("FATAL: attempted to add a null InputDevice. Check supported Devices in addDevice()");
                }
                else if (prefs.useController && (currentInput == null || !currentInput.isXbox)) {
                    prevInput = currentInput;
                    currentInput = newDevice;
                    devices.Add(newDevice);
                    currentInput.doConnectRumble();
                    Debug.Log("Controller " + newDevice.inputName + " is now the default device");
                }
                else {
                    Debug.LogWarning("Failed to add Controller " + newDevice.currentIndex);
                }
            }
            else if (!controller && supported) {
                Debug.Log(newDevice.inputName + " is now the default device");
                devices.Add(newDevice);
                if (currentInput == null) currentInput = newDevice;
            }
        }
        void updateDevices() {
            foreach (InputDevice d in devices) {
                d.isConnected = d.getConnected();
            }
        }
        //If currentInput has disconnected, change it
        void correctInput() {
            if (!currentInput.isConnected) {
                Debug.Log("currentInput disconnected");
                if (prefs.useController) {
                    Debug.Log(currentInput.inputName + "disconnected");
                    InputDevice toUseDevice = null;
                    foreach (InputDevice device in devices) {
                        if (device.isConnected && device.inputName != InputDevice.KEYBOARDMOUSE) {
                            toUseDevice = device;
                        }
                    }
                    if (toUseDevice != null) {
                        //use the new controller
                        prevInput = currentInput;
                        currentInput = toUseDevice;
                        currentInput.isConnected = true;
                        Debug.Log("Controller" + currentInput.currentIndex + " is now the default input");
                        currentInput.doConnectRumble();
                    }
                    else {
                        //No controller was found, use
                        prevInput = currentInput;
                        currentInput = devices[0];
                        currentInput.isConnected = true;
                        Debug.Log("keyboard/mouse is now the default input");
                    }
                }
            }
            else if (prevInput!= null && prefs.useController && (prevInput.isConnected && prevInput.isXbox)) {
                //Previous controller has been reconnected, make it default
                if ((int)prevInput.currentIndex < (int)currentInput.currentIndex) {
                    InputDevice holder = prevInput;
                    prevInput = currentInput;
                    currentInput = holder;
                    Debug.Log("Controller" + currentInput.currentIndex + " is now the default input");
                    currentInput.doConnectRumble();
                    currentInput.isConnected = true;
                }
            }
        }
        //This is by far the most dumb thing I've done. NOREGRETS
        void OnDestroy() {
            isPlaying = false;
        }

        //Static retrieval methods
        public static InputDevice[] getInputs() {
            InputDevice[] deviceList = new InputDevice[devices.Count];
            for (int i = 0; i < devices.Count; i++) {
                deviceList[i] = devices[i];
            }
            return deviceList;
        }
        public static ButtonState getButtonState(XboxKey key, bool current) {
            switch(key) {
                case XboxKey.A:
                    if (current) { return currentInput.currentState.Buttons.A; }
                    else { return currentInput.previousState.Buttons.A; }
                case XboxKey.B:
                    if (current) { return currentInput.currentState.Buttons.B; }
                    else { return currentInput.previousState.Buttons.B; }
                case XboxKey.Back:
                    if (current) { return currentInput.currentState.Buttons.Back; }
                    else { return currentInput.previousState.Buttons.Back; }
                case XboxKey.Guide:
                    if (current) { return currentInput.currentState.Buttons.Guide; }
                    else { return currentInput.previousState.Buttons.Guide; }
                case XboxKey.LeftShoulder:
                    if (current) { return currentInput.currentState.Buttons.LeftShoulder; }
                    else { return currentInput.previousState.Buttons.LeftShoulder; }
                case XboxKey.LeftStick:
                    if (current) { return currentInput.currentState.Buttons.LeftStick; }
                    else { return currentInput.previousState.Buttons.LeftStick; }
                case XboxKey.RightShoulder:
                    if (current) { return currentInput.currentState.Buttons.RightShoulder; }
                    else { return currentInput.previousState.Buttons.RightShoulder; }
                case XboxKey.RightStick:
                    if (current) { return currentInput.currentState.Buttons.RightStick; }
                    else { return currentInput.previousState.Buttons.RightStick; }
                case XboxKey.Start:
                    if (current) { return currentInput.currentState.Buttons.Start; }
                    else { return currentInput.previousState.Buttons.Start; }
                case XboxKey.X:
                    if (current) { return currentInput.currentState.Buttons.X; }
                    else { return currentInput.previousState.Buttons.X; }
                case XboxKey.Y:
                    if (current) { return currentInput.currentState.Buttons.Y; }
                    else { return currentInput.previousState.Buttons.Y; }
                case XboxKey.DPadLeft:
                    if (current) { return currentInput.currentState.DPad.Left; }
                    else { return currentInput.previousState.DPad.Left; }
                case XboxKey.DPadRight:
                    if (current) { return currentInput.currentState.DPad.Right; }
                    else { return currentInput.previousState.DPad.Right; }
                case XboxKey.DPadDown:
                    if (current) { return currentInput.currentState.DPad.Down; }
                    else { return currentInput.previousState.DPad.Down; }
                case XboxKey.DPadUp:
                    if (current) { return currentInput.currentState.DPad.Up; }
                    else { return currentInput.previousState.DPad.Up; }
                default:
                    Debug.LogWarning("getButtonState(): could not find button");
                    return new ButtonState();
            }
        }
        public static float convertAxisState(XboxAxis axis)
        {
            switch (axis)
            {
                case XboxAxis.LeftStickX:
                    return currentInput.currentState.ThumbSticks.Left.X;
                case XboxAxis.LeftStickY:
                    return currentInput.currentState.ThumbSticks.Left.Y;
                case XboxAxis.RightTrigger:
                    return currentInput.currentState.Triggers.Right;
                case XboxAxis.LeftTrigger:
                    return currentInput.currentState.Triggers.Left;
                case XboxAxis.RightStickX:
                    return currentInput.currentState.ThumbSticks.Right.X;
                case XboxAxis.RightStickY:
                    return currentInput.currentState.ThumbSticks.Right.Y;
                default:
                    Debug.Log("convertAxisState(): could not find axis");
                    return 0;
            }
        }
        public static bool convertButtonState(ButtonState state) {
            int foo = (int)state;
            if (foo == 0) { return true; }
            else { return false; }
        }
        public static void printInputs() {
            InputDevice[] deviceList = getInputs();
            foreach (InputDevice device in deviceList) {
                Debug.Log(device.inputName);
            }
        }
        public static bool GetKeyDown(InputButton button) {
            if (currentInput == null) {
                return false;
            }
            else if (!button.isKey) {
                Debug.LogError("GetKeyDown(): tried to get a key down with an axis");
                return false;
            }
            else if (currentInput.isXbox) {
                ButtonState currentState = getButtonState(button.inputKey.getXboxKey(), true);
                ButtonState previousState = getButtonState(button.inputKey.getXboxKey(), false);
                                
                bool prev = convertButtonState(previousState);
                bool curr = convertButtonState(currentState);
                if (!prev && curr) { return true; }
                else { return false; }
            }
            else {
                if(button.inputKey.lookupName) { return Input.GetKeyDown(button.inputKey.getKeyboardKey()); }
                else { return Input.GetKeyDown(button.inputKey.getKeyCode()); }
            }
        }
        public static bool GetKey(InputButton button) {
            if (currentInput == null) {
                return false;
            }
            else if (!button.isKey) {
                Debug.LogError("GetKey(): tried to get a key down with an axis");
                return false;
            }
            else if (currentInput.isXbox) {
                ButtonState currentState = getButtonState(button.inputKey.getXboxKey(), true);
                return convertButtonState(currentState);                
            }
            else {
                if (button.inputKey.lookupName) { return Input.GetKey(button.inputKey.getKeyboardKey()); }
                else { return Input.GetKey(button.inputKey.getKeyCode()); }
            }
        }
        public static bool GetKeyUp(InputButton button) {
            if (currentInput == null) {
                return false;
            }
            else if (!button.isKey) {
                Debug.LogError("GetKeyDown(): tried to get a key down with an axis");
                return false;
            }
            else if (currentInput.isXbox) {
                ButtonState currentState = getButtonState(button.inputKey.getXboxKey(), true);
                ButtonState previousState = getButtonState(button.inputKey.getXboxKey(), false);

                bool prev = convertButtonState(previousState);
                bool curr = convertButtonState(currentState);
                if (prev && curr) { return false; }
                if (!prev && !curr) { return false; }
                if (prev && !curr) { return true; }
                else { return false; }
            }
            else {
                if (button.inputKey.lookupName) { return Input.GetKeyUp(button.inputKey.getKeyboardKey()); }
                else { return Input.GetKeyUp(button.inputKey.getKeyCode()); }
            }
        }
        public static float GetAxis(InputButton axis)
        {
            if (currentInput == null)
            {
                return 0;
            }
            else if (axis.isKey)
            {
                Debug.LogError("GetAxis(): tried to get an axis value with a key");
                return 0;
            }
            else if (currentInput.isXbox)
            {
                //XboxAxis getAxis = axis.inputAxis.getXboxAxis();
                /*if(getAxis == XboxAxis.LeftTrigger || getAxis == XboxAxis.RightTrigger)
                {
                    state = GamePad.GetState(currentInput.currentIndex, GamePadDeadZone.IndependentAxes);
                }
                else
                {
                    state = GamePad.GetState(currentInput.currentIndex, GamePadDeadZone.Circular);
                }*/
                return convertAxisState(axis.inputAxis.getXboxAxis());
            }
            else
            {
                return Input.GetAxis(axis.inputAxis.getMouseAxis());
            }
        }
        public static float GetAxisRaw(InputButton axis)
        {
            if (currentInput == null)
            {
                return 0;
            }
            else if (axis.isKey)
            {
                Debug.LogError("GetAxis(): tried to get an axis value with a key");
                return 0;
            }
            else if (currentInput.isXbox)
            {
                return convertAxisState(axis.inputAxis.getXboxAxis());
            }
            else
            {
                return Input.GetAxisRaw(axis.inputAxis.getMouseAxis());
            }
        }
    }
}
   