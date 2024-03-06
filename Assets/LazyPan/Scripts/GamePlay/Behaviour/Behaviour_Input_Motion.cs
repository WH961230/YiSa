using UnityEngine;
using UnityEngine.InputSystem;

namespace LazyPan {
    public class Behaviour_Input_Motion : Behaviour {
        private Vector3 inputMotionValue;
        private CharacterController characterController;

        public Behaviour_Input_Motion(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            InputRegister.Instance.Load(InputRegister.Instance.Motion, MotionEvent);
            characterController = entity.Comp.Get<CharacterController>("CharacterController");
            Data.Instance.OnUpdateEvent.AddListener(OnUpdate);
        }

        private void MotionEvent(InputAction.CallbackContext obj) {
            if (!Data.Instance.CanControl) {
                return;
            }
            inputMotionValue = obj.ReadValue<Vector2>();
        }

        void OnUpdate() {
            PlayerMotion();
        }

        private void PlayerMotion() {
            if (Cond.Instance.GetCameraEntity() == null) {
                return;
            }

            Vector3 cameraForward = Cond.Instance.GetCameraEntity().Prefab.transform.forward;
            cameraForward.y = 0;
            entity.EntityData.BaseRuntimeData.CurMotionDir = Vector3.zero;
            entity.EntityData.BaseRuntimeData.CurMotionDir += cameraForward * inputMotionValue.y * 5f;
            entity.EntityData.BaseRuntimeData.CurMotionDir += Cond.Instance.GetCameraEntity().Prefab.transform.right * inputMotionValue.x * 5f;
            characterController.Move(entity.EntityData.BaseRuntimeData.CurMotionDir * Time.deltaTime * entity.EntityData.BaseRuntimeData.CurMotionSpeed);
            if (entity.EntityData.BaseRuntimeData.CurMotionDir != Vector3.zero) {
                entity.Comp.GetEvent("PlayerMotionEvent")?.Invoke();
            }
        }

        public override void Clear() {
            base.Clear();
            InputRegister.Instance.UnLoad(InputRegister.Instance.Motion, MotionEvent);
            Data.Instance.OnUpdateEvent.RemoveListener(OnUpdate);
        }
    }
}