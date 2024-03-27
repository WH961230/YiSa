using UnityEngine;
using UnityEngine.InputSystem;

namespace LazyPan {
    public class Behaviour_Input_AlignCameraMovement : Behaviour {
        private Vector2 input;

        public Behaviour_Input_AlignCameraMovement(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            InputRegister.Instance.Load(InputRegister.Instance.Motion, GetInput);
            Data.Instance.OnUpdateEvent.AddListener(Movement);
        }
        
        /*获取输入*/
        private void GetInput(InputAction.CallbackContext obj) {
            input = obj.ReadValue<Vector2>();
        }

        /*获取移动物*/
        private CharacterController GetTarget() {
            return Cond.Instance.Get<CharacterController>(entity, Label.CHARACTERCONTROLLER);
        }

        /*获取相机*/
        private Transform GetCamera() {
            return Cond.Instance.Get<Camera>(Cond.Instance.GetCameraEntity(), Label.CAMERA).transform;
        }

        /*获取速度*/
        private float GetSpeed() {
            return entity.EntityData.BaseRuntimeData.CurMotionSpeed;
        }

        /*获取输入对齐相机方向*/
        private Vector3 GetInputAlignCameraDir() {
            Transform camera = GetCamera();
            Vector3 cameraForward = camera.forward;
            cameraForward.y = 0;
            Vector3 cameraRight = camera.right;
            cameraRight.y = 0;
            return cameraForward.normalized * input.y + cameraRight.normalized * input.x;
        }

        /*获取位移方向*/
        private Vector3 GetMovementDir() {
            return entity.EntityData.BaseRuntimeData.CurMotionDir;
        }

        /*设置位移方向*/
        private void SetMovementDir(Vector3 dir) {
            entity.EntityData.BaseRuntimeData.CurMotionDir = dir;
        }

        /*可以移动*/
        private bool CanMovement() {
            return !Data.Instance.CanControl;
        }

        /*获取动画控制器*/
        private Animator GetAnimator() {
            return Cond.Instance.Get<Animator>(entity, Label.Assemble(Label.BODY, Label.ANIMATOR));
        }

        /*移动*/
        private void Movement() {
            if (entity.EntityData == null) {
                return;
            }
            if (CanMovement()) {
                CharacterController cc = GetTarget();
                float speed = GetSpeed();
                Vector3 dir = GetInputAlignCameraDir();
                SetMovementDir(dir);
                dir = GetMovementDir();
                cc.Move(speed * Time.deltaTime * dir);
            }
        }

        public override void Clear() {
            base.Clear();
            Data.Instance.OnUpdateEvent.RemoveListener(Movement);
        }
    }
}