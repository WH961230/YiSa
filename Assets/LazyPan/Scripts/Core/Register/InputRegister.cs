using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LazyPan {
    public class InputRegister : Singleton<InputRegister> {
        public string Shift = "Player/Shift";
        public string Movement = "Player/Movement";
        public string LeftClick = "Player/LeftClick";
        public string RightClick = "Player/RightClick";
        public string Tab = "Player/Tab";
        private InputControls inputControls;
        public string R = "Player/R";

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
    }
}