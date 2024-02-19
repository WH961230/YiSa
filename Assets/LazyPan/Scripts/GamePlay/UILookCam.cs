using UnityEngine;

public class UILookCam : MonoBehaviour {
    void Start() {
    }

    void Update() {
    }

    private void LateUpdate() {
        transform.forward = Camera.main.gameObject.transform.forward;
    }
}