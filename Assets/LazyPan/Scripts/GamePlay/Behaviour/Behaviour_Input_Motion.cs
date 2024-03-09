using UnityEngine;
using UnityEngine.InputSystem;

namespace LazyPan {
    public class Behaviour_Input_Motion : Behaviour {
        private Vector3 inputMotionValue;
        private CharacterController characterController;

        public Behaviour_Input_Motion(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            InputRegister.Instance.Load(InputRegister.Instance.Motion, MotionEvent);
            characterController = Cond.Instance.Get<CharacterController>(entity, Label.CHARACTERCONTROLLER);
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
            Camera camera = Cond.Instance.Get<Camera>(Cond.Instance.GetCameraEntity(), Label.CAMERA);
            if (camera == null) {
                return;
            }

            Vector3 cameraForward = camera.transform.forward;
            cameraForward.y = 0;
            entity.EntityData.BaseRuntimeData.CurMotionDir = Vector3.zero;
            entity.EntityData.BaseRuntimeData.CurMotionDir += cameraForward * inputMotionValue.y;
            entity.EntityData.BaseRuntimeData.CurMotionDir += camera.transform.right * inputMotionValue.x;
            characterController.Move(entity.EntityData.BaseRuntimeData.CurMotionDir * Time.deltaTime * entity.EntityData.BaseRuntimeData.CurMotionSpeed);
            if (entity.EntityData.BaseRuntimeData.CurMotionDir != Vector3.zero) {
                Cond.Instance.Get(entity, Label.Assemble(Label.MOTION, Label.EVENT))?.Invoke();
            }
        }

        public override void Clear() {
            base.Clear();
            InputRegister.Instance.UnLoad(InputRegister.Instance.Motion, MotionEvent);
            Data.Instance.OnUpdateEvent.RemoveListener(OnUpdate);
        }
    }
}