using UnityEngine;

public class Gravity : MonoBehaviour {
    [Tooltip("物体")] public Transform ObjectTr;
    [Tooltip("控制器")] public CharacterController characterController;

    public bool IsGround = false;
    public float OverlapCapsuleOffset;
    [Tooltip("重力")] public float GravitySpeed;
    public LayerMask GravityDetectMaskLayer;

    private Vector3 currentGravityDir; //重力方向
    private Vector3 controllerMoveDir;
    private Collider[] colliders;

    private void Update() {
        GravityEvent();
    }

    private void GravityEvent() {
        Vector3 bottom = ObjectTr.position + ObjectTr.up * characterController.radius + ObjectTr.up * OverlapCapsuleOffset;
        Vector3 top = ObjectTr.position + ObjectTr.up * characterController.height - ObjectTr.up * characterController.radius;
        colliders = Physics.OverlapCapsule(bottom, top, characterController.radius, GravityDetectMaskLayer);
        IsGround = colliders.Length > 0;
        currentGravityDir = IsGround ? Vector3.zero : Vector3.down;

        controllerMoveDir = Vector3.zero;
        controllerMoveDir += currentGravityDir * GravitySpeed;

        if (controllerMoveDir.magnitude > 0) {
            characterController.Move(controllerMoveDir * Time.deltaTime);
        }
    }
}