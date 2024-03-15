using System;
using UnityEngine.InputSystem;

namespace LazyPan {
    public class InputRegister : Singleton<InputRegister> {
        public string Shift = "Player/Shift";
        public string Motion = "Player/Motion";
        public string LeftClick = "Player/MouseLeft";
        public string RightClick = "Player/RightClick";
        public string Tab = "Player/Tab";
        public string R = "Player/R";
        public string MouseRightPress = "Player/MouseRightPress";
        public string Space = "Player/Space";
        public string Console = "Global/Console";
        public string ESCAPE = "Global/Escape";

        private InputControls inputControls;

        public void Load(string actionName, Action<InputAction.CallbackContext> action) {
            if (inputControls == null) { 
                inputControls = new InputControls();
            }
            inputControls.Enable();
            inputControls.FindAction(actionName).started += action;
            inputControls.FindAction(actionName).performed += action;
            inputControls.FindAction(actionName).canceled += action;
        }

        public void UnLoad(string actionName, Action<InputAction.CallbackContext> action) {
            if (inputControls == null) { 
                inputControls = new InputControls();
            }
            inputControls.Enable();
            inputControls.FindAction(actionName).started -= action;
            inputControls.FindAction(actionName).performed -= action;
            inputControls.FindAction(actionName).canceled -= action;
        }

        public void Dispose(string actionName) {
            if (inputControls == null) { 
                inputControls = new InputControls();
            }
            inputControls.Enable();
            inputControls.FindAction(actionName).Dispose();
        }

        public void Dispose() {
            if (inputControls == null) {
                inputControls = new InputControls();
            }
            inputControls.Enable();
            inputControls.Dispose();
        }
    }
}