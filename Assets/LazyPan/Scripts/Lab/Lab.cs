using LazyPan;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class Lab : MonoBehaviour {
    public CharacterController characterController;
    public RectTransform CursorRect;
    public Comp PlayerComp;
    public PlayableDirector Director;
    public float PlayerRotateSpeed;
    public float BulletShootSpeed;

    public UnityEvent PlayerShootEvent = new UnityEvent();
    public UnityEvent PlayerMoveEvent = new UnityEvent();

    private Vector3 mousePositionToWorld;
    private RaycastHit hit;
    private bool isRightMouseHold;
    private bool isHitFloor;
    private Vector3 motionDir;
    private bool canControl;

    void Start() {
        InputRegister.Instance.Load("Player/MouseRightPress", MouseRightPress);
        InputRegister.Instance.Load("Player/MouseLeft", MouseLeft);
        InputRegister.Instance.Load("Player/Motion", MotionEvent);
        Director.Play();
        Director.stopped += CheckStartControl;
        isRightMouseHold = false;
        CursorRect.gameObject.SetActive(false);
    }

    private void MouseLeft(InputAction.CallbackContext obj) {
        if (obj.performed) {
            Shoot();
            PlayerShootEvent?.Invoke();
        }
    }
    
    private void MouseRightPress(InputAction.CallbackContext obj) {
        if (obj.performed) {
            isRightMouseHold = true;
            CursorRect.gameObject.SetActive(true);
        } else if (obj.canceled) {
            isRightMouseHold = false;
            CursorRect.gameObject.SetActive(false);
        }
    }

    private void MotionEvent(InputAction.CallbackContext obj) {
        motionDir = obj.ReadValue<Vector2>();
    }

    void Update() {
        CheckHitFloor();
        PlayerMotion();
    }

    private void LateUpdate() {
        RotateToHitPoint();
    }

    private void CheckStartControl(PlayableDirector director) {
        canControl = true;
    }

    private void PlayerMotion() {
        if (!canControl) {
            return;
        }

        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0;
        Vector3 moveDir = Vector3.zero;
        moveDir += cameraForward * motionDir.y * 5f;
        moveDir += Camera.main.transform.right * motionDir.x * 5f;
        characterController.Move(moveDir * Time.deltaTime);
        if (moveDir != Vector3.zero) {
            PlayerMoveEvent?.Invoke();
        }
    }

    private void Shoot() {
        if (!canControl) {
            return;
        }

        GameObject template = PlayerComp.Get<GameObject>("BulletPrefab");
        Transform bulletMuzzle = PlayerComp.Get<Transform>("Muzzle");
        GameObject bullet = Instantiate(template);
        bullet.transform.position = bulletMuzzle.position;
        bullet.transform.rotation = bulletMuzzle.rotation;
        bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * BulletShootSpeed, ForceMode.Impulse);
    }

    private void CheckHitFloor() {
        if (!canControl) {
            return;
        }

        if (isRightMouseHold) {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Vector3 currentMousePositionToWorld = Camera.main.ScreenToWorldPoint(mousePosition);
            if (mousePositionToWorld != currentMousePositionToWorld) {
                Ray ray = Camera.main.ScreenPointToRay(mousePosition);
                isHitFloor = Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Floor"));
                if (isHitFloor) {
                    Vector3 hitPointScreen = Camera.main.WorldToScreenPoint(hit.point);
                    CursorRect.position = new Vector2(hitPointScreen.x, hitPointScreen.y);
                }

                mousePositionToWorld = currentMousePositionToWorld;
            }
        } else {
            isHitFloor = false;
        }
    }

    private void RotateToHitPoint() {
        if (!canControl) {
            return;
        }

        if (isHitFloor) {
            Vector3 pointVec = PlayerComp.Get<Transform>("Point").position;
            Vector3 worldToScreenPoint = Camera.main.WorldToScreenPoint(hit.point); //击中的点
            Vector3 pointToScreenVec = Camera.main.WorldToScreenPoint(pointVec); //玩家头部
            Vector3 hitPointToScreenVec =
                Camera.main.WorldToScreenPoint(new Vector3(hit.point.x, pointVec.y, hit.point.z)); //击中的点到角色头部
            Vector3 v1 = (worldToScreenPoint - pointToScreenVec).normalized;
            Vector3 v2 = (hitPointToScreenVec - pointToScreenVec).normalized;
            float angle = Vector3.Angle(v1, v2);
            angle = Mathf.Abs(angle);
            Vector3 targetAimVec = (hit.point - pointVec).normalized;
            Vector3 tempForward = Vector3.ProjectOnPlane(targetAimVec, Vector3.up);
            tempForward = Quaternion.AngleAxis(angle, Vector3.Cross(v1, v2).y > 0 ? Vector3.up : Vector3.down) *
                          tempForward;
            characterController.transform.forward = Vector3.MoveTowards(characterController.transform.forward,
                tempForward, Time.deltaTime * PlayerRotateSpeed);
        }
    }
}