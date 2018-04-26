using UnityEngine;
using InControl;

namespace GameLab.Tennis4Two {
    
    public static class InputHandler
    {
        private static bool debug;

        public static void Init(bool debugInput = false)
        {
            debug = debugInput;

            if (debug) {
                InputManager.OnDeviceAttached += inputDevice => Debug.Log("Attached: " + inputDevice.Name);
                InputManager.OnDeviceDetached += inputDevice => Debug.Log("Detached: " + inputDevice.Name);
                InputManager.OnActiveDeviceChanged += inputDevice => Debug.Log("Active device changed to: " + inputDevice.Name);
            }
        }

        public static Vector2 GetLeftStickAxis(int player)
        {
            if (PlayerDeviceAttached(player)) {
                return InputManager.Devices[player].LeftStick.Vector;
            }

            return Vector2.zero;
        }

        public static bool GetHitButton(int player)
        {
            if (PlayerDeviceAttached(player)) {
                return InputManager.Devices[player].Action1.WasPressed;
            }

            return false;
        }

        private static bool PlayerDeviceAttached(int player)
        {
            if (InputManager.Devices.Count > player) {
                return true;
            } else {
                if (debug) Debug.LogWarning("InputHandler: Device " + player + " is not attached");
                return false;
            }
        }

    }
}


