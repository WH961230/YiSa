using UnityEngine;
using UnityEngine.InputSystem;

namespace LazyPan {
    public class Behaviour_AlignCameraMovement : Behaviour {
        private bool isShift;
        private CharacterController characterController;
        private Animator animator;
        private Camera mainCamera;
        private float footStepDeploy;
        private float timeDeploy;
        private Vector2 movementInputVec;

        public Behaviour_AlignCameraMovement(Entity entity, string sign) : base(entity, sign) {
            //输入注册
            InputRegister.Instance.Load(InputRegister.Instance.Shift, InputShift);
            //移动
            InputRegister.Instance.Load(InputRegister.Instance.Movement, InputMovement);
            //获取角色控制器
            characterController = entity.Comp.Get<CharacterController>("CharacterController");
            //角色动画状态机
            animator = entity.Comp.Get<Animator>("Animator");
            //主相机
            mainCamera = Camera.main;
            //注册移动更新
            Data.Instance.OnUpdateEvent.AddListener(OnMovementUpdate);
        }

        private void InputShift(InputAction.CallbackContext obj) {
            isShift = true;
        }

        private void InputMovement(InputAction.CallbackContext obj) {
            movementInputVec = obj.ReadValue<Vector2>();
            if (obj.canceled) {
                movementInputVec = Vector2.zero;
            }
        }

        private void OnMovementUpdate() {
            if (characterController != null) {
                //根据相机的方向计算移动方向 获取运动向量
                Vector3 cameraVec = mainCamera.transform.forward;
                cameraVec.y = 0;
                Vector3 moveDirection = movementInputVec.y * cameraVec * entity.EntitySetting.MovementSpeed;
                moveDirection += movementInputVec.x * mainCamera.transform.right * entity.EntitySetting.MovementSpeed;
                //尘土特效
                if (moveDirection != Vector3.zero) {
                    if (footStepDeploy > 0) {
                        footStepDeploy -= Time.deltaTime;
                    } else {
                        GameObject footStepFxGo = GameObject.Instantiate(entity.EntityData.FootStepFx);
                        footStepFxGo.transform.position = entity.Prefab.transform.position + Vector3.up * 0.2f;
                        footStepDeploy = 0.2f;
                        GameObject.Destroy(footStepFxGo, 2f);
                    }

                    timeDeploy = 0.01f;
                    animator.SetBool("Walk", true);
                    animator.SetFloat("MotionSpeed", isShift ? 2 : 1);

                    //移动
                    characterController.Move(moveDirection * Time.deltaTime * entity.EntityData.MovementSpeed);
                } else {
                    if (timeDeploy > 0) {
                        timeDeploy -= Time.deltaTime;
                    } else {
                        timeDeploy = 0;
                        animator.SetBool("Walk", false);
                    }
                }

                Vector3 entityVec = entity.Prefab.transform.position;
                Vector3 headVec = entity.Prefab.transform.position + Vector3.up * 1.155f;
                Vector3 cursorVec = Cursor.Instance.hitPoint;
                Vector3 cursorHeadVec = new Vector3(Cursor.Instance.hitPoint.x,
                    (entity.Prefab.transform.position + Vector3.up * 1.155f).y, Cursor.Instance.hitPoint.z);

                Vector3 toVec = Camera.main.WorldToScreenPoint(cursorVec) - Camera.main.WorldToScreenPoint(headVec);
                Vector3 fromVec = Camera.main.WorldToScreenPoint(cursorHeadVec) - Camera.main.WorldToScreenPoint(headVec);

                float angleSign = Vector3.Cross(fromVec, toVec).y < 0 ? 1 : -1 ;//正往右 负往左
                float angle = Vector3.Angle(fromVec, toVec) - angleSign * 3;//偏移补偿3度
                angle *= angleSign;

                Vector3 lookForwardVec = (cursorVec - headVec).normalized;
                lookForwardVec.y = 0;
                lookForwardVec = Quaternion.AngleAxis(angle, Vector3.up) * lookForwardVec;
                entity.Prefab.transform.forward = Vector3.Lerp(entity.Prefab.transform.forward, lookForwardVec,
                    Time.deltaTime * entity.EntitySetting.RotateSpeed);

                //计算当前面向的方向 和 移动的方向的角度
                float engle = Vector3.Angle(entity.Prefab.transform.forward, moveDirection.normalized);
                float sign = 1;
                //判断移动方向和朝向的左右关系
                if (Vector3.Cross(entity.Prefab.transform.forward, moveDirection.normalized).y < 0) {
                    sign = -1;
                }

                float tempHor = Mathf.Sin(sign * engle * Mathf.Deg2Rad);
                float tempVer = Mathf.Cos(sign * engle * Mathf.Deg2Rad);
                if (isShift) {
                    tempHor *= 2;
                    tempVer *= 2;
                }

                animator.SetFloat("Horizontal", Mathf.Lerp(animator.GetFloat("Horizontal"), tempHor, Time.deltaTime * 5));
                animator.SetFloat("Vertical", Mathf.Lerp(animator.GetFloat("Vertical"), tempVer, Time.deltaTime * 5));
            }
        }

        public override void OnClear() {
            base.OnClear();
            Data.Instance.OnUpdateEvent.RemoveListener(OnMovementUpdate);
            InputRegister.Instance.UnLoad(InputRegister.Instance.Shift, InputShift);
            InputRegister.Instance.UnLoad(InputRegister.Instance.Movement, InputMovement);
        }
    }
}