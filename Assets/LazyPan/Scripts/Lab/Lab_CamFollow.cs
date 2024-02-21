using UnityEngine;
using UnityEngine.InputSystem;

public class Lab_CamFollow : MonoBehaviour {
    public GameObject CameraGo;
    public CharacterController characterController;
    public Transform PlayerTr;
    private Vector3 off;
    private Vector3 motionDir;
    void Start() {
        off = transform.position - PlayerTr.position;
        LazyPan.InputRegister.Instance.Load("Player/Motion", MotionEvent);
    }

    private void MotionEvent(InputAction.CallbackContext obj) {
        motionDir = obj.ReadValue<Vector2>();
    }

    void Update() {
        Vector2 dir = Vector3.zero;
        if (dir != Vector2.zero) {
            Vector3 cameraForward = CameraGo.transform.forward;
            cameraForward.y = 0; // 将y轴设为0，保持在水平面上

            Vector3 moveDir = Vector3.zero;
            moveDir += cameraForward * motionDir.x * 5f;
            moveDir += gameObject.transform.right * dir.y * 5f;
            characterController.Move(moveDir * Time.deltaTime);
        }
    }

    private void LateUpdate() {
        transform.position = Vector3.Lerp(transform.position, PlayerTr.position + off, 5 * Time.deltaTime);
    }
}