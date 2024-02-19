using UnityEngine;

public class Lab_CamFollow : MonoBehaviour {
    public GameObject CameraGo;
    public CharacterController characterController;
    public Transform PlayerTr;
    private Vector3 off;
    void Start() {
        off = transform.position - PlayerTr.position;
    }

    void Update() {
        Vector2 dir = Vector3.zero;
        dir.x += Input.GetKey(KeyCode.W) ? 1 : 0;
        dir.x += Input.GetKey(KeyCode.S) ? -1 : 0;
        dir.y += Input.GetKey(KeyCode.A) ? -1 : 0;
        dir.y += Input.GetKey(KeyCode.D) ? 1 : 0;

        if (dir != Vector2.zero) {
            Vector3 cameraForward = CameraGo.transform.forward;
            cameraForward.y = 0; // 将y轴设为0，保持在水平面上

            Vector3 moveDir = Vector3.zero;
            moveDir += cameraForward * dir.x * 5f;
            moveDir += gameObject.transform.right * dir.y * 5f;
            characterController.Move(moveDir * Time.deltaTime);
        }
    }

    private void LateUpdate() {
        transform.position = Vector3.Lerp(transform.position, PlayerTr.position + off, 5 * Time.deltaTime);
    }
}