using UnityEngine;
using UnityEngine.InputSystem;

namespace LazyPan {
    public class Behaviour_Input_Motion : Behaviour {
        private Vector3 inputMotionValue;
        private CharacterController characterController;
        private Animator animator;

        public Behaviour_Input_Motion(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            InputRegister.Instance.Load(InputRegister.Instance.Motion, MotionEvent);
            characterController = Cond.Instance.Get<CharacterController>(entity, Label.CHARACTERCONTROLLER);
            animator = Cond.Instance.Get<Animator>(entity, Label.ANIMATOR);
            Data.Instance.OnUpdateEvent.AddListener(OnUpdate);
        }

        private void MotionEvent(InputAction.CallbackContext obj) {
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

            /*冲刺中*/
            if (entity.EntityData.BaseRuntimeData.CurMotionState == 2) {
                return;
            }

            if (inputMotionValue.y != 0 || inputMotionValue.x != 0) {
                Vector3 cameraForward = camera.transform.forward;
                cameraForward.y = 0;
                entity.EntityData.BaseRuntimeData.CurMotionDir = Vector3.zero;
                entity.EntityData.BaseRuntimeData.CurMotionDir += cameraForward * inputMotionValue.y;
                entity.EntityData.BaseRuntimeData.CurMotionDir += camera.transform.right * inputMotionValue.x;
                characterController.Move(entity.EntityData.BaseRuntimeData.CurMotionDir * Time.deltaTime *
                                         entity.EntityData.BaseRuntimeData.CurMotionSpeed);
                if (entity.EntityData.BaseRuntimeData.CurMotionDir != Vector3.zero) {
                    Cond.Instance.Get(entity, Label.Assemble(Label.MOTION, Label.EVENT))?.Invoke();
                }

                entity.EntityData.BaseRuntimeData.CurRotateDir = entity.EntityData.BaseRuntimeData.CurMotionDir;
                entity.EntityData.BaseRuntimeData.CurMotionState = 1;
            } else {
                entity.EntityData.BaseRuntimeData.CurMotionState = 0;
            }
        }

        public override void Clear() {
            base.Clear();
            InputRegister.Instance.UnLoad(InputRegister.Instance.Motion, MotionEvent);
            Data.Instance.OnUpdateEvent.RemoveListener(OnUpdate);
        }
    }
}