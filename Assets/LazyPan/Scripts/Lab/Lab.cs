using LazyPan;
using UnityEngine;
using UnityEngine.UI;

public class Lab : MonoBehaviour {
    public CharacterController characterController;
    public Image CursorImg;
    public Comp PlayerComp;
    
    private Vector3 mousePositionToWorld;
    private RaycastHit hit;
    private bool isHitFloor;
    private RectTransform cursorRect;

    void Start() {
        cursorRect = CursorImg.GetComponent<RectTransform>();
    }

    void Update() {
        if (Input.GetMouseButtonDown(1)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            isHitFloor = Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Floor"));
        }

        if (Input.GetMouseButton(1)) {
            if (mousePositionToWorld != Camera.main.ScreenToWorldPoint(Input.mousePosition)) {
                mousePositionToWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                isHitFloor = Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Floor"));
            }
        } else {
            isHitFloor = false;
        }

        CursorImg.gameObject.SetActive(isHitFloor);

        if (CursorImg.gameObject.activeSelf) {
            Vector3 hitPointScreen = Camera.main.WorldToScreenPoint(hit.point);
            cursorRect.position = new Vector2(hitPointScreen.x, hitPointScreen.y);
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