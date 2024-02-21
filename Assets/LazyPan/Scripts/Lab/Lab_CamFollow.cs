using UnityEngine;

public class Lab_CamFollow : MonoBehaviour {
    public CharacterController characterController;
    public Transform PlayerTr;
    private Vector3 off;

    void Start() {
        off = transform.position - PlayerTr.position;
    }

    private void LateUpdate() {
        transform.position = Vector3.Lerp(transform.position, PlayerTr.position + off, 5 * Time.deltaTime);
    }
}