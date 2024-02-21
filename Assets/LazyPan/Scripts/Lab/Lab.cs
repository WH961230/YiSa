using LazyPan;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lab : MonoBehaviour {
    public CharacterController characterController;
    public RectTransform CursorRect;
    public Comp PlayerComp;
    
    private Vector3 mousePositionToWorld;
    private RaycastHit hit;
    private bool isHitFloor;

    void Start() {
        InputRegister.Instance.Load("Player/MouseRight", MouseRight);
        InputRegister.Instance.Load("Player/MouseRightPress", MouseRightPress);
    }

    private void MouseRight(InputAction.CallbackContext obj) {
        if (obj.performed) {
            Debug.Log("right mouse");
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            isHitFloor = Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Floor"));
        }
    }

    private void MouseRightPress(InputAction.CallbackContext obj) {
        if (obj.performed) {
            Debug.Log("right mouse press");
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            isHitFloor = Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Floor"));
        } else {
            isHitFloor = false;
        }
    }

    void Update() {
        CursorRect.gameObject.SetActive(isHitFloor);
        if (CursorRect.gameObject.activeSelf) {
            Vector3 hitPointScreen = Camera.main.WorldToScreenPoint(hit.point);
            CursorRect.position = new Vector2(hitPointScreen.x, hitPointScreen.y);
        }
    }

    private void LateUpdate() {
        if (isHitFloor) {
            Vector3 pointVec = PlayerComp.Get<Transform>("Point").position;
            Vector3 worldToScreenPoint = Camera.main.WorldToScreenPoint(hit.point);//击中的点
            Vector3 pointToScreenVec = Camera.main.WorldToScreenPoint(pointVec);//玩家头部
            Vector3 hitPointToScreenVec = Camera.main.WorldToScreenPoint(new Vector3(hit.point.x, pointVec.y, hit.point.z));//击中的点到角色头部
            Vector3 v1 = (worldToScreenPoint - pointToScreenVec).normalized;
            Vector3 v2 = (hitPointToScreenVec - pointToScreenVec).normalized;
           
            float angle = Vector3.Angle(v1, v2);
            angle = Mathf.Abs(angle);
            Vector3 targetAimVec = (hit.point - pointVec).normalized;
            Vector3 tempForward = Vector3.ProjectOnPlane(targetAimVec, Vector3.up);
            tempForward = Quaternion.AngleAxis(angle, Vector3.Cross(v1, v2).y > 0 ? Vector3.up : Vector3.down) * tempForward;
            characterController.transform.forward = Vector3.MoveTowards(characterController.transform.forward,
                tempForward, Time.deltaTime * 10);
        }
    }
}