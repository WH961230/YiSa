using UnityEngine;
using UnityEngine.InputSystem;

namespace LazyPan {
    public class Behaviour_InputMotion : Behaviour {
        private Vector3 inputMotionValue;
        private CharacterController characterController;
        private Entity myCameraEntity;

        public Behaviour_InputMotion(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
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
            GetMyCamera();
            PlayerMotion();
        }

        private void GetMyCamera() {
            if (myCameraEntity == null) {
                Data.Instance.TryGetEntityByType(entity.EntityData.BaseRuntimeData.CameraType, out myCameraEntity);
            }
        }

        private void PlayerMotion() {
            if (myCameraEntity == null) {
                return;
            }

            Vector3 cameraForward = myCameraEntity.Prefab.transform.forward;
            cameraForward.y = 0;
            entity.EntityData.BaseRuntimeData.CurMotionDir = Vector3.zero;
            entity.EntityData.BaseRuntimeData.CurMotionDir += cameraForward * inputMotionValue.y * 5f;
            entity.EntityData.BaseRuntimeData.CurMotionDir += myCameraEntity.Prefab.transform.right * inputMotionValue.x * 5f;
            characterController.Move(entity.EntityData.BaseRuntimeData.CurMotionDir * Time.deltaTime * entity.EntityData.BaseRuntimeData.CurMotionSpeed);
            if (entity.EntityData.BaseRuntimeData.CurMotionDir != Vector3.zero) {
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