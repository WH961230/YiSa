using LazyPan;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public Camera Camera;
    public Transform FollowTran;
    public Transform CameraRootTran;
    public Vector3 FollowOffset;
    public float CameraFollowSpeed;
    public float CameraLookSpeed;

    private Quaternion cameraDefaultQua;
    void Start() {
        cameraDefaultQua = Camera.transform.rotation;
    }

    private void LateUpdate() {
        if (FollowTran == null) {
            GameObject playerGo = GameObject.FindWithTag("Player");
            if (playerGo != null) {
                FollowTran = playerGo.GetComponent<Player>().transform;
            }
        } else {
            Camera.transform.position = Vector3.Lerp(Camera.transform.position, CameraRootTran.position + FollowTran.position + FollowOffset, Time.deltaTime * CameraFollowSpeed);
            Camera.transform.rotation = Quaternion.Lerp(Camera.transform.rotation, CameraRootTran.rotation * cameraDefaultQua, Time.deltaTime * CameraLookSpeed);
        }
    }
}