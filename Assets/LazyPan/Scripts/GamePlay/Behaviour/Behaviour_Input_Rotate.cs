﻿using UnityEngine;
using UnityEngine.InputSystem;

namespace LazyPan {
    public class Behaviour_Input_Rotate : Behaviour {
        private RectTransform CursorRect;
        private bool isRightMouseHold;

        private Vector3 mousePositionToWorld;
        private RaycastHit hit;
        private bool isHitFloor;
        private CharacterController characterController;

        public Behaviour_Input_Rotate(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            InputRegister.Instance.Load(InputRegister.Instance.MouseRightPress, MouseRightPress);
            CursorRect = Cond.Instance.Get<Transform>(UI.Instance.Get("UI_Fight"), Label.CURSOR).GetComponent<RectTransform>();
            CursorRect.gameObject.SetActive(false);
            characterController = Cond.Instance.Get<CharacterController>(entity, Label.CHARACTERCONTROLLER);
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
            if (isHitFloor) {
                Camera camera = Cond.Instance.Get<Camera>(Cond.Instance.GetCameraEntity(), Label.CAMERA);
                Vector3 pointVec = Cond.Instance.Get<Transform>(entity, Label.AIMOFFSETPOINT).position;
                Vector3 worldToScreenPoint = camera.WorldToScreenPoint(hit.point); //击中的点
                Vector3 pointToScreenVec = camera.WorldToScreenPoint(pointVec); //玩家头部
                Vector3 hitPointToScreenVec = camera.WorldToScreenPoint(new Vector3(hit.point.x, pointVec.y, hit.point.z)); //击中的点到角色头部
                Vector3 v1 = (worldToScreenPoint - pointToScreenVec).normalized;
                Vector3 v2 = (hitPointToScreenVec - pointToScreenVec).normalized;
                float angle = Vector3.Angle(v1, v2);
                angle = Mathf.Abs(angle);
                Vector3 targetAimVec = (hit.point - pointVec).normalized;
                Vector3 tempForward = Vector3.ProjectOnPlane(targetAimVec, Vector3.up);
                tempForward = Quaternion.AngleAxis(angle, Vector3.Cross(v1, v2).y > 0 ? Vector3.up : Vector3.down) * tempForward;
                characterController.transform.forward = Vector3.MoveTowards(characterController.transform.forward,
                    tempForward, Time.deltaTime * entity.EntityData.BaseRuntimeData.CurRotateSpeed);
            } else {
                if (entity.EntityData.BaseRuntimeData.CurMotionDir != Vector3.zero) {
                    characterController.transform.forward = Vector3.MoveTowards(characterController.transform.forward,
                        entity.EntityData.BaseRuntimeData.CurMotionDir, Time.deltaTime * entity.EntityData.BaseRuntimeData.CurRotateSpeed);
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