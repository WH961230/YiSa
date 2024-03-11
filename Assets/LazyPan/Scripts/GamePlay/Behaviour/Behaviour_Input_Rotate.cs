using UnityEngine;
using UnityEngine.InputSystem;

namespace LazyPan {
    public class Behaviour_Input_Rotate : Behaviour {
        private RectTransform CursorRect;
        private bool isRightMouseHold;

        private Vector3 mousePositionToWorld;
        private RaycastHit hit;
        private bool isHitFloor;

        public Behaviour_Input_Rotate(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            InputRegister.Instance.Load(InputRegister.Instance.MouseRightPress, MouseRightPress);
            CursorRect = Cond.Instance.Get<Transform>(UI.Instance.Get("UI_Fight"), Label.CURSOR).GetComponent<RectTransform>();
            CursorRect.gameObject.SetActive(false);
            isRightMouseHold = false;
            Data.Instance.OnUpdateEvent.AddListener(OnUpdate);
            Data.Instance.OnLateUpdateEvent.AddListener(OnLateUpdate);
        }

        private void MouseRightPress(InputAction.CallbackContext obj) {
            if (!Data.Instance.CanControl) {
                return;
            }
            if (obj.performed) {
                isRightMouseHold = true;
                CursorRect.gameObject.SetActive(true);
            } else if (obj.canceled) {
                isRightMouseHold = false;
                CursorRect.gameObject.SetActive(false);
            }
        }

        private void OnUpdate() {
            CheckHitFloor();
        }

        private void OnLateUpdate() {
            RotateToHitPoint();
        }

        private void CheckHitFloor() {
            if (isRightMouseHold) {
                Camera camera = Cond.Instance.Get<Camera>(Cond.Instance.GetCameraEntity(), Label.CAMERA);
                Vector2 mousePosition = Mouse.current.position.ReadValue();
                Vector3 currentMousePositionToWorld = camera.ScreenToWorldPoint(mousePosition);
                if (mousePositionToWorld != currentMousePositionToWorld) {
                    Ray ray = camera.ScreenPointToRay(mousePosition);
                    isHitFloor = Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Floor"));
                    if (isHitFloor) {
                        Vector3 hitPointScreen = camera.WorldToScreenPoint(hit.point);
                        CursorRect.position = new Vector2(hitPointScreen.x, hitPointScreen.y);
                    }

                    mousePositionToWorld = currentMousePositionToWorld;
                }
            } else {
                isHitFloor = false;
            }
        }

        private void RotateToHitPoint() {
            if (entity.EntityData.BaseRuntimeData.CurMotionState == 2) {
                return;
            }

            //击中地板
            if (isHitFloor) {
                //相机
                Camera camera = Cond.Instance.Get<Camera>(Cond.Instance.GetCameraEntity(), Label.CAMERA);
                //瞄准偏移点
                Vector3 aimOffPointVec = Cond.Instance.Get<Transform>(entity, Label.AIMOFFSETPOINT).position;
                //击中点在相机坐标系的点
                Vector3 hitScreenPoint = camera.WorldToScreenPoint(hit.point); //击中的点
                //偏移瞄准点在相机坐标系的点
                Vector3 aimOffPointToScreenVec = camera.WorldToScreenPoint(aimOffPointVec); //玩家头部
                //击中的点到角色头部
                Vector3 hitPointToAimOffScreenVec = camera.WorldToScreenPoint(new Vector3(hit.point.x, aimOffPointVec.y, hit.point.z));
                //向量
                Vector3 v1 = (hitScreenPoint - aimOffPointToScreenVec).normalized;
                Vector3 v2 = (hitPointToAimOffScreenVec - aimOffPointToScreenVec).normalized;
                //偏移角度
                float angle = Vector3.Angle(v1, v2);
                angle = Mathf.Abs(angle);
                Vector3 targetAimVec = (hit.point - aimOffPointVec).normalized;
                Vector3 tempForward = Vector3.ProjectOnPlane(targetAimVec, Vector3.up);
                tempForward = Quaternion.AngleAxis(angle, Vector3.Cross(v1, v2).y > 0 ? Vector3.up : Vector3.down) * tempForward;

                if (tempForward != Vector3.zero) {
                    Quaternion toRotation = Quaternion.LookRotation(tempForward, Vector3.up);
                    Cond.Instance.Get<Transform>(entity, Label.BODY).rotation = Quaternion.RotateTowards(
                        Cond.Instance.Get<Transform>(entity, Label.BODY).rotation, toRotation,
                        entity.EntityData.BaseRuntimeData.CurRotateSpeed * Time.deltaTime);
                    entity.EntityData.BaseRuntimeData.CurRotateDir = tempForward;
                }
            } else {
                if (entity.EntityData.BaseRuntimeData.CurRotateDir != Vector3.zero) {
                    Quaternion toRotation = Quaternion.LookRotation(entity.EntityData.BaseRuntimeData.CurRotateDir, Vector3.up);
                    Cond.Instance.Get<Transform>(entity, Label.BODY).rotation = Quaternion.RotateTowards(
                        Cond.Instance.Get<Transform>(entity, Label.BODY).rotation, toRotation,
                        entity.EntityData.BaseRuntimeData.CurRotateSpeed * Time.deltaTime);
                }
            }
        }

        public override void Clear() {
            base.Clear();
            InputRegister.Instance.UnLoad(InputRegister.Instance.MouseRightPress, MouseRightPress);
            Data.Instance.OnUpdateEvent.RemoveListener(OnUpdate);
            Data.Instance.OnLateUpdateEvent.RemoveListener(OnLateUpdate);
        }
    }
}