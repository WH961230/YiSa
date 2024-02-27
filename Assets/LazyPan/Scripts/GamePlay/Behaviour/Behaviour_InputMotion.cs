using UnityEngine;
using UnityEngine.InputSystem;

namespace LazyPan {
    public class Behaviour_InputMotion : Behaviour {
        private Vector3 motionDir;
        private CharacterController characterController;
        private Entity mainCameraEntity;

        public Behaviour_InputMotion(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            InputRegister.Instance.Load(InputRegister.Instance.Motion, MotionEvent);
            characterController = entity.Comp.Get<CharacterController>("CharacterController");
            Data.Instance.OnUpdateEvent.AddListener(OnUpdate);
        }

        private void MotionEvent(InputAction.CallbackContext obj) {
            if (!Data.Instance.CanControl) {
                return;
            }
            motionDir = obj.ReadValue<Vector2>();
        }

        void OnUpdate() {
            GetMainCamera();
            PlayerMotion();
        }

        private void GetMainCamera() {
            if (mainCameraEntity == null) {
                Data.Instance.TryGetEntityByObjType(ObjType.MainCamera, out mainCameraEntity);
            }
        }

        private void PlayerMotion() {
            if (mainCameraEntity == null) {
                return;
            }
            Vector3 cameraForward = Camera.main.transform.forward;
            cameraForward.y = 0;
            entity.EntityData.MotionDir = Vector3.zero;
            entity.EntityData.MotionDir += cameraForward * motionDir.y * 5f;
            entity.EntityData.MotionDir += Camera.main.transform.right * motionDir.x * 5f;
            characterController.Move(entity.EntityData.MotionDir * Time.deltaTime);
            if (entity.EntityData.MotionDir != Vector3.zero) {
                entity.Comp.GetEvent("PlayerMotionEvent")?.Invoke();
            }
        }

        public override void OnClear() {
            base.OnClear();
            InputRegister.Instance.UnLoad(InputRegister.Instance.Motion, MotionEvent);
            Data.Instance.OnUpdateEvent.RemoveListener(OnUpdate);
        }
    }
}