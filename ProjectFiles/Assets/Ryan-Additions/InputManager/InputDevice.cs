using UnityEngine;
using System.Collections.Generic;
using XInputDotNetPure;
using System.Collections;
using System.Threading;


namespace PurdueVR.InputManager {
    public class InputDevice {

        public string inputName;
        public static string KEYBOARDMOUSE = "keyboard/mouse";
        public static string XBOX360 = "xbox360";
        public static string XBOXONE = "xboxOne";
        public static string UNSUPPORTED = "unsupported";
        public int buttonCount;
        public int axisCount;
        public bool hasRumble;
        //These should just be getters ugh
        public bool isXbox { get; set; }
        public bool isConnected { get; set; }
        public int DeviceID;
        public PlayerIndex currentIndex { get; set; }
        public GamePadState currentState;
        public GamePadState previousState;

        Thread checkThread;
        //managerMain manager;

        private bool checkAlive = true;

        public InputDevice(string inputName, int buttonCount, int axisCount, bool hasRumble) {
            this.inputName = inputName;
            this.buttonCount = buttonCount;
            this.axisCount = axisCount;
            this.hasRumble = hasRumble;
        }
        public InputDevice(string inputName) {
            if (inputName == KEYBOARDMOUSE) {
                //If you're asking why not use constructor chaining, I ask myself the same
                this.inputName = inputName;
                buttonCount = System.Enum.GetNames(typeof(KeyCode)).Length;
                axisCount = 2;  //X and Y
                hasRumble = false;
                isXbox = false;
                currentIndex = PlayerIndex.Four;
                isConnected = true;
            }
        }
        public InputDevice(PlayerIndex index) {
            inputName = "XboxController" + (int)index;
            buttonCount = 10;
            axisCount = 7;
            hasRumble = true;
            isXbox = true;
            isConnected = true;
            //Since it's not a monobehaviour, we have to thread the checker ourselves
            checkThread = new Thread(new ThreadStart(() => checkConnection(index, isConnected)));
            checkThread.Start();
        }
        public void dispose() {
            checkAlive = false;
            checkThread.Abort();
        }
        /*public void addManager(managerMain man) {
            manager = man;
        }*/
        public bool getConnected() {
            return isConnected;
        }
        void checkConnection(PlayerIndex playerIndex, bool isConnected) {
            while(checkAlive && managerMain.isPlaying) {
                GamePadState currentState = GamePad.GetState(playerIndex);
                if(currentState.IsConnected) {
                    if (!this.isConnected) {
                        Debug.Log("checkConnection: XboxController" + (int)playerIndex + " reconnected!");            
                    }
                    this.isConnected = true;
                }
                else {
                    if (this.isConnected) {
                        Debug.Log("checkConnection: XboxController" + (int)playerIndex + " disconnected!");
                    }
                    this.isConnected = false;
                }
                Thread.Sleep(250);
            }           
        }
        public void doConnectRumble() {
            Thread rumbleThread = new Thread(new ThreadStart(() => ConnectRumble()));
            rumbleThread.Start();
        }
        private void ConnectRumble() {
            for (int i = 0; i < 2; i++) {
                GamePad.SetVibration(currentIndex, 1, 1);
                Thread.Sleep(100);
                GamePad.SetVibration(currentIndex, 0, 0);
                Thread.Sleep(100);
            }         
        }
    }
}