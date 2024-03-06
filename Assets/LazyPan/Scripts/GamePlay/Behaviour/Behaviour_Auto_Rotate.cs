using UnityEngine;
using UnityEngine.InputSystem;

namespace LazyPan {
    public class Behaviour_Auto_Rotate : Behaviour {
        private RectTransform CursorRect;
        private Vector3 mousePositionToWorld;
        private RaycastHit hit;
        private bool isHitFloor;
        private CharacterController characterController;
        private Camera camera;

        public Behaviour_Auto_Rotate(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            characterController = entity.Comp.Get<CharacterController>("CharacterController");
            CursorRect = UI.Instance.Get("UI_Fight").Get<Transform>("Cursor").GetComponent<RectTransform>();
            CursorRect.gameObject.SetActive(true);
            Data.Instance.OnUpdateEvent.AddListener(OnUpdate);
            Data.Instance.OnLateUpdateEvent.AddListener(OnLateUpdate);
            Data.Instance.TryGetEntityByType(entity.EntityData.BaseRuntimeData.CameraType, out Entity cameraEntity);
            camera = cameraEntity.Comp.Get<Camera>("Camera");
        }

        private void OnUpdate() {
            CheckHitFloor();
        }

        private void OnLateUpdate() {
            RotateTowardsMouseWorldPoint();
        }

        private void CheckHitFloor() {
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
        }

        private void RotateTowardsMouseWorldPoint() {
            Vector3 pointVec = entity.Comp.Get<Transform>("Point").position;
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
            Quaternion targetRotation = Quaternion.LookRotation(tempForward, Vector3.up);
            characterController.transform.rotation = Quaternion.RotateTowards(characterController.transform.rotation, targetRotation, entity.EntityData.BaseRuntimeData.CurRotateSpeed * Time.deltaTime);
        }

        public override void Clear() {
            base.Clear();
            Data.Instance.OnUpdateEvent.RemoveListener(OnUpdate);
            Data.Instance.OnLateUpdateEvent.RemoveListener(OnLateUpdate);
        }
    }
}